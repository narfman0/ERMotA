using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace ERMotA
{
    class RandomQuest
    {
        public string CurrentRandomQuestDialog;
        public int TimeCurrentRandomQuestDialog;
        public int RandomQuestFlag;
        public bool SageQuestInitiated;
        Game1 Game1Reference;
        public RandomQuest(Game1 GameRef)
        {
            TimeCurrentRandomQuestDialog=Environment.TickCount;
            Game1Reference = GameRef;
            SageQuestInitiated = false;
            RandomQuestFlag = 0;
        }
        public void Update()
        {
            /*
             * RandomQuestFlag - 
             * -1 is no current mission
             * 0 is mission accomplished
             * any number higher is the amount of things left to complete
             */
            if (Environment.TickCount - TimeCurrentRandomQuestDialog > 300000)
            {   //abort quest
                if (CurrentRandomQuestDialog != null &&
                    (CurrentRandomQuestDialog.Substring(0, 18) == "A group of bandits" ||
                    CurrentRandomQuestDialog.Substring(0, 21) == "An evil monster stole"))
                {
                    for (int z = 0; z < Game1.MAX_ENEMIES; ++z)
                        Game1Reference.enemy[z].Flag = 0;
                    RandomQuestFlag = -1;
                }
            }
            if (RandomQuestFlag < 1)
            {
                float distanceSage1 = Game1Reference.Fast2d(Game1Reference.Player.Pos, Game1Reference.NPC[20].Pos);
                float distanceSage2 = Game1Reference.Fast2d(Game1Reference.Player.Pos, Game1Reference.NPC[22].Pos);
                float distanceSage3 = Game1Reference.Fast2d(Game1Reference.Player.Pos, Game1Reference.NPC[21].Pos);
                if (RandomQuestFlag == -1 && SageQuestInitiated &&
                    distanceSage1 < 2.0f ||
                    distanceSage2 < 2.0f ||
                    distanceSage3 < 2.0f)
                {
                    TimeCurrentRandomQuestDialog = Environment.TickCount;
                    RandomQuestFlag = 0;
                    switch (Game1Reference.rand.Next() % 2)   //find what random quest you wish to do
                    {
                        case 0:     //a random kill group of enemies quest
                            {
                                //Randomly generate whether they are north, south, east, west
                                Vector2 GroupPos = Game1Reference.Player.Pos;
                                switch (Game1Reference.rand.Next() % 4)
                                {
                                    case 0:
                                        GroupPos.X += 2.0f - Game1Reference.rand.Next() % 5;
                                        GroupPos.Y += 12.0f + Game1Reference.rand.Next() % 6;
                                        CurrentRandomQuestDialog = "A group of bandits South.\nPlease go slay them.";
                                        break;
                                    case 1:
                                        GroupPos.X += 2.0f - Game1Reference.rand.Next() % 5;
                                        GroupPos.Y -= 12.0f + Game1Reference.rand.Next() % 6;
                                        CurrentRandomQuestDialog = "A group of bandits North.\nPlease go slay them.";
                                        break;
                                    case 2:
                                        GroupPos.X += 12.0f + Game1Reference.rand.Next() % 6;
                                        GroupPos.Y += 2.0f - Game1Reference.rand.Next() % 5;
                                        CurrentRandomQuestDialog = "A group of bandits East.\nPlease go slay them.";
                                        break;
                                    case 3:
                                        GroupPos.X -= 12.0f + Game1Reference.rand.Next() % 6;
                                        GroupPos.Y += 2.0f - Game1Reference.rand.Next() % 5;
                                        CurrentRandomQuestDialog = "A group of bandits West.\nPlease go slay them.";
                                        break;
                                }
                                int TotalEnemiesInQuest = 3 + Game1Reference.rand.Next() % 4;
                                for (int NumRandQuestEnemies = 0; NumRandQuestEnemies < TotalEnemiesInQuest; ++NumRandQuestEnemies)
                                {
                                    Game1Reference.SpawnEnemy(NumRandQuestEnemies);
                                    RandomQuestFlag++;
                                    Game1Reference.enemy[NumRandQuestEnemies].Flag = 1;
                                    Game1Reference.enemy[NumRandQuestEnemies].Pos = GroupPos;
                                    Game1Reference.enemy[NumRandQuestEnemies].Pos.X += NumRandQuestEnemies % 3;
                                    if (NumRandQuestEnemies > 3)
                                        Game1Reference.enemy[NumRandQuestEnemies].Pos.Y += NumRandQuestEnemies % 2;
                                }
                                break;
                            }
                        case 1:
                            {
                                //Randomly generate whether they are north, south, east, west
                                Vector2 Pos = Game1Reference.Player.Pos;
                                switch (Game1Reference.rand.Next() % 4)
                                {
                                    case 0:
                                        Pos.X += 2.0f - Game1Reference.rand.Next() % 5;
                                        Pos.Y += 12.0f + Game1Reference.rand.Next() % 6;
                                        CurrentRandomQuestDialog = "An evil monster stole my\ngold. Please kill him and\nbring it back. He went South.";
                                        break;
                                    case 1:
                                        Pos.X += 2.0f - Game1Reference.rand.Next() % 5;
                                        Pos.Y -= 12.0f + Game1Reference.rand.Next() % 6;
                                        CurrentRandomQuestDialog = "An evil monster stole my\ngold. Please kill him and\nbring it back. He went North.";
                                        break;
                                    case 2:
                                        Pos.X += 12.0f + Game1Reference.rand.Next() % 6;
                                        Pos.Y += 2.0f - Game1Reference.rand.Next() % 5;
                                        CurrentRandomQuestDialog = "An evil monster stole my\ngold. Please kill him and\nbring it back. He went East.";
                                        break;
                                    case 3:
                                        Pos.X -= 12.0f + Game1Reference.rand.Next() % 6;
                                        Pos.Y += 2.0f - Game1Reference.rand.Next() % 5;
                                        CurrentRandomQuestDialog = "An evil monster stole my\ngold. Please kill him and\nbring it back. He went West.";
                                        break;
                                }
                                Game1Reference.SpawnEnemy(0);
                                RandomQuestFlag++;
                                Game1Reference.enemy[0].Flag = 1;
                                Game1Reference.enemy[0].Pos = Pos;
                                Game1Reference.enemy[0].Level += 2;
                                Game1Reference.enemy[0].Dexterity += 10;
                                Game1Reference.enemy[0].Strength += 10;
                                Game1Reference.enemy[0].Speed += 10;
                                break;
                            }
                    }
                }
                else if (RandomQuestFlag == 0 &&
                    distanceSage1 < 2.0f ||
                    distanceSage2 < 2.0f ||
                    distanceSage3 < 2.0f)//you completed a quest
                {
                    RandomQuestFlag = -1;
                    SageQuestInitiated = false;
                    Game1Reference.Player.XP += 100;
#if(!WINDOWS)
                    foreach (NetworkGamer gamer in Game1Reference.Network.Session.RemoteGamers)
                    {
                        Game1Reference.Network.AddXP(100);
                    }
#endif
                }
            }
        }
    }
}