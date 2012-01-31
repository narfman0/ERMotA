using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;

namespace ERMotA
{
    /// <summary>
    /// The network class. Call update every frame to pump network.
    /// 
    /// Message types:
    /// 0 - position updates
    /// 1 - enemy position updates, partitioned / 3
    /// 2 - map updates, sent once when gamer joins
    /// 3 - attack update - say what player attacked what enemy
    /// 4 - experience packet, only from host TODO
    /// 5 - attack packet for how much an enemy damaged a player
    /// 6 - send player level, strength, dex, speed, etc
    /// </summary>
    public class Network
    {
        #region Declarations
        public AvailableNetworkSessionCollection AvailableSessions;
        Game1 game1;
        public int MaxPlayers = 8;
        PacketWriter packetWriter;
        PacketReader packetReader;
        public NetworkSession Session;
        int TimeSinceLastJoinGame;
        int TimeLastPosUpdate;
        #endregion
        #region Network Functions
        public Network(Game1 GameReference)
        {
            game1 = GameReference;
            TimeLastPosUpdate = Environment.TickCount;
            packetWriter = new PacketWriter();
            packetReader = new PacketReader();
            TimeSinceLastJoinGame = Environment.TickCount;
        }
        public void AddXP(Int16 XP)
        {
            packetWriter.Write((Int16)4);
            packetWriter.Write(XP);
        }
        public void EnemyAttack(int HashCode, int PlayerDamage)
        {
            packetWriter.Write((Int16)5);
            packetWriter.Write(HashCode);
            packetWriter.Write(PlayerDamage);
        }
        public void FindGames()
        {
            ShutDown();
            try
            {
                NetworkSessionProperties searchProperties = new NetworkSessionProperties();
                AvailableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, searchProperties);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public void JoinGame(int Selected)
        {
            try
            {
                Session = NetworkSession.Join(AvailableSessions[Selected]);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public void CreateGame()
        {
            try
            {
                ShutDown();
                Session = NetworkSession.Create(NetworkSessionType.SystemLink, 1, MaxPlayers);
                Session.AllowHostMigration = true;
                Session.AllowJoinInProgress = true;
                Session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(Session_GamerJoined);
                Session.StartGame();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public void PlayerAttack(int index)
        {
            packetWriter.Write((Int16)3);
            packetWriter.Write((Int16)index);
        }
        public void PlayerStatsUpdate(LivingEntity Player)
        {
            packetWriter.Write((Int16)6);
            packetWriter.Write(Player.Level);
            packetWriter.Write(Player.Dexterity);
            packetWriter.Write(Player.Speed);
            packetWriter.Write(Player.Strength);
            packetWriter.Write(Player.CurrentAnimation);
            packetWriter.Write(Player.CurrentFrame);
            packetWriter.Write(Player.isAttacking);
            if (Player.isAttacking)
            {
                packetWriter.Write(Player.CurrentAnimationAttack);
                packetWriter.Write(Player.CurrentFrameAttack);
            }
        }
        void Session_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            Vector2 Pos = new Vector2(Map.MapSize / 2, Map.MapSize - 8);
            e.Gamer.Tag = new LivingEntity(Pos);

            if (!e.Gamer.IsHost)
            {
                packetWriter.Write((Int16)2);
                for (int i = 0; i < Map.MapSize; ++i)
                    for (int j = 0; j < Map.MapSize; ++j)
                        packetWriter.Write((Int16)game1.GameMap.MapArray[i, j]);
                try
                {
                    Session.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder, e.Gamer);
                }
                catch (Exception ex)
                {   //why fail, why? I dont want to send it to everyone!
                    ex.ToString();
                }
            }
        }
        public void ShutDown()
        {
            if(Session!=null)
                try
                {
                    if (Session.SessionState == NetworkSessionState.Playing &&
                        Session.AllGamers.Count == 1)
                    {
                        Session.EndGame();
                        Session.Dispose();
                        Session = null;
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
        }
        public void Update()
        {
            if (Session != null)
            {
                try
                {
                    Session.Update();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                foreach (LocalNetworkGamer gamer in Session.LocalGamers)
                {
                    #region Send network data
                    //write YOUR player data
                    gamer.Tag = game1.Player;
                    packetWriter.Write((Int16)0);
                    packetWriter.Write(game1.Player.Pos);

                    //write HOST data
                    if (Session.IsHost)
                    {
                        packetWriter.Write((Int16)1);   //1 = enemy updates
                        for (int i = 0; i < Game1.MAX_ENEMIES; ++i)
                        {
                            packetWriter.Write(game1.enemy[i].Pos);
                            packetWriter.Write(game1.enemy[i].CurrentAnimation);
                            packetWriter.Write(game1.enemy[i].CurrentFrame);
                            packetWriter.Write((Int16)game1.enemy[i].Flag);
                        }
                    }
                    foreach (NetworkGamer remoteGamer in Session.RemoteGamers)
                        gamer.SendData(packetWriter, SendDataOptions.InOrder, remoteGamer);
                    packetWriter.Flush();
                    packetWriter = new PacketWriter();
                    #endregion
                    #region Get network data
                    while (gamer.IsDataAvailable)
                    {
                        NetworkGamer sender;
                        gamer.ReceiveData(packetReader, out sender);
                        if (!sender.IsLocal)
                        {
                            LivingEntity remotePlayer = sender.Tag as LivingEntity;
                            if (remotePlayer == null)
                            {
                                Vector2 Pos = new Vector2(Map.MapSize / 2, Map.MapSize - 8);
                                remotePlayer = new LivingEntity(Pos);
                            }
                            while (packetReader.Position < packetReader.Length)
                                switch (packetReader.ReadInt16())   //get message type
                                {
                                    case 0: //remote player pos update
                                        {
                                            remotePlayer.Pos = packetReader.ReadVector2();
                                            break;
                                        }
                                    
                                    case 1:
                                        {
                                            for (int i = 0; i < Game1.MAX_ENEMIES; ++i)
                                            {
                                                game1.enemy[i].Pos = packetReader.ReadVector2();
                                                game1.enemy[i].CurrentAnimation = packetReader.ReadInt16();
                                                game1.enemy[i].CurrentFrame = packetReader.ReadInt16();
                                                game1.enemy[i].Flag = packetReader.ReadInt16();
                                            }
                                            break;
                                        }
                                    case 2:
                                        {   //receive map!
                                            for (int i = 0; i < Map.MapSize; ++i)
                                                for (int j = 0; j < Map.MapSize; ++j)
                                                    game1.GameMap.MapArray[i, j] = packetReader.ReadInt16();
                                            break;
                                        }
                                    case 3: //only host responds to attacks
                                        {
                                            if (Session.IsHost)
                                            {
                                                int e = packetReader.ReadInt16();
                                                remotePlayer.Attack(game1.enemy[e]);
                                                if (game1.enemy[e].CurrentHealth < 1)    //death
                                                {
                                                    packetWriter.Write((Int16)4);
                                                    Int16 XPToAdd = (Int16)(game1.enemy[e].Level * (game1.enemy[e].Dexterity + 
                                                        game1.enemy[e].Speed + game1.enemy[e].Strength) / 3);
                                                    packetWriter.Write((Int16)XPToAdd);
                                                    game1.Player.XP += XPToAdd;
                                                    game1.enemy[e].Pos.X = 236 + game1.rand.Next() % 40;
                                                    game1.enemy[e].Pos.Y = 484 + game1.rand.Next() % 20;
                                                }
                                            }
                                            break;
                                        }
                                    case 4: //xp additions
                                        {
                                            game1.Player.XP += packetReader.ReadInt16();
                                            break;
                                        }
                                    case 5:
                                        {
                                            if(gamer.Gamertag.GetHashCode() == packetReader.ReadInt32())
                                                game1.Player.CurrentHealth -= packetReader.ReadInt16();
                                            else
                                                packetReader.ReadInt16();
                                            break;
                                        }
                                    case 6:
                                        {
                                            remotePlayer.Level = packetReader.ReadInt16();
                                            remotePlayer.Dexterity = packetReader.ReadInt16();
                                            remotePlayer.Speed = packetReader.ReadInt16();
                                            remotePlayer.Strength = packetReader.ReadInt16();
                                            remotePlayer.CurrentAnimation = packetReader.ReadInt16();
                                            remotePlayer.CurrentFrame = packetReader.ReadInt16();
                                            remotePlayer.isAttacking = packetReader.ReadBoolean();
                                            if (remotePlayer.isAttacking)
                                            {
                                                remotePlayer.CurrentAnimationAttack = packetReader.ReadInt16();
                                                remotePlayer.CurrentFrameAttack = packetReader.ReadInt16();
                                            }
                                            break;
                                        }
                                }
                            sender.Tag = remotePlayer;
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion
    }
}