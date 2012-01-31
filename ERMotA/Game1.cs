using System;
using System.Collections.Generic;
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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Class Declarations
        int CurrentMenuOption;
        public int CurrentScreenMode;  //0 is main menu, 1 is ingame, 2 is joining games
        int currentSong = 0;
        public LivingEntity.EnemyEntity[] enemy;
        public Map GameMap;
        GraphicsDeviceManager graphics;
        SpriteFont Font1;
        Vector2 FontPos;
        int lastButtonPressed;
        MediaLibrary library;
#if(ZUNE)
        public const int MAX_ENEMIES = 20;
#else
        public const int MAX_ENEMIES = 40;
#endif
#if(ZUNE)
        public const float MAX_ENEMY_DISTANCE_FROM_PLAYER = 32.0f;
#else
        public const float MAX_ENEMY_DISTANCE_FROM_PLAYER = 100.0f;
#endif
        const int MAX_SOUNDS = 20;
        public Network Network;
        public LivingEntity.PlayerEntity Player;
        public Random rand;
        int SCREEN_WIDTH, SCREEN_HEIGHT;
        SpriteBatch spriteBatch;
        bool MusicOn;
        SongCollection songs;
        SoundEffect[] sounds;
        SoundEffectInstance[] soundsPlaying;
        Storage StorageUnit;
        RandomQuest RandomQuest;
        int attackPressed = 0;
        int interactPressed = 0;
        int TimeLastScreenModeChange;
        int TimeLastMenuChange;

        string[] storyMessage;
        int messagetime;
        int currentMessage;
        public const int MAX_MESSAGE_TIME = 120;
        public const int MAX_MESSAGES = 200;
        
        public static Vector2 LocationBHamHam = new Vector2(Map.MapSize / 2, Map.MapSize - Map.MapSize / 32);
        public static Vector2 LocationJRoberton = new Vector2(32, 32);
        public static Vector2 LocationDickinsongrad = new Vector2(Map.MapSize - Map.MapSize / 8, Map.MapSize / 2);
        public static Vector2 LocationGargoyle = new Vector2(Map.MapSize / 2 + 2, Map.MapSize / 2);

        //NPC stuff
        public LivingEntity.NPCEntity[] NPC;
        const int MAX_NPCS = 30;

        private Texture2D DesertTexture;
        private Texture2D DirtTexture;
        private Texture2D EnemyFrontTexture;
        private Texture2D GrassTexture;
        private Texture2D Grass2Texture;
        private Texture2D HealthBarTexture;
        private Texture2D HealthBarOutlineTexture;
        private Texture2D PlayerFrontTexture;
        private Texture2D QuestArrowTexture;
        private Texture2D Tree1Texture;
        public static Texture2D TextureAttackLight1of5;
        public static Texture2D TextureAttackLight2of5;
        public static Texture2D TextureAttackLight3of5;
        public static Texture2D TextureAttackLight4of5;
        public static Texture2D TextureAttackLight5of5;
        #region Declare Frog Textures
        public static Texture2D TexFrogEast1;
        public static Texture2D TexFrogEast2;
        public static Texture2D TexFrogEast3;
        public static Texture2D TexFrogEast4;
        public static Texture2D TexFrogNorth1;
        public static Texture2D TexFrogNorth2;
        public static Texture2D TexFrogNorth3;
        public static Texture2D TexFrogNorth4;
        public static Texture2D TexFrogSouth1;
        public static Texture2D TexFrogSouth2;
        public static Texture2D TexFrogSouth3;
        public static Texture2D TexFrogSouth4;
        public static Texture2D TexFrogWest1;
        public static Texture2D TexFrogWest2;
        public static Texture2D TexFrogWest3;
        public static Texture2D TexFrogWest4;
        #endregion
        #region Declare WaterThug Textures

        public static Texture2D TexWaterThugEast1;
        public static Texture2D TexWaterThugEast2;
        public static Texture2D TexWaterThugEast3;
        public static Texture2D TexWaterThugNorth1;
        public static Texture2D TexWaterThugNorth2;
        public static Texture2D TexWaterThugNorth3;
        public static Texture2D TexWaterThugSouth1;
        public static Texture2D TexWaterThugSouth2;
        public static Texture2D TexWaterThugSouth3;
        public static Texture2D TexWaterThugWest1;
        public static Texture2D TexWaterThugWest2;
        public static Texture2D TexWaterThugWest3;
        #endregion
        #region Declare Zombie Textures
        public static Texture2D TexZombieEast1;
        public static Texture2D TexZombieEast2;
        public static Texture2D TexZombieNorth1;
        public static Texture2D TexZombieNorth2;
        public static Texture2D TexZombieNorth3;
        public static Texture2D TexZombieSouth1;
        public static Texture2D TexZombieSouth2;
        public static Texture2D TexZombieSouth3;
        public static Texture2D TexZombieWest1;
        public static Texture2D TexZombieWest2;
        #endregion
        #region Declare Needlion Textures

        public static Texture2D TexNeedlionEast1;
        public static Texture2D TexNeedlionEast2;
        public static Texture2D TexNeedlionEast3;
        public static Texture2D TexNeedlionNorth1;
        public static Texture2D TexNeedlionNorth2;
        public static Texture2D TexNeedlionNorth3;
        public static Texture2D TexNeedlionSouth1;
        public static Texture2D TexNeedlionSouth2;
        public static Texture2D TexNeedlionSouth3;
        public static Texture2D TexNeedlionWest1;
        public static Texture2D TexNeedlionWest2;
        public static Texture2D TexNeedlionWest3;
        #endregion
        #region Declare DarkStalker Textures
        public static Texture2D TexDarkStalkerEast1;
        public static Texture2D TexDarkStalkerEast2;
        public static Texture2D TexDarkStalkerEast3;
        public static Texture2D TexDarkStalkerNorth1;
        public static Texture2D TexDarkStalkerNorth2;
        public static Texture2D TexDarkStalkerNorth3;
        public static Texture2D TexDarkStalkerSouth1;
        public static Texture2D TexDarkStalkerSouth2;
        public static Texture2D TexDarkStalkerSouth3;
        public static Texture2D TexDarkStalkerWest1;
        public static Texture2D TexDarkStalkerWest2;
        public static Texture2D TexDarkStalkerWest3;
        #endregion
        #region Declare Sabin Textures
        public static Texture2D TexSabinNorth1of4;
        public static Texture2D TexSabinNorth2of4;
        public static Texture2D TexSabinNorth3of4;
        public static Texture2D TexSabinNorth4of4;
        public static Texture2D TexSabinEast1of4;
        public static Texture2D TexSabinEast2of4;
        public static Texture2D TexSabinEast3of4;
        public static Texture2D TexSabinEast4of4;
        public static Texture2D TexSabinSouth1of4;
        public static Texture2D TexSabinSouth2of4;
        public static Texture2D TexSabinSouth3of4;
        public static Texture2D TexSabinSouth4of4;
        public static Texture2D TexSabinWest1of4;
        public static Texture2D TexSabinWest2of4;
        public static Texture2D TexSabinWest3of4;
        public static Texture2D TexSabinWest4of4;

        #endregion
        #region Declare Link Textures
        public static Texture2D TexLinkSouth1of8;
        public static Texture2D TexLinkSouth2of8;
        public static Texture2D TexLinkSouth3of8;
        public static Texture2D TexLinkSouth4of8;
        public static Texture2D TexLinkSouth5of8;
        public static Texture2D TexLinkSouth6of8;
        public static Texture2D TexLinkSouth7of8;
        public static Texture2D TexLinkSouth8of8;

        public static Texture2D TexLinkNorth1of8;
        public static Texture2D TexLinkNorth2of8;
        public static Texture2D TexLinkNorth3of8;
        public static Texture2D TexLinkNorth4of8;
        public static Texture2D TexLinkNorth5of8;
        public static Texture2D TexLinkNorth6of8;
        public static Texture2D TexLinkNorth7of8;
        public static Texture2D TexLinkNorth8of8;

        public static Texture2D TexLinkEast1of8;
        public static Texture2D TexLinkEast2of8;
        public static Texture2D TexLinkEast3of8;
        public static Texture2D TexLinkEast4of8;
        public static Texture2D TexLinkEast5of8;
        public static Texture2D TexLinkEast6of8;
        public static Texture2D TexLinkEast7of8;
        public static Texture2D TexLinkEast8of8;

        public static Texture2D TexLinkWest1of8;
        public static Texture2D TexLinkWest2of8;
        public static Texture2D TexLinkWest3of8;
        public static Texture2D TexLinkWest4of8;
        public static Texture2D TexLinkWest5of8;
        public static Texture2D TexLinkWest6of8;
        public static Texture2D TexLinkWest7of8;
        public static Texture2D TexLinkWest8of8;

        #endregion
        private Texture2D WaterTexture;
        private Texture2D PenguinTexture;
        private Texture2D AmuletTexture;
        private Texture2D SwordTexture;
        private Texture2D GargoyleTexture;
        private Texture2D SageTexture;
        private Texture2D GargoyleEyesTexture;
        public const int MAX_PUZZLE_GARGOYLES = 4;
        public bool[] gargoyleActivated;
        public int[] gargoyleActivationOrder;
        public int numGargoylesActivated;
        bool puzzleSolved = false;
        private Texture2D BattleAxeTexture;
        private Texture2D CatapultTexture;
        private Texture2D JesterTexture;
        private Texture2D SantaTexture;
        private Texture2D TreasureChestTexture;
        public int treasureGotten;
        private Texture2D MenuBackgroundTexture;
        private Texture2D[] WoodsBlacksmithTexture;
        private Texture2D[] WoodsHouseTexture;
        private Texture2D[] DesertBlacksmithTexture;
        private Texture2D[] DesertHouseTexture;
        //private Texture2D PenguinTexture;
        private Texture2D InteractTexture;

        //minimap
        private Texture2D MinimapTexture;
        private Texture2D MinimapBuildingTexture;
        private Texture2D CircleGreenTexture;
        int minimapSelection = 0;

        //minimap textures (1x1)
        private Texture2D PixelGrassTexture;
        private Texture2D PixelWaterTexture;
        private Texture2D PixelDesertTexture;
        private Texture2D PixelDirtTexture;
        private Texture2D PixelTreeTexture;

        bool visitedJRoberton = false;
        bool visitedBHamHam = false;
        bool visitedDickinsongrad = false;
        bool visitedGargoyle = false;

        #endregion

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Vector2 RenderPos, textVector;
            string Title1, Title2, NewGame, JoinGame, Exit;
            int k;
            switch(CurrentScreenMode)
            {
                // main menu
                #region CurrentScreenMode == 0
                case 0:
                    RenderPos = new Vector2(0.0f, 0.0f);
                    spriteBatch.Draw(MenuBackgroundTexture, RenderPos, Color.White);

                    Title1 = "Ever-Rotating Machine";
                    FontPos.X = SCREEN_WIDTH / 2;   FontPos.Y = SCREEN_HEIGHT / 4 - 16;
                    RenderPos = Font1.MeasureString(Title1) / 2;
                    spriteBatch.DrawString(Font1, Title1, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    Title2 = "of the Apocalypse";
                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 4;
                    RenderPos = Font1.MeasureString(Title2) / 2;
                    spriteBatch.DrawString(Font1, Title2, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    NewGame = "New Game";
                    JoinGame = "Join Game";
                    string LoadChar = "Load Character";
                    Exit = "Exit";

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 - 16;
                    RenderPos = Font1.MeasureString(NewGame) / 2;
                    if (CurrentMenuOption == 0)
                        spriteBatch.DrawString(Font1, NewGame, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, NewGame, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2;
                    RenderPos = Font1.MeasureString(JoinGame) / 2;
                    if (CurrentMenuOption == 1)
                        spriteBatch.DrawString(Font1, JoinGame, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, JoinGame, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 + 16;
                    RenderPos = Font1.MeasureString(LoadChar) / 2;
                    if (CurrentMenuOption == 2)
                        spriteBatch.DrawString(Font1, LoadChar, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, LoadChar, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 + 32;
                    RenderPos = Font1.MeasureString(Exit) / 2;
                    if (CurrentMenuOption == 3)
                        spriteBatch.DrawString(Font1, Exit, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Exit, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    break;
                #endregion
                // gameplay
                #region CurrentScreenMode == 1
                case 1:
                    #region Draw Ground Tiles
                    int i, j;  //generic iterators
                    Vector2 WorldPos = Vector2.Zero;
#if (ZUNE)
                    for (i = -1; i < 9; ++i)
                        for (j = -1; j < 11; ++j)
#else
                    for (i = -32; i < 32; ++i)
                        for (j = -32; j < 32; ++j)
#endif
                        {
                            WorldPos.X = i + (int)(Math.Floor(Player.Pos.X + .5f)) - SCREEN_WIDTH / 64;
                            WorldPos.Y = j + (int)(Math.Floor(Player.Pos.Y + .5f)) - SCREEN_HEIGHT / 64;
                            if((int)WorldPos.X > 0 && (int)WorldPos.X < Map.MapSize &&
                                (int)WorldPos.Y > 0 && (int)WorldPos.Y < Map.MapSize)
                            switch (GameMap.MapArray[(int)WorldPos.X, (int)WorldPos.Y])
                            {
                                #region case0-99, terrain
                                case 0:
                                    RenderRelativeTexture(GrassTexture, WorldPos);
                                    break;
                                case 1:
                                    RenderRelativeTexture(Grass2Texture, WorldPos);
                                    break;
                                case 2:
                                    RenderRelativeTexture(Tree1Texture, WorldPos);
                                    break;
                                case 3:
                                    RenderRelativeTexture(DesertTexture, WorldPos);
                                    break;
                                case 5:
                                    RenderRelativeTexture(WaterTexture, WorldPos);
                                    break;
                                case 6:
                                    RenderRelativeTexture(DirtTexture, WorldPos);
                                    break;
                                #endregion
                                #region case100-103, house
                                case 100:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsHouseTexture[0], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertHouseTexture[0], WorldPos);
                                        break;
                                    }
                                case 101:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsHouseTexture[1], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertHouseTexture[1], WorldPos);
                                        break;
                                    }
                                case 102:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsHouseTexture[2], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertHouseTexture[2], WorldPos);
                                        break;
                                    }
                                case 103:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsHouseTexture[3], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertHouseTexture[3], WorldPos);
                                        break;
                                    }
                                #endregion
                                #region case110-113, blacksmith
                                case 110:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsBlacksmithTexture[0], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertBlacksmithTexture[0], WorldPos);
                                        break;
                                    }
                                case 111:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsBlacksmithTexture[1], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertBlacksmithTexture[1], WorldPos);
                                        break;
                                    }
                                case 112:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsBlacksmithTexture[2], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertBlacksmithTexture[2], WorldPos);
                                        break;
                                    }
                                case 113:
                                    {
                                        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                                            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                                            RenderRelativeTexture(WoodsBlacksmithTexture[3], WorldPos);
                                        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                                            RenderRelativeTexture(DesertBlacksmithTexture[3], WorldPos);
                                        break;
                                    }
                                #endregion
                            }
                        }
                    #endregion
                    #region Draw Enemies
                    for (i = 0; i < MAX_ENEMIES; ++i)
                    {
                        if (enemy[i].CurrentHealth > 0)
                            RenderEntityTexture(enemy[i]);
                        if(enemy[i].Flag > 0)
                        {
                            FontPos.X = enemy[i].Pos.X; 
                            FontPos.Y = enemy[i].Pos.Y-1.0f;
                            RenderRelativeTexture(QuestArrowTexture, FontPos);
                        }

                    }
                    #endregion
                    #region Draw NPCs

                    for (i = 0; i < NPC.Length; i++)
                    {
                        if(NPC[i].alive)
                        {
                            switch (i)
                            {
                                case 0:
                                    RenderPos = RenderPosCalc(NPC[i].Pos);
                                    Rectangle RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                        (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                        32, 32);
                                    spriteBatch.Draw(PenguinTexture, RenderRect, Color.White);
                                    break;
                                case 1:
                                    RenderPos = RenderPosCalc(NPC[i].Pos);
                                    RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                        (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                        32, 32);
                                    spriteBatch.Draw(AmuletTexture, RenderRect, Color.White);
                                    break;
                                case 2:
                                    RenderPos = RenderPosCalc(NPC[i].Pos);
                                    RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                        (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                        32, 32);
                                    spriteBatch.Draw(SwordTexture, RenderRect, Color.White);
                                    break;
                                case 3:
                                    RenderPos = RenderPosCalc(NPC[i].Pos);
                                    RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                        (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                        32, 32);
                                    spriteBatch.Draw(PlayerFrontTexture, RenderRect, Color.White);
                                    break;
                                case 4:
                                    RenderPos = RenderPosCalc(NPC[i].Pos);
                                    RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                        (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                        71, 64);
                                    spriteBatch.Draw(GargoyleTexture, RenderRect, Color.White);
                                    break;
                                case 20:
                                    RenderRelativeTexture(SageTexture, NPC[i].Pos);
                                    break;
                                case 21:
                                    RenderRelativeTexture(SageTexture, NPC[i].Pos);
                                    break;
                                case 22:
                                    RenderRelativeTexture(SageTexture, NPC[i].Pos);
                                    break;
                                case 9:
                                   RenderPos = RenderPosCalc(NPC[i].Pos);
                                   RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                       (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                       24, 47);
                                   spriteBatch.Draw(BattleAxeTexture, RenderRect, Color.White);
                                   break;
                                case 25:
                                   RenderPos = RenderPosCalc(NPC[i].Pos);
                                   RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                       (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                       48, 48);
                                   spriteBatch.Draw(SantaTexture, RenderRect, Color.White);
                                   break;
                                case 26:
                                   RenderPos = RenderPosCalc(NPC[i].Pos);
                                   RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                       (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                       75, 70);
                                   spriteBatch.Draw(CatapultTexture, RenderRect, Color.White);
                                   break;
                                case 27://jester
                                   RenderPos = RenderPosCalc(NPC[i].Pos);
                                   RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                       (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                       42, 64);
                                   spriteBatch.Draw(JesterTexture, RenderRect, Color.White);
                                   break;
                                default:
                                    if(i >=5 && i <= 8)
                                    {
                                       RenderPos = RenderPosCalc(NPC[i].Pos);
                                       RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                           (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                           71, 64);
                                       if(gargoyleActivated[i-5])
                                       {
                                           spriteBatch.Draw(GargoyleEyesTexture, RenderRect, Color.White);
                                           //RenderRelativeTexture(GargoyleEyesTexture, RenderPos);
                                       }
                                       else
                                       {
                                           spriteBatch.Draw(GargoyleTexture, RenderRect, Color.White);
                                       }
                                       
                                    }
                                   if (i >= 10 && i <= 19)
                                   {
                                       RenderPos = RenderPosCalc(NPC[i].Pos);
                                       RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                           (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                           64, 58);
                                       spriteBatch.Draw(TreasureChestTexture, RenderRect, Color.White);
                                       //break;
                                   }
                                   break;
                            }

                        }
                        if (Fast2d(Player.Pos, NPC[i].Pos) < 2 && NPC[i].alive)
                        {
                            RenderPos = RenderPosCalc(NPC[i].Pos + new Vector2(0, 1.5f));

                            Rectangle RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                                (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                                32, 32);
                            spriteBatch.Draw(InteractTexture, RenderRect, Color.White);
                        }
                    }
                    #endregion
                    #region Draw Gamers
                    if (Network.Session != null)
                        foreach (NetworkGamer gamer in Network.Session.RemoteGamers)
                        {
                            LivingEntity player = gamer.Tag as LivingEntity;
                            if (player != null)
                            {
                                // RenderRelativeTexture(PlayerFrontTexture, player.Pos);
                                RenderRelativeTexture(Player.texAnimation[player.CurrentAnimation][player.CurrentFrame], player.Pos);
                                if (player.isAttacking)
                                    RenderRelativeAttackTexture(Player.Abilities[player.CurrentAnimationAttack].texAbility[player.CurrentFrameAttack], player.Pos);
                                // Draw a gamertag label.
                                string label = gamer.Gamertag;
                                if (gamer.IsHost)
                                    label += " (server)";
                                RenderPos = RenderPosCalc(player.Pos);
                                Vector2 labelOffset = new Vector2(Font1.MeasureString(label).X / 2, 32 + Font1.MeasureString(label).Y);
                                if (!(Network.Session.LocalGamers[0].Gamertag == gamer.Gamertag))
                                    spriteBatch.DrawString(Font1, label, RenderPos, Color.Black, 0, labelOffset, 0.7f, SpriteEffects.None, 0);
                            }
                        }
                    RenderEntityTexture(Player);
                    if (Player.isAttacking)
                        RenderRelativeAttackTexture(Player.Abilities[Player.CurrentAnimationAttack].texAbility[Player.CurrentFrameAttack], Player.Pos);
                    #endregion
                    #region Draw HUD

                    FontPos.X = 8; FontPos.Y = 8;
                    string Health = "Health: " + Player.CurrentHealth.ToString();
                    spriteBatch.DrawString(Font1, Health, FontPos, Color.DarkKhaki,
                        0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = 8; FontPos.Y = 24;
                    string Level = "Level: " + Player.Level.ToString();
                    spriteBatch.DrawString(Font1, Level, FontPos, Color.DarkKhaki,
                        0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

                    if (Player.UnassignedStatPoints > 0)
                    {
                        FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = 8;
                        string Levelup = "Level Up!";
                        Vector2 RenderPozzizzle = Font1.MeasureString(Levelup) / 2; ;
                        spriteBatch.DrawString(Font1, Levelup, FontPos, Color.DarkKhaki,
                            0, RenderPozzizzle, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    if (RandomQuest.RandomQuestFlag > 0 && Environment.TickCount - RandomQuest.TimeCurrentRandomQuestDialog < 10000)
                    {
                        FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = 128;
                        Vector2 RenderPozzizzle = Font1.MeasureString(RandomQuest.CurrentRandomQuestDialog) / 2; ;
                        spriteBatch.DrawString(Font1, RandomQuest.CurrentRandomQuestDialog, FontPos, Color.DarkKhaki,
                            0, RenderPozzizzle, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    /*
                    Vector2 textVector = new Vector2(15, 20);
                    Rectangle hudstuff = new Rectangle();
                    hudstuff.X = 10;
                    hudstuff.Y = 10;
                    hudstuff.Width = 54;
                    hudstuff.Height = 14;
                    spriteBatch.Draw(HealthBarOutlineTexture, hudstuff, Color.White);
                    hudstuff.X = 12;
                    hudstuff.Y = 12;
                    hudstuff.Width = 50;
                    hudstuff.Height = 10;
                    spriteBatch.Draw(HealthBarTexture, hudstuff, Color.White);
                    spriteBatch.DrawString(Font1, Player.XP.ToString() + " XP", textVector, Color.White);
                     */
                    if (messagetime > 0)
                    {
                        messagetime--;
                        textVector = new Vector2(5, 273);
                        try
                        {
                            //if (currentMessage == 100)
                            //{
                            //    storyMessage[currentMessage] = "Treasure: " + treasureGotten + "/10";
                            //}
                            spriteBatch.DrawString(Font1, storyMessage[currentMessage], textVector, Color.White);
                        }
                        catch
                        {

                        }
                        //storyMessage[currentMessage]
                        //spriteBatch.DrawString(Font1, storyMessage[currentMessage], textVector, Color.White);

                    }
                    #endregion
                    break;
                #endregion
                // join session
                #region CurrentScreenMode == 2
                case 2:
                    if (Network.AvailableSessions == null ||
                        Network.AvailableSessions.Count == 0)
                    {
                        FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 - 12);
                        RenderPos = Font1.MeasureString("Searching for games...") / 2;
                        spriteBatch.DrawString(Font1, "Searching for games...", FontPos, Color.DarkKhaki,
                            0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    else
                    {
                        try
                        {
                            for (k = 0; k < Network.AvailableSessions.Count; k++)
                            {
                                FontPos = new Vector2(SCREEN_WIDTH / 2, 32 + k * 16);
                                RenderPos = Font1.MeasureString(Network.AvailableSessions[k].HostGamertag) / 2;
                                if (CurrentMenuOption == k)
                                    spriteBatch.DrawString(Font1, Network.AvailableSessions[k].HostGamertag, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                                else
                                    spriteBatch.DrawString(Font1, Network.AvailableSessions[k].HostGamertag, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                            }
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                    }
                    break;
                #endregion
                // in-game menu
                #region CurrentScreenMode == 3
                case 3:
                    textVector = new Vector2(5, 273);
                    try
                    {
                        //if (currentMessage == 100)
                        //{
                        //    storyMessage[currentMessage] = "Treasure: " + treasureGotten + "/10";
                        //}
                        spriteBatch.DrawString(Font1, storyMessage[currentMessage], textVector, Color.White);
                    }
                    catch
                    {

                    }
                    Title1 = "Menu";
                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 4 - 16);
                    RenderPos = Font1.MeasureString(Title1) / 2;
                    spriteBatch.DrawString(Font1, Title1, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    //string Title2 = "of the Apocalypse";
                    //FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 4);
                    //RenderPos = Font1.MeasureString(Title2) / 2;
                    //spriteBatch.DrawString(Font1, Title2, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    #region Options
                    string CharScreen = "Character Screen";
                    NewGame = "Resume Game";
                    JoinGame = "Change Music";
                    string MusicOnOff = "Turn Music On/Off";
                    string QuitCurrentGame = "Quit Current Game";
                    Exit = "Exit";

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 - 32);
                    RenderPos = Font1.MeasureString(NewGame) / 2;
                    if (CurrentMenuOption == 0)
                        spriteBatch.DrawString(Font1, NewGame, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, NewGame, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 - 16);
                    RenderPos = Font1.MeasureString(JoinGame) / 2;
                    if (CurrentMenuOption == 1)
                        spriteBatch.DrawString(Font1, JoinGame, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, JoinGame, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
                    RenderPos = Font1.MeasureString(CharScreen) / 2;
                    if (CurrentMenuOption == 2)
                        spriteBatch.DrawString(Font1, CharScreen, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, CharScreen, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 16);
                    RenderPos = Font1.MeasureString(MusicOnOff) / 2;
                    if (CurrentMenuOption == 3)
                        spriteBatch.DrawString(Font1, MusicOnOff, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, MusicOnOff, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 32);
                    RenderPos = Font1.MeasureString(QuitCurrentGame) / 2;
                    if (CurrentMenuOption == 4)
                        spriteBatch.DrawString(Font1, QuitCurrentGame, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, QuitCurrentGame, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 48);
                    RenderPos = Font1.MeasureString(Exit) / 2;
                    if (CurrentMenuOption == 5)
                        spriteBatch.DrawString(Font1, Exit, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Exit, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    #endregion
                    #region NowPlaying
                    string artistPlaying;
                    try
                    {
                        artistPlaying = songs[currentSong].Artist.ToString();
                        RenderPos = Font1.MeasureString(artistPlaying) / 2;
                    }
                    catch (Exception e)
                    {
                        RenderPos = new Vector2(32, 8);
                        artistPlaying = "*Artist Error*";
                        e.ToString();
                    }
                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 80);
                    spriteBatch.DrawString(Font1, artistPlaying, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    string songPlaying;
                    try
                    {
                        songPlaying = songs[currentSong].Name.ToString();
                        RenderPos = Font1.MeasureString(songPlaying) / 2;
                    }
                    catch (Exception e)
                    {
                        RenderPos = new Vector2(32, 8);
                        songPlaying = "*Title Error*";
                        e.ToString();
                    }
                    FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 100);
                    spriteBatch.DrawString(Font1, songPlaying, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    //Vector2 textVector = new Vector2(200, 5);
                    //spriteBatch.DrawString(Font1, songs[currentSong].Artist.ToString() + " - " + songs[currentSong].Name.ToString(), textVector, Color.White);
                    #endregion
                    break;
                #endregion
                // character screen
                #region CurrentScreenMode == 4
                case 4:
                    Title1 = "Character Screen";
                    FontPos = new Vector2(SCREEN_WIDTH / 2, 8);
                    RenderPos = Font1.MeasureString(Title1) / 2;
                    spriteBatch.DrawString(Font1, Title1, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    Level = "Level: ";
                    FontPos = new Vector2(4, 16);
                    RenderPos = new Vector2(0, 0);
                    spriteBatch.DrawString(Font1, Level, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    string Strength = "Strength: ";
                    FontPos = new Vector2(4, 32);
                    RenderPos = new Vector2(0,0);
                    if (CurrentMenuOption == 0)
                        spriteBatch.DrawString(Font1, Strength, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Strength, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    string Dexterity = "Dexterity: ";
                    FontPos = new Vector2(4, 48);
                    RenderPos = new Vector2(0, 0);
                    if (CurrentMenuOption == 1)
                        spriteBatch.DrawString(Font1, Dexterity, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Dexterity, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    string Speed = "Speed: ";
                    FontPos = new Vector2(4, 64);
                    RenderPos = new Vector2(0, 0);
                    if (CurrentMenuOption == 2)
                        spriteBatch.DrawString(Font1, Speed, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Speed, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    string UnassignedStatPoints = "Unassigned Stat Points: ";
                    FontPos = new Vector2(4, 80);
                    RenderPos = new Vector2(0, 0);
                    spriteBatch.DrawString(Font1, UnassignedStatPoints, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    Level = Player.Level.ToString();
                    FontPos = new Vector2(SCREEN_WIDTH - 32, 16);
                    RenderPos = new Vector2(0, 0);
                    spriteBatch.DrawString(Font1, Level, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    Strength = Player.Strength.ToString();
                    FontPos = new Vector2(SCREEN_WIDTH - 32, 32);
                    RenderPos = new Vector2(0, 0);
                    if (CurrentMenuOption == 0)
                        spriteBatch.DrawString(Font1, Strength, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Strength, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    Dexterity = Player.Dexterity.ToString();
                    FontPos = new Vector2(SCREEN_WIDTH - 32, 48);
                    RenderPos = new Vector2(0, 0);
                    if (CurrentMenuOption == 1)
                        spriteBatch.DrawString(Font1, Dexterity, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Dexterity, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    Speed = Player.Speed.ToString(); ;
                    FontPos = new Vector2(SCREEN_WIDTH - 32, 64);
                    RenderPos = new Vector2(0, 0);
                    if (CurrentMenuOption == 2)
                        spriteBatch.DrawString(Font1, Speed, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, Speed, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    UnassignedStatPoints = Player.UnassignedStatPoints.ToString();
                    FontPos = new Vector2(SCREEN_WIDTH - 32, 80);
                    RenderPos = new Vector2(0, 0);
                    spriteBatch.DrawString(Font1, UnassignedStatPoints, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    string WorldCoords = "World Coordinates: (" + Math.Floor(Player.Pos.X + .5f).ToString() + ", " + Math.Floor(Player.Pos.Y + .5f).ToString() + ")";
                    FontPos = new Vector2(4, 128);
                    RenderPos = Vector2.Zero;
                    spriteBatch.DrawString(Font1, WorldCoords, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    string Experience = "Experience = " + Player.XP.ToString();
                    FontPos = new Vector2(4, 144);
                    RenderPos = Vector2.Zero;
                    spriteBatch.DrawString(Font1, Experience, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    if (Network.Session != null)
                    {
                        string SessionInfo = Network.Session.Host.ToString() + "'s game";
                        FontPos = new Vector2(4, 160);
                        RenderPos = Vector2.Zero;
                        spriteBatch.DrawString(Font1, SessionInfo, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    break;
                #endregion
                // character load screen
                #region CurrentScreenMode == 5
                case 5:
                    int counter = 0;
                    foreach (LivingEntity.PlayerEntity o in StorageUnit.PlayersLoaded)
                    {
                        string stats = "HP:" + o.Health + " " +
                            "Str:" + o.Strength + " " +
                            "Dex:" + o.Dexterity + " " +
                            "Spd:" + o.Speed;
                        FontPos = new Vector2(SCREEN_WIDTH / 2, 32 + counter * 16);
                        RenderPos = Font1.MeasureString(stats) / 2;
                        if (CurrentMenuOption == counter)
                            spriteBatch.DrawString(Font1, stats, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Font1, stats, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                        ++counter;
                    }
                    break;
                #endregion
                // quick travel and minimap
                #region CurrentScreenMode == 6
                case 6:
                                        #region Draw Minimap
                    ////int i, j;  //generic iterators
                    //int mapScale = 5;
                    //for (i = 0; i < Map.MapSize - mapScale; i += mapScale)
                    //    for (j = 0; j < Map.MapSize - mapScale; j += mapScale)
                    //    {
                    //        //startX = (float)(.5*(map.mapsize - i / mapScale))
                    //        //startY = (float)(.5*(map.mapsize - j / mapScale))
                    //        RenderPos = new Vector2(70 + (float)i / mapScale, 210 + (float)j / mapScale);
                    //        //RenderPos = new Vector2((float)(.5 * (Map.MapSize - i / mapScale)), (float)(.5 * (Map.MapSize - j / mapScale)));
                    //        //WorldPos.X = i + (int)(Math.Floor(Player.Pos.X + .5f)) - SCREEN_WIDTH / 64;
                    //        //WorldPos.Y = j + (int)(Math.Floor(Player.Pos.Y + .5f)) - SCREEN_HEIGHT / 64;
                    //        //spriteBatch.Draw(MenuBackgroundTexture, RenderPos, Color.White);
                    //        switch (GameMap.MapArray[i, j])
                    //        {
                    //            #region case0-99, terrain
                    //            case 0:
                    //                spriteBatch.Draw(PixelGrassTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(GrassTexture, WorldPos);
                    //                break;
                    //            case 1:
                    //                spriteBatch.Draw(PixelGrassTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(Grass2Texture, WorldPos);
                    //                break;
                    //            case 2:
                    //                spriteBatch.Draw(PixelTreeTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(Tree1Texture, WorldPos);
                    //                break;
                    //            case 3:
                    //                spriteBatch.Draw(PixelDesertTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(DesertTexture, WorldPos);
                    //                break;
                    //            case 5:
                    //                spriteBatch.Draw(PixelWaterTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(WaterTexture, WorldPos);
                    //                break;
                    //            case 6:
                    //                spriteBatch.Draw(PixelDirtTexture, RenderPos, Color.White);
                    //                //RenderRelativeTexture(DirtTexture, WorldPos);
                    //                break;
                    //            #endregion
                    //            //#region case100-103, house
                    //            //case 100:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsHouseTexture[0], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertHouseTexture[0], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 101:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsHouseTexture[1], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertHouseTexture[1], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 102:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsHouseTexture[2], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertHouseTexture[2], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 103:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsHouseTexture[3], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertHouseTexture[3], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //#endregion
                    //            //#region case110-113, blacksmith
                    //            //case 110:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsBlacksmithTexture[0], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertBlacksmithTexture[0], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 111:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsBlacksmithTexture[1], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertBlacksmithTexture[1], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 112:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsBlacksmithTexture[2], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 1), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertBlacksmithTexture[2], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //case 113:
                    //            //    {
                    //            //        if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 0 ||
                    //            //            GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 1) //then we are in forest
                    //            //            RenderRelativeTexture(WoodsBlacksmithTexture[3], WorldPos);
                    //            //        else if (GameMap.MapArray[(int)(WorldPos.X - 2), (int)WorldPos.Y] == 3)
                    //            //            RenderRelativeTexture(DesertBlacksmithTexture[3], WorldPos);
                    //            //        break;
                    //            //    }
                    //            //#endregion
                    //        }
                    //    }
                    Vector2 minimapPos = new Vector2(56.0f, 174.0f);
                    Vector2 minimapJrobertonPos = new Vector2(52.0f, 165.0f);
                    Vector2 minimapBHamHamPos = new Vector2(102.0f, 277.0f);
                    Vector2 minimapDickinsongradPos = new Vector2(160.0f, 216.0f);
                    Vector2 minimapGargoylePos = new Vector2(102.0f, 216.0f);

                    //RenderPos = new Vector2(56.0f, 174.0f);
                    spriteBatch.Draw(MinimapTexture, minimapPos, Color.White);
                    //RenderPos = new Vector2(52.0f, 165.0f);
                    spriteBatch.Draw(MinimapBuildingTexture, minimapBHamHamPos, Color.White);
                    //RenderPos = new Vector2(102.0f, 277.0f);
                    spriteBatch.Draw(MinimapBuildingTexture, minimapJrobertonPos, Color.White);
                    //RenderPos = new Vector2(160.0f, 216.0f);
                    spriteBatch.Draw(MinimapBuildingTexture, minimapDickinsongradPos, Color.White);

                    Vector2 playerMinimapPos = new Vector2((float)(Player.Pos.X / 4.0), (float)(Player.Pos.Y / 4.0));
                    spriteBatch.Draw(PlayerFrontTexture, minimapPos + playerMinimapPos + new Vector2(-16, -16), Color.White);

                    switch (CurrentMenuOption)
                    {
                        case 0: //Bhamham
                            if(visitedBHamHam) spriteBatch.Draw(QuestArrowTexture, minimapBHamHamPos, Color.White);
                            break;
                        case 1: //Jroberton
                            if(visitedJRoberton) spriteBatch.Draw(QuestArrowTexture, minimapJrobertonPos, Color.White);
                            break;
                        case 2: //Dickinsongrad
                            if(visitedDickinsongrad) spriteBatch.Draw(QuestArrowTexture, minimapDickinsongradPos, Color.White);
                            break;
                        case 3: //Gargoyles
                            if (visitedGargoyle) spriteBatch.Draw(QuestArrowTexture, minimapGargoylePos, Color.White);
                            break;
                    }
                    #endregion
                    

                    Title1 = "Map / Quick Travel";
                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 4 - 16;
                    RenderPos = Font1.MeasureString(Title1) / 2;
                    spriteBatch.DrawString(Font1, Title1, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    //Title2 = "of the Apocalypse";
                    //FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 4;
                    //RenderPos = Font1.MeasureString(Title2) / 2;
                    //spriteBatch.DrawString(Font1, Title2, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    string City;
                    if(visitedBHamHam) City = "Birminghamham";
                        else City = "----";

                    string City2; 
                    if(visitedJRoberton) City2 = "JRoberton";
                        else City2 = "----";
                                 
                    string City3;
                    if(visitedDickinsongrad) City3 = "DickinsonGrad";
                        else City3 = "----";

                    string City4;
                    if (visitedGargoyle) City4 = "Gargoyle";
                        else City4 = "----";

                    //Exit = "";

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 - 48;
                    RenderPos = Font1.MeasureString(City) / 2;
                    if (CurrentMenuOption == 0)
                        spriteBatch.DrawString(Font1, City, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, City, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 - 32;
                    RenderPos = Font1.MeasureString(City2) / 2;
                    if (CurrentMenuOption == 1)
                        spriteBatch.DrawString(Font1, City2, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, City2, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2 - 16;
                    RenderPos = Font1.MeasureString(City3) / 2;
                    if (CurrentMenuOption == 2)
                        spriteBatch.DrawString(Font1, City3, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, City3, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    FontPos.X = SCREEN_WIDTH / 2; FontPos.Y = SCREEN_HEIGHT / 2;
                    RenderPos = Font1.MeasureString(City4) / 2;
                    if (CurrentMenuOption == 3)
                        spriteBatch.DrawString(Font1, City4, FontPos, Color.Beige, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Font1, City4, FontPos, Color.DarkKhaki, 0, RenderPos, 1.0f, SpriteEffects.None, 0.5f);

                    break;
                #endregion

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void EnemyAI(GameTime gameTime)
        {
            for (int i = 0; i < MAX_ENEMIES; i++)
            {
                float Distance, enemyDetectDistance = 4;

                //find closest player
                LivingEntity ClosestPlayer = Player;
#if (!WINDOWS)
                foreach (NetworkGamer gamer in Network.Session.AllGamers)
                {
                    LivingEntity player = gamer.Tag as LivingEntity;
                    if (player != null)
                    {
                        Distance = Fast2d(enemy[i].Pos, player.Pos);
                        if(Network.Session.IsHost)
                            if (Distance < 1.0f)
                            {
                                Int16 PlayerDamage = enemy[i].Attack(player);
                                if (PlayerDamage > 0)
                                {
                                    try
                                    {
                                        soundsPlaying[2] = sounds[2].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                Network.EnemyAttack(gamer.Gamertag.GetHashCode(),
                                    PlayerDamage);
                            }
                        if (Distance < Fast2d(enemy[i].Pos, ClosestPlayer.Pos))
                            ClosestPlayer = player;
                    }
                }
#endif
                Distance = Fast2d(enemy[i].Pos, ClosestPlayer.Pos);
#if(WINDOWS)
                if (Distance < 1.0f)
                    Player.CurrentHealth -= enemy[i].Attack(Player);
#endif
                if (Distance < enemyDetectDistance &&
                    Distance > 1.0f)
                {
                    if (enemy[i].Pos.X < ClosestPlayer.Pos.X)
                        enemy[i].Velocity.X += .01f;
                    if (enemy[i].Pos.Y < ClosestPlayer.Pos.Y)
                        enemy[i].Velocity.Y += .01f;
                    if (enemy[i].Pos.X > ClosestPlayer.Pos.X)
                        enemy[i].Velocity.X -= .01f;
                    if (enemy[i].Pos.Y > ClosestPlayer.Pos.Y)
                        enemy[i].Velocity.Y -= .01f;

                    if (enemy[i].Velocity.X > 0)
                    {
                        if (enemy[i].Velocity.Y > 0.001)
                        {
                            if (enemy[i].Velocity.X > enemy[i].Velocity.Y)
                            { enemy[i].CurrentAnimation = 3; }
                            else { enemy[i].CurrentAnimation = 0; }
                        }
                        else
                        {
                            if (enemy[i].Velocity.X > -enemy[i].Velocity.Y)
                            { enemy[i].CurrentAnimation = 3; }
                            else { enemy[i].CurrentAnimation = 1; }
                        }
                    }
                    else if (enemy[i].Velocity.X < 0.001)
                    {
                        if (enemy[i].Velocity.Y > 0.001)
                        {
                            if (-enemy[i].Velocity.X > enemy[i].Velocity.Y)
                            { enemy[i].CurrentAnimation = 2; }
                            else { enemy[i].CurrentAnimation = 0; }
                        }
                        else
                        {
                            if (enemy[i].Velocity.X > enemy[i].Velocity.Y)
                            { enemy[i].CurrentAnimation = 1; }
                            else { enemy[i].CurrentAnimation = 2; }
                        }
                    }
                    enemy[i].Update();
                    if (IsCollision(enemy[i]) != 0)
                    {
                        enemy[i].Pos -= enemy[i].Velocity;
                        enemy[i].Velocity = Vector2.Zero;
                    }


                }
                if (Distance > MAX_ENEMY_DISTANCE_FROM_PLAYER &&
                    enemy[i].Flag == 0)
                    SpawnEnemy(i);
            }
        }
        public float Fast2d(Vector2 ThingOne, Vector2 ThingTwo)
        {
            //fast 2d calculations, 3.5% error. returns distance from
            float x = Math.Abs(ThingTwo.X - ThingOne.X);
            float z = Math.Abs(ThingTwo.Y - ThingOne.Y);
            int mn = (int)((x < z) ? x : z);
            return (x + z - (mn >> 1) - (mn >> 2) + (mn >> 4));
        }
        public Game1()
        {
#if (ZUNE)
            Components.Add(new GamerServicesComponent(this));
#endif
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        private Rectangle GetNewTileRect(Vector2 Pos)
        {
            Rectangle NewRect = new Rectangle((int)Pos.X - LivingEntity.LivingEntityRadius,
                (int)Pos.Y - LivingEntity.LivingEntityRadius,
                LivingEntity.LivingEntityRadiusT2,
                LivingEntity.LivingEntityRadiusT2);
            return NewRect;
        }
        protected override void Initialize()
        {
            base.Initialize();
            MusicOn = false;
            RandomQuest = new RandomQuest(this);
            RandomQuest.RandomQuestFlag = -1;
            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;
            GameMap = new Map();
            Player = new LivingEntity.PlayerEntity();
            Player.Speed = Player.Strength = Player.Dexterity = Player.Health = 10;
            Player.Pos = LocationBHamHam;
            StorageUnit = new Storage();

            FontPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            Network = new Network(this);
            rand = new Random(DateTime.UtcNow.Millisecond);

            enemy = new LivingEntity.EnemyEntity[MAX_ENEMIES];
            for (int i = 0; i < MAX_ENEMIES; ++i)
                enemy[i] = new LivingEntity.EnemyEntity(1);
            for (int i = 0; i < MAX_ENEMIES; ++i)
                SpawnEnemy(i);

            treasureGotten = 0;

            //MAX_PUZZLE_GARGOYLES
            gargoyleActivated = new bool[MAX_PUZZLE_GARGOYLES];
            gargoyleActivationOrder = new int[MAX_PUZZLE_GARGOYLES];
            for (int i = 0; i < MAX_PUZZLE_GARGOYLES; ++i)
            {
                gargoyleActivated[i] = false;
                gargoyleActivationOrder[i] = 0;
            }
            numGargoylesActivated = 0;
            //gargoyleActivated[];

            NPC = new LivingEntity.NPCEntity[MAX_NPCS];
            for (int i = 0; i < MAX_NPCS; ++i)
            {
                switch (i)
                {
                    case 0://Shaniqua
                        NPC[i] = new LivingEntity.NPCEntity(PenguinTexture);
                        NPC[i].Pos = new Vector2(256f, 500f);
                        break;
                    case 1://Amulet
                        NPC[i] = new LivingEntity.NPCEntity(AmuletTexture);
                        NPC[i].Pos = new Vector2(236f, 500f);
                        break;
                    case 2://Sword
                        NPC[i] = new LivingEntity.NPCEntity(SwordTexture);
                        NPC[i].Pos = new Vector2(32, 32);
                        break;
                    case 3://Blacksmith in JRoberton
                        NPC[i] = new LivingEntity.NPCEntity(PlayerFrontTexture);
                        NPC[i].Pos = new Vector2(35, 31);
                        break;
                    case 4://Gargoyle to East of JRoberton
                        NPC[i] = new LivingEntity.NPCEntity(GargoyleTexture);
                        NPC[i].Pos = new Vector2(275, 498);
                        break;
                    case 5://Gargoyle 1 in group in center
                        NPC[i] = new LivingEntity.NPCEntity(GargoyleTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(-4, 0);
                        break;
                    case 6://Gargoyle 2 in group in center
                        NPC[i] = new LivingEntity.NPCEntity(GargoyleTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(1, 0);
                        break;
                    case 7://Gargoyle 3 in group in center
                        NPC[i] = new LivingEntity.NPCEntity(GargoyleTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(-4, 5);
                        break;
                    case 8://Gargoyle 4 in group in center
                        NPC[i] = new LivingEntity.NPCEntity(GargoyleTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(1, 5);
                        break;
                    case 9://Battle Axe gotten for beating Gargoyle puzzle
                        NPC[i] = new LivingEntity.NPCEntity(BattleAxeTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(0, 3);
                        NPC[i].alive = false;
                        break;
                    case 20://Birminghamham Town Sage
                        NPC[i] = new LivingEntity.NPCEntity(SageTexture);
                        NPC[i].Pos = LocationBHamHam;
                        NPC[i].Pos.Y -= 7.0f;
                        break;
                    case 21://Dickinsongrad Town Sage
                        NPC[i] = new LivingEntity.NPCEntity(SageTexture);
                        NPC[i].Pos = LocationDickinsongrad + new Vector2(5, 0);
                        break;
                    case 22://JRoberton Town Sage
                        NPC[i] = new LivingEntity.NPCEntity(SageTexture);
                        NPC[i].Pos = LocationJRoberton;// +new Vector2(2, 0); ;
                        break;
                    case 25://Santa 
                        NPC[i] = new LivingEntity.NPCEntity(SantaTexture);
                        NPC[i].Pos = LocationDickinsongrad + new Vector2(1, -2);
                        break;
                    case 26://Catapult
                        NPC[i] = new LivingEntity.NPCEntity(CatapultTexture);
                        NPC[i].Pos = LocationGargoyle + new Vector2(2, -8);
                        NPC[i].alive = false;
                        break;
                    case 27://Jester
                        NPC[i] = new LivingEntity.NPCEntity(JesterTexture);
                        //NPC[i].Pos = LocationBHamHam + new Vector2(-5, 4);
                        NPC[i].Pos = LocationDickinsongrad + new Vector2(0, 4);
                        NPC[i].alive = false;
                        break;
                    default:
                        
                        if (i >= 10 && i <= 19)
                        {
                            NPC[i] = new LivingEntity.NPCEntity(TreasureChestTexture);
                            //NPC[i].Pos = new Vector2((float)(Map.MapSize / 2 + rand.Next() % 10), (float)(Map.MapSize - Map.MapSize / 32 + rand.Next() % 10));
                            NPC[i].Pos = new Vector2((float)(rand.Next() % 512), (float)(rand.Next() % 512));
                            //NPC[i].Pos = new Vector2((float)(LocationBHamHam.X + rand.Next() % 10 - 5), (float)(LocationBHamHam.Y + rand.Next() % 10 - 5));
                            //Map.MapSize / 2, Map.MapSize - Map.MapSize / 32
                        }
                        else
                        {
                            NPC[i] = new LivingEntity.NPCEntity(PenguinTexture);
                            NPC[i].Pos = new Vector2(275, 498);
                        }

                    break;
                }

                // 0 Shaniqua
                // 1 Amulet
                // 2 Sword
                // 3 Blacksmith in Jroberton
                // 4 Gargoyle to East of Jroberton
            }

            storyMessage = new string[MAX_MESSAGES];
            for (int i = 0; i < MAX_MESSAGES; i++)
            {
                switch (i)
                {
                    case 0: storyMessage[i] = "Find Shaniqua the penguin.\nShe is somewhere around here.";
                        break;
                    case 1: storyMessage[i] = "You must head west and\n retrieve the Amulet of Ages.";
                        break;
                    case 2: storyMessage[i] = "Head northwest and take\n the path to Jroberton.";
                        break;
                    case 3: storyMessage[i] = "Talk to the blacksmith,\nwho will give guidance.";
                        break;
                    case 4: storyMessage[i] = "Find the lone gargoyle to\nthe east of Birminghamham.";
                        break;
                    case 5: storyMessage[i] = "Take the northern road to\nthe four gargoyles.";
                        break;
                    case 6: storyMessage[i] = "Sir Mark is Dickinsongrad,\nto the East. Kill him.";
                        break;
                    case 7: storyMessage[i] = "You have defeated Sir Mark.\nA jester laughs to the south.";
                        break;
                    case 8: storyMessage[i] = "You must find the magical\n axe to defeat Sir Mark.";
                        break;
                    case 9: storyMessage[i] = "Above the gargoyles lies\nthe machine of the apocalypse.";
                        break;
                    case 10: storyMessage[i] = "You swing the axe,destroy-\ning the machine. You win.";
                        break;
                    case 11: storyMessage[i] = "You pick up the key to the castle of Dickinsongrad in the city of the same name.  Shaniqua tells you to go to the evil lair of Sir Mark in the north.";
                        break;
                    case 12: storyMessage[i] = "You must now defeat Sir Mark.  Short in stature, Sir Mark is nevertheless a force to be reckoned with.";
                        break;
                    case 13: storyMessage[i] = "Swinging your sword one last time, you hew off the head of the one who killed your best friend.  As it bounces to the earth, you realize that your quest is at an end.";
                        break;
                    case 100: storyMessage[i] = "Treasure: " + treasureGotten + "/10";
                        break;                   
                    default: storyMessage[i] = "DEFAULT MESSAGE";
                        break;

                    
                }
            }
            displayMessage(0, 90);
            //currentMessage = 0;
            //messagetime = 120;

            /*
            You remember little of your past. The hoops of fire, tigers, and freaks of a past life accentuate
                the grit and grime of futuristic Birminghamham life. The one you loved… lost forever. Since the
            day your heart was ripped to pieces, you swore to die to his old self and bring new life. Wallowing
            in the blacksmithery of Jew in the town of Jroberton outside Birminghamham, you realize the goal
            of your life must be to avenge the death of your love. You still have nightmares, perhaps telling
             * the future. You still have nightmares of the past - the slaying of your one hope of a happy life, 
             * your pearl of great price, your penguin, Shaniqua. On occasion she whispers to you that she will help.
             Tonight is a night of nightmares. Every night is a night of nightmares. Every night is a night of future nightmares 
             * of night. Tonight your Shaniqua whispers demands for revenge; you feel propelled to escape and exact 
             * revenge on the evil one, Sir Mark in Dickinsongrad, also outside of Birminghamham. Your ragtag crew of 
             * you and your old dead penguin ghost show no fear, ready to eventually bring down the ever-rotating 
             * machine of the apocalypse, pain harbinger sense-offender extraordinaire, Sir Mark.
            Your sword shop offers little help to begin. What could you use to fight the enemy? Use your circus-born 
             * ingenuity to take hold of the situation and fight it with a light, quick, sharp, and pointy attack. 
             * May your blade of justice shine the way, and may the voice of Shaniqua be your guide.
            */
            /*
                The voice of Shaniqua calls out to you, 'Return to the blacksmithery in Jroberton, where you will find the Amulet of the Ages.'
                Entering the town of JRoberton, you are overcome with grief, hunger, and the urge to avenge the shameless slaughter of the beautiful pengiun named Shaniqua.
                You pick up the amulet and Shaniqua speaks to you again:  “Go to the four ends of the earth and retrieve the shards of the sword Awesome, and then return here to forge anew the sword that was shattered.”
                You have retrieved the first fragment of the sword.
                You have retrieved the second fragment of the sword.
                You have retrieved the third fragment of the sword.
                You have retrieved the last fragment of the sword and do a little dance to celebrate.  You also sing a silly song, but we didn’t want to overload the Zune with such mindless foolishness.  Return to the blacksmithery with the pieces of the sword.
                Throwing the shards on the Anvil of Justice, you reforge the blade and name it DyKnow the Destroyer.  Shaniqua tells you to head to the eastern hills to recover the lost key into Birminghamham. 
                Hewing your way through the last of the enemies that guard the lost key, the ghost of Shaniqua guides you to the key.  Picking it up, you somehow know that traveling to Birminghamham will shed more light on where to find Sir Mark, the evil one of old who stole your life’s shining joy from you.
                You enter the town of Birminghamham.  Steeped in tradition, the old city is a monolithic tribute to the times of old.
                You pick up the key to the castle of Dickinsongrad in the city of the same name.  Shaniqua tells you to go to the evil lair of Sir Mark in the north.
                You must now defeat Sir Mark.
                Swinging your sword one last time, you hew off the head of the one who killed your best friend.  As it bounces to the earth, you realize that your quest is at an end.
            */

            library = new MediaLibrary();
            songs = library.Songs;
            MediaPlayer.State.Equals(MediaState.Stopped);
            TimeLastMenuChange = TimeLastScreenModeChange = Environment.TickCount;
            CurrentScreenMode = CurrentMenuOption = lastButtonPressed = 0;
        }
        private Int16 IsCollision(LivingEntity Entity)
        {
            if (Entity.Pos.X < 7) Entity.Pos.X = 7;
            if (Entity.Pos.X > Map.MapSize - 6) Entity.Pos.X = Map.MapSize - 7;
            if (Entity.Pos.Y < 7) Entity.Pos.Y = 7;
            if (Entity.Pos.Y > Map.MapSize - 7) Entity.Pos.Y = Map.MapSize - 7;

            const float COLLISION_DISTANCE = 0.6f;
            Int16 MapInt;
            try
            {
                MapInt = GameMap.MapArray[(int)Math.Floor(Entity.Pos.X + 0.5f), (int)Math.Floor(Entity.Pos.Y + 0.5f)];
            }
            catch (Exception e)
            {
                //array out of bounds probably
                MapInt = 2;
                e.ToString();
            }
            if ((MapInt == 2) ||    //any other impass
                (MapInt == 5) ||
                (MapInt == 4) ||
                MapInt > 99)
                return MapInt;

            /*foreach (NetworkGamer gamer in Network.Session.RemoteGamers)
            {
                LivingEntity player = gamer.Tag as LivingEntity;
                if (player != null)
                    if (Fast2d(Entity.Pos, Player.Pos) < COLLISION_DISTANCE)
                        return true;
            }*/

            for (int i = 0; i < MAX_ENEMIES; ++i)
            {
                float Distance = Fast2d(enemy[i].Pos, Entity.Pos);
                if (Distance < COLLISION_DISTANCE && Distance != 0.0f)
                    return 1;
            }
            return 0;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DesertTexture = Content.Load<Texture2D>("Artwork\\desert");
            DirtTexture = Content.Load<Texture2D>("Artwork\\dirt");
            GrassTexture = Content.Load<Texture2D>("Artwork\\grass1");
            Grass2Texture = Content.Load<Texture2D>("Artwork\\grass2");
            Tree1Texture = Content.Load<Texture2D>("Artwork\\tree1");
            EnemyFrontTexture = Content.Load<Texture2D>("Artwork\\enemy");
            WaterTexture = Content.Load<Texture2D>("Artwork\\water");
            QuestArrowTexture = Content.Load<Texture2D>("Artwork\\QuestArrow");
            PlayerFrontTexture = Content.Load<Texture2D>("Artwork\\playerfront");
            HealthBarTexture = Content.Load<Texture2D>("Artwork\\health bar");
            HealthBarOutlineTexture = Content.Load<Texture2D>("Artwork\\health bar outline");
            TextureAttackLight1of5 = Content.Load<Texture2D>("Artwork\\AttackLight-1of5");
            TextureAttackLight2of5 = Content.Load<Texture2D>("Artwork\\AttackLight-2of5");
            TextureAttackLight3of5 = Content.Load<Texture2D>("Artwork\\AttackLight-3of5");
            TextureAttackLight4of5 = Content.Load<Texture2D>("Artwork\\AttackLight-4of5");
            TextureAttackLight5of5 = Content.Load<Texture2D>("Artwork\\AttackLight-5of5");
            #region Load textures of Frog Enemy
            TexFrogEast1 = Content.Load<Texture2D>("Artwork\\Frog_run_east004");
            TexFrogEast2 = Content.Load<Texture2D>("Artwork\\Frog_run_east003");
            TexFrogEast3 = Content.Load<Texture2D>("Artwork\\Frog_run_east002");
            TexFrogEast4 = Content.Load<Texture2D>("Artwork\\Frog_run_east001");
            TexFrogNorth1 = Content.Load<Texture2D>("Artwork\\Frog_run_north004");
            TexFrogNorth2 = Content.Load<Texture2D>("Artwork\\Frog_run_north003");
            TexFrogNorth3 = Content.Load<Texture2D>("Artwork\\Frog_run_north002");
            TexFrogNorth4 = Content.Load<Texture2D>("Artwork\\Frog_run_north001");
            TexFrogWest1 = Content.Load<Texture2D>("Artwork\\Frog_run_west004");
            TexFrogWest2 = Content.Load<Texture2D>("Artwork\\Frog_run_west003");
            TexFrogWest3 = Content.Load<Texture2D>("Artwork\\Frog_run_west002");
            TexFrogWest4 = Content.Load<Texture2D>("Artwork\\Frog_run_west001");
            TexFrogSouth1 = Content.Load<Texture2D>("Artwork\\Frog_run_south004");
            TexFrogSouth2 = Content.Load<Texture2D>("Artwork\\Frog_run_south003");
            TexFrogSouth3 = Content.Load<Texture2D>("Artwork\\Frog_run_south002");
            TexFrogSouth4 = Content.Load<Texture2D>("Artwork\\Frog_run_south001");
            #endregion
            #region Load textures of WaterThug Enemy
            TexWaterThugEast1 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_east003");
            TexWaterThugEast2 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_east002");
            TexWaterThugEast3 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_east001");
            TexWaterThugNorth1 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_north003");
            TexWaterThugNorth2 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_north002");
            TexWaterThugNorth3 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_north001");
            TexWaterThugWest1 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_west003");
            TexWaterThugWest2 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_west002");
            TexWaterThugWest3 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_west001");
            TexWaterThugSouth1 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_south003");
            TexWaterThugSouth2 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_south002");
            TexWaterThugSouth3 = Content.Load<Texture2D>("Artwork\\WaterThug_walk_south001");
            #endregion
            #region Load textures of Zombie Enemy
            TexZombieEast1 = Content.Load<Texture2D>("Artwork\\Zombie_walk_east001");
            TexZombieEast2 = Content.Load<Texture2D>("Artwork\\Zombie_walk_east002");
            TexZombieNorth1 = Content.Load<Texture2D>("Artwork\\Zombie_walk_north003");
            TexZombieNorth2 = Content.Load<Texture2D>("Artwork\\Zombie_walk_north002");
            TexZombieNorth3 = Content.Load<Texture2D>("Artwork\\Zombie_walk_north001");
            TexZombieWest1 = Content.Load<Texture2D>("Artwork\\Zombie_walk_west001");
            TexZombieWest2 = Content.Load<Texture2D>("Artwork\\Zombie_walk_west002");
            TexZombieSouth1 = Content.Load<Texture2D>("Artwork\\Zombie_walk_south003");
            TexZombieSouth2 = Content.Load<Texture2D>("Artwork\\Zombie_walk_south002");
            TexZombieSouth3 = Content.Load<Texture2D>("Artwork\\Zombie_walk_south001");
            #endregion
            #region Load textures of Needlion Enemy
            TexNeedlionEast1 = Content.Load<Texture2D>("Artwork\\Needlion_walk_east003");
            TexNeedlionEast2 = Content.Load<Texture2D>("Artwork\\Needlion_walk_east002");
            TexNeedlionEast3 = Content.Load<Texture2D>("Artwork\\Needlion_walk_east001");
            TexNeedlionNorth1 = Content.Load<Texture2D>("Artwork\\Needlion_walk_north003");
            TexNeedlionNorth2 = Content.Load<Texture2D>("Artwork\\Needlion_walk_north002");
            TexNeedlionNorth3 = Content.Load<Texture2D>("Artwork\\Needlion_walk_north001");
            TexNeedlionWest1 = Content.Load<Texture2D>("Artwork\\Needlion_walk_west003");
            TexNeedlionWest2 = Content.Load<Texture2D>("Artwork\\Needlion_walk_west002");
            TexNeedlionWest3 = Content.Load<Texture2D>("Artwork\\Needlion_walk_west001");
            TexNeedlionSouth1 = Content.Load<Texture2D>("Artwork\\Needlion_walk_south003");
            TexNeedlionSouth2 = Content.Load<Texture2D>("Artwork\\Needlion_walk_south002");
            TexNeedlionSouth3 = Content.Load<Texture2D>("Artwork\\Needlion_walk_south001");
            #endregion
            #region Load textures of DarkStalker Enemy
            TexDarkStalkerEast1 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_east001");
            TexDarkStalkerEast2 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_east002");
            TexDarkStalkerEast3 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_east003");
            TexDarkStalkerNorth1 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_north001");
            TexDarkStalkerNorth2 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_north002");
            TexDarkStalkerNorth3 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_north003");
            TexDarkStalkerWest1 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_west001");
            TexDarkStalkerWest2 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_west002");
            TexDarkStalkerWest3 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_west003");
            TexDarkStalkerSouth1 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_south001");
            TexDarkStalkerSouth2 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_south002");
            TexDarkStalkerSouth3 = Content.Load<Texture2D>("Artwork\\DarkStalker_walk_south003");
            #endregion
            #region Load textures of Sabin
            TexSabinNorth1of4 = Content.Load<Texture2D>("Artwork\\SabinWalkNorth1of4");
            TexSabinNorth2of4 = Content.Load<Texture2D>("Artwork\\SabinWalkNorth2of4");
            TexSabinNorth3of4 = Content.Load<Texture2D>("Artwork\\SabinWalkNorth3of4");
            TexSabinNorth4of4 = Content.Load<Texture2D>("Artwork\\SabinWalkNorth4of4");
            TexSabinSouth1of4 = Content.Load<Texture2D>("Artwork\\SabinWalkSouth1of4");
            TexSabinSouth2of4 = Content.Load<Texture2D>("Artwork\\SabinWalkSouth2of4");
            TexSabinSouth3of4 = Content.Load<Texture2D>("Artwork\\SabinWalkSouth3of4");
            TexSabinSouth4of4 = Content.Load<Texture2D>("Artwork\\SabinWalkSouth4of4");
            TexSabinEast1of4 = Content.Load<Texture2D>("Artwork\\SabinWalkEast1of4");
            TexSabinEast2of4 = Content.Load<Texture2D>("Artwork\\SabinWalkEast2of4");
            TexSabinEast3of4 = Content.Load<Texture2D>("Artwork\\SabinWalkEast3of4");
            TexSabinEast4of4 = Content.Load<Texture2D>("Artwork\\SabinWalkEast4of4");
            TexSabinWest1of4 = Content.Load<Texture2D>("Artwork\\SabinWalkWest1of4");
            TexSabinWest2of4 = Content.Load<Texture2D>("Artwork\\SabinWalkWest2of4");
            TexSabinWest3of4 = Content.Load<Texture2D>("Artwork\\SabinWalkWest3of4");
            TexSabinWest4of4 = Content.Load<Texture2D>("Artwork\\SabinWalkWest4of4");
            #endregion
            #region Load textures of Link
            TexLinkSouth1of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front001");
            TexLinkSouth2of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front002");
            TexLinkSouth3of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front003");
            TexLinkSouth4of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front004");
            TexLinkSouth5of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front005");
            TexLinkSouth6of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front006");
            TexLinkSouth7of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front007");
            TexLinkSouth8of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_front008");

            TexLinkNorth1of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back001");
            TexLinkNorth2of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back002");
            TexLinkNorth3of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back003");
            TexLinkNorth4of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back004");
            TexLinkNorth5of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back005");
            TexLinkNorth6of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back006");
            TexLinkNorth7of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back007");
            TexLinkNorth8of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_back008");

            TexLinkEast1of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side001");
            TexLinkEast2of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side002");
            TexLinkEast3of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side003");
            TexLinkEast4of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side004");
            TexLinkEast5of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side005");
            TexLinkEast6of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side006");
            TexLinkEast7of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side007");
            TexLinkEast8of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_side008");

            TexLinkWest1of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM001");
            TexLinkWest2of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM002");
            TexLinkWest3of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM003");
            TexLinkWest4of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM004");
            TexLinkWest5of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM005");
            TexLinkWest6of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM006");
            TexLinkWest7of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM007");
            TexLinkWest8of8 = Content.Load<Texture2D>("Artwork\\LinkGN_walk_sideM008");

            #endregion
            PenguinTexture = Content.Load<Texture2D>("Artwork\\penguin");
            SageTexture = Content.Load<Texture2D>("Artwork\\sage");
            MenuBackgroundTexture = Content.Load<Texture2D>("Artwork\\wallpaper_menu");
            InteractTexture = Content.Load<Texture2D>("Artwork\\interact");
            AmuletTexture = Content.Load<Texture2D>("Artwork\\amulet");
            SwordTexture = Content.Load<Texture2D>("Artwork\\sword");
            GargoyleTexture = Content.Load<Texture2D>("Artwork\\gargoyle");
            GargoyleEyesTexture = Content.Load<Texture2D>("Artwork\\gargoyleEYES");
            TreasureChestTexture = Content.Load<Texture2D>("Artwork\\treasureChest");
            BattleAxeTexture = Content.Load<Texture2D>("Artwork\\BattleAxe");
            SantaTexture = Content.Load<Texture2D>("Artwork\\Santa");
            CatapultTexture = Content.Load<Texture2D>("Artwork\\catapult");
            JesterTexture = Content.Load<Texture2D>("Artwork\\jester");

            PixelGrassTexture = Content.Load<Texture2D>("Artwork\\pixel_grass");
            PixelWaterTexture = Content.Load<Texture2D>("Artwork\\pixel_water");
            PixelDesertTexture = Content.Load<Texture2D>("Artwork\\pixel_desert");
            PixelDirtTexture = Content.Load<Texture2D>("Artwork\\pixel_dirt");
            PixelTreeTexture = Content.Load<Texture2D>("Artwork\\pixel_tree");
            MinimapTexture = Content.Load<Texture2D>("Artwork\\minimap");
            MinimapBuildingTexture = Content.Load<Texture2D>("Artwork\\minimap_building");
            CircleGreenTexture = Content.Load<Texture2D>("Artwork\\minimap");

            #region Load Buildings
            WoodsBlacksmithTexture = new Texture2D[4];
            for (int i = 1; i < 5; i++)
                WoodsBlacksmithTexture[i - 1] = Content.Load<Texture2D>("Artwork\\Buildings\\woodsblacksmith" + i.ToString());
            WoodsHouseTexture = new Texture2D[4];
            for(int i=1; i<5; i++)
                WoodsHouseTexture[i-1] = Content.Load<Texture2D>("Artwork\\Buildings\\woodshouse"+i.ToString());
            DesertBlacksmithTexture = new Texture2D[4];
            for (int i = 1; i < 5; i++)
                DesertBlacksmithTexture[i - 1] = Content.Load<Texture2D>("Artwork\\Buildings\\desertblacksmith" + i.ToString());
            DesertHouseTexture = new Texture2D[4];
            for (int i = 1; i < 5; i++)
                DesertHouseTexture[i - 1] = Content.Load<Texture2D>("Artwork\\Buildings\\deserthouse" + i.ToString());
            #endregion
            Font1 = Content.Load<SpriteFont>("SpriteFont1");

            sounds = new SoundEffect[MAX_SOUNDS];
            soundsPlaying = new SoundEffectInstance[MAX_SOUNDS];
            for (int i = 0; i < MAX_SOUNDS; i++)
                switch (i)
                {
                    case 0: sounds[i] = Content.Load<SoundEffect>("Sounds\\Sword Hit");// 0" + ((i % 5) + 1).ToString());
                        break;
                    case 1: sounds[i] = Content.Load<SoundEffect>("Sounds\\Sword Miss");//" + ((i % 2) + 1).ToString());
                        break;
                    case 2: sounds[i] = Content.Load<SoundEffect>("Sounds\\Scream");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 3: sounds[i] = Content.Load<SoundEffect>("Sounds\\Player Die");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 4: sounds[i] = Content.Load<SoundEffect>("Sounds\\gargoyle");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 5: sounds[i] = Content.Load<SoundEffect>("Sounds\\puzzlefail");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 6: sounds[i] = Content.Load<SoundEffect>("Sounds\\coins");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 7: sounds[i] = Content.Load<SoundEffect>("Sounds\\menu_change");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 8: sounds[i] = Content.Load<SoundEffect>("Sounds\\growl");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 9: sounds[i] = Content.Load<SoundEffect>("Sounds\\windows98");//0" + ((i % 4) + 1).ToString());
                        break;
                    case 10: sounds[i] = Content.Load<SoundEffect>("Sounds\\WaterDrip");//0" + ((i % 4) + 1).ToString());
                        break;
                }
        }
        protected void MusicUpdate(GameTime gameTime)
        {
            //if (currentSong >= songs.Count) currentSong = 0;

            if (MediaPlayer.State != MediaState.Playing)
            {
                //while (!(songs[currentSong].Artist.ToString() == "THEME"))
                //{
                    currentSong++;
                    if (currentSong >= songs.Count) currentSong = 0;
                //}
                //songName = songs[currentSong].Name.ToString();
                MediaPlayer.Play(songs[currentSong]);
                MediaPlayer.Volume = 1.0f;
            }

            //if (MediaPlayer.State == MediaState.Stopped)            //{
            //   MediaPlayer.Play(songs[currentSong++]);
            //}
        }
        private Vector2 RenderPosCalc(Vector2 vec)
        {
            return new Vector2(SCREEN_WIDTH / 2 - (LivingEntity.LivingEntityRadiusT2 * -(vec.X - Player.Pos.X)) + LivingEntity.LivingEntityRadius / 2,
                SCREEN_HEIGHT / 2 - (LivingEntity.LivingEntityRadiusT2 * -(vec.Y - Player.Pos.Y)) + LivingEntity.LivingEntityRadius);
        }
        private void RenderRelativeTexture(Texture2D Texture, Vector2 WorldPosition, Rectangle rect)
        {
            //Description: Renders a texture from world coordinates relative to player (so player is always in middle)
            //the magic interpolator
            Vector2 RenderPos = RenderPosCalc(WorldPosition);

            Rectangle RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                LivingEntity.LivingEntityRadiusT2,
                LivingEntity.LivingEntityRadiusT2);

            spriteBatch.Draw(Texture, RenderRect, rect, Color.White);
        }
        private void RenderRelativeTexture(Texture2D Texture, Vector2 WorldPosition)
        {
            //Description: Renders a texture from world coordinates relative to player (so player is always in middle)
            //the magic interpolator
            Vector2 RenderPos = RenderPosCalc(WorldPosition);

            Rectangle RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                LivingEntity.LivingEntityRadiusT2,
                LivingEntity.LivingEntityRadiusT2);

            spriteBatch.Draw(Texture, RenderRect, Color.White);
        }
        private void RenderEntityTexture(LivingEntity pLE)
        {
            //Description: Renders a texture from world coordinates relative to player (so player is always in middle)
            //the magic interpolator
            Vector2 RenderPos = RenderPosCalc(pLE.Pos);
    
            Rectangle RenderRect = new Rectangle((int)RenderPos.X - LivingEntity.LivingEntityRadius,
                (int)RenderPos.Y - LivingEntity.LivingEntityRadius,
                LivingEntity.LivingEntityRadiusT2,
                LivingEntity.LivingEntityRadiusT2);

            if (pLE.Velocity.X > .005f || pLE.Velocity.Y > .005f ||
                pLE.Velocity.X < -.005f || pLE.Velocity.Y < -.005f ||
                pLE.isAttacking)
                spriteBatch.Draw(pLE.texAnimation[pLE.CurrentAnimation][pLE.CurrentFrame], RenderRect, Color.White);
            else
                spriteBatch.Draw(pLE.texAnimation[pLE.CurrentAnimation][0], RenderRect, Color.White);
        }
        private void RenderRelativeAttackTexture(Texture2D Texture, Vector2 WorldPosition)
        {
            //Description: Renders a texture from world coordinates relative to player (so player is always in middle)
            //the magic interpolator
            Vector2 RenderPos = RenderPosCalc(WorldPosition);
    
            Rectangle RenderRect = new Rectangle((int)RenderPos.X - Player.Abilities[Player.CurrentAnimationAttack].widthAbility/2,
                                                  (int)RenderPos.Y - Player.Abilities[Player.CurrentAnimationAttack].heightAbility/2,
                                                  Player.Abilities[Player.CurrentAnimationAttack].widthAbility, 
                                                  Player.Abilities[Player.CurrentAnimationAttack].heightAbility);
            spriteBatch.Draw(Texture, RenderRect, Color.White);
        }
        public void SpawnEnemy(Int32 index)
        {
            int Random = rand.Next() % 4;
            if (Fast2d(Player.Pos, LocationBHamHam) < Map.MapSize / 8)
            {
                if (Random==0)
                    enemy[index] = new LivingEntity.EnemyFrog(2);
                else
                    enemy[index] = new LivingEntity.GreenEnemy(1);
            }
            else if (Fast2d(Player.Pos, LocationJRoberton) < Map.MapSize / 8)
            {
                if (Random==0)
                    enemy[index] = new LivingEntity.EnemyNeedlion(3);
                else
                    enemy[index] = new LivingEntity.EnemyFrog(2);
            }
            else if (Fast2d(Player.Pos, LocationDickinsongrad) < Map.MapSize / 8)
            {
                if (Random == 0)
                    enemy[index] = new LivingEntity.EnemyDarkStalker(4);
                else
                    enemy[index] = new LivingEntity.EnemyFrog(3);
            }
            else
            {
                if (Random == 0)
                    enemy[index] = new LivingEntity.EnemyZombie((Int16)(Player.Level / 3 + (rand.Next() % 3)));
                else
                    enemy[index] = new LivingEntity.EnemyDarkStalker(4);
            }

            float Distance;
            enemy[index].Pos = new Vector2(Player.Pos.X + (rand.Next() % 32) - 16,
                    Player.Pos.Y + (rand.Next() % 32) - 16);
            do
            {
                enemy[index].Pos = new Vector2(Player.Pos.X + (rand.Next() % 32) - 16,
                    Player.Pos.Y + (rand.Next() % 32) - 16);
                Distance = Fast2d(enemy[index].Pos, Player.Pos);
                if(IsCollision(enemy[index])==5)
                    enemy[index] = new LivingEntity.EnemyWaterThug(10);
#if (ZUNE)
            } while (Distance < 9.0f 
#else
            } while (Distance < 18.0f 
#endif
                || Distance > MAX_ENEMY_DISTANCE_FROM_PLAYER 
                || (IsCollision(enemy[index])!=0));

        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            switch(CurrentScreenMode)
            {
                #region CurrentScreenMode == 0
                case 0:
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption > 3)
                            CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption < 0)
                            CurrentMenuOption = 3;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        try
                        {
                            soundsPlaying[10] = sounds[10].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        TimeLastScreenModeChange = Environment.TickCount;
                        switch (CurrentMenuOption)
                        {
                            case 0:
                                Network.CreateGame();
                                CurrentScreenMode = 1;
                                break;
                            case 1:
                                Network.FindGames();
                                CurrentScreenMode = 2;
                                break;
                            case 2:
                                StorageUnit.DoLoadGame();
                                CurrentScreenMode = 5;
                                break;
                            case 3:
                                this.Exit();
                                break;
                        }
                        CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 1;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
                    break;
                #endregion
                #region CurrentScreenMode == 1
                case 1:
#if (!WINDOWS)
                    if (Network.Session != null &&
                        Network.Session.IsHost)
#endif
                        RandomQuest.Update();
                    #region Network, Velocity checking
#if (!WINDOWS)
                    if (Network.Session == null)
                    {
                        CurrentScreenMode = 0;
                        return;
                    }
#endif
#if (!WINDOWS)
                    if (Network.Session.IsHost)
#endif
                    EnemyAI(gameTime);
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 3;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
                        GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed &&
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 6;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }


                    const float VelMod = .005f;
                    const float StickMod = .25f;
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > StickMod)
#endif
                        Player.Velocity.X += VelMod;
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -StickMod)
#endif
                        Player.Velocity.X -= VelMod;
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > StickMod)
#endif
                        Player.Velocity.Y -= VelMod;
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -StickMod)
#endif
                        Player.Velocity.Y += VelMod;

                    Player.Update();
                    if (IsCollision(Player)!=0)
                    {
                        Player.Pos -= Player.Velocity;
                        Player.Velocity = Vector2.Zero;
                    }

                    if (Player.Velocity.X > 0)
                    {
                        if (Player.Velocity.Y > 0.001)
                        {
                            if (Player.Velocity.X > Player.Velocity.Y)
                            { Player.CurrentAnimation = 3; }
                            else { Player.CurrentAnimation = 0; }
                        }
                        else
                        {
                            if (Player.Velocity.X > -Player.Velocity.Y)
                            { Player.CurrentAnimation = 3; }
                            else { Player.CurrentAnimation = 1; }
                        }
                    }
                    else if (Player.Velocity.X < 0.001)
                    {
                        if (Player.Velocity.Y > 0.001)
                        {
                            if (-Player.Velocity.X > Player.Velocity.Y)
                            { Player.CurrentAnimation = 2; }
                            else { Player.CurrentAnimation = 0; }
                        }
                        else
                        {
                            if (Player.Velocity.X > Player.Velocity.Y)
                            { Player.CurrentAnimation = 1; }
                            else { Player.CurrentAnimation = 2; }
                        }
                    }
                    #endregion
                    #region Death
                    if (Player.CurrentHealth < 1)
                    {
                        Player.CurrentHealth = Player.Health;
                        //go to nearest city
                        float distanceBHamHam = Fast2d(Player.Pos, LocationBHamHam);
                        float distanceJroberton = Fast2d(Player.Pos, LocationJRoberton);
                        float distanceDickinsongrad = Fast2d(Player.Pos, LocationDickinsongrad);
                        float distanceGargoyle = Fast2d(Player.Pos, LocationGargoyle);
                        if(distanceBHamHam < distanceDickinsongrad &&
                            distanceBHamHam < distanceJroberton &&
                            distanceBHamHam < distanceGargoyle)
                            Player.Pos = LocationBHamHam;
                        else if (distanceDickinsongrad < distanceBHamHam &&
                            distanceDickinsongrad < distanceJroberton &&
                            distanceDickinsongrad < distanceGargoyle)
                            Player.Pos = LocationDickinsongrad;
                        else if (distanceJroberton < distanceDickinsongrad &&
                            distanceJroberton < distanceBHamHam &&
                            distanceJroberton < distanceGargoyle)
                            Player.Pos = LocationJRoberton;
                        else if (distanceGargoyle < distanceDickinsongrad &&
                            distanceGargoyle < distanceBHamHam &&
                            distanceGargoyle < distanceJroberton)
                            Player.Pos = LocationGargoyle;
                        Player.Velocity = Vector2.Zero;
                        try
                        {
                            soundsPlaying[3] = sounds[3].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                    }
                    #endregion Death
                    #region Attack
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
#endif
                    {
                        int numAttacked = 0;
                        ++attackPressed;
                        if (!Player.isAttacking)
                            Player.isAttacking = true;
                        if(attackPressed > 60) attackPressed = 1;
                        for (int i = 0; i < MAX_ENEMIES; ++i)
                            if (Fast2d(enemy[i].Pos, Player.Pos) < 1.0f)
                            {
                                numAttacked++;
#if (!WINDOWS)
                                if (Network.Session != null &&
                                    Network.Session.IsHost)
#endif
                                {

                                    Player.Attack(enemy[i]);
                                    if (enemy[i].CurrentHealth < 1)
                                    {
                                        Player.XP += (Int16)(enemy[i].Level * (enemy[i].Dexterity + enemy[i].Speed + enemy[i].Strength));
#if (!WINDOWS)
                                        if (Network.Session.IsHost)
#endif
                                        {
                                            if (enemy[i].Flag == 1)
                                                --RandomQuest.RandomQuestFlag;
                                            SpawnEnemy(i);
                                        }
                                        try
                                        {
                                            soundsPlaying[8] = sounds[8].Play();
                                        }
                                        catch(Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }
                                   
                                }
#if (!WINDOWS)
                                else
                                    Network.PlayerAttack(i);
#endif
                            }

                        try
                        {
                            if (attackPressed == 1) 
                            {
                                if (numAttacked >= 1) soundsPlaying[0] = sounds[0].Play();
                                else soundsPlaying[1] = sounds[1].Play();
                            }
                            
                            //int current;// = rand.Next() % 6;
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        attackPressed = 0;
                    }
                    #endregion
                    #region Music, Storyline quests

                    if (!visitedBHamHam)
                        if (Fast2d(Player.Pos, LocationBHamHam) < 20f)
                            visitedBHamHam = true;
                    if (!visitedDickinsongrad)
                        if (Fast2d(Player.Pos, LocationDickinsongrad) < 20f)
                            visitedDickinsongrad = true;
                    if (!visitedJRoberton)
                        if (Fast2d(Player.Pos, LocationJRoberton) < 20f)
                            visitedJRoberton = true;
                    if (!visitedGargoyle)
                        if (Fast2d(Player.Pos, LocationGargoyle) < 20f)
                            visitedGargoyle = true;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        lastButtonPressed = 1;
                    }
                    else
                    {
                        lastButtonPressed = 0;
                    }
                    //
                    //  Play/Pause Button advances to next song
                    //
                    if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                    {
                        lastButtonPressed = 2;
                    }
                    else
                    {
                        lastButtonPressed = 0;
                    }
                    // check if player is going to interact 
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed && Network.Session.IsHost)
#endif
                    {
                        //background = new Color(255, 255, 255);
                        //red = green = blue = 0;
                        ++interactPressed;
                        if (interactPressed > 60) interactPressed = 1;
                        bool interacted = false;
                        lastButtonPressed = 0;

                        if (interactPressed == 1)
                        {
                            for (int i = 0; i < NPC.Length; i++)
                            {

                                if (Fast2d(Player.Pos, NPC[i].Pos) < 2 && NPC[i].alive)
                                {
                                    interacted = true;
                                    switch (i)
                                    {
                                        case 0:// Shaniqua
                                            displayMessage(1, 90);
                                            break;
                                        case 1: //amulet
                                            //currentMessage = 2;
                                            displayMessage(2, 90);
                                            NPC[1].alive = false; //make amulet disappear when you take it
                                            NPC[0].alive = false; //make penguin disappear
                                            break;
                                        case 2: //sword
                                            displayMessage(3, 90);
                                            NPC[i].alive = false;
                                            break;
                                        case 3: //blacksmith
                                            displayMessage(4, 90);
                                            //NPC[i].alive = false;
                                            break;
                                        case 4: //gargoyle
                                            displayMessage(5, 90);
                                            NPC[3].alive = false; //make blacksmith disappear

                                            //quick move to gargoyle - for testing only
                                            //Player.Pos = LocationGargoyle + new Vector2(-1.5f, 1.0f);
                                            
                                            
                                            break;
                                        case 9: //battle axe
                                            //if (puzzleSolved && NPC[i].alive = false)
                                            //{
                                                displayMessage(6, 90);
                                                NPC[i].alive = false;

                                                //quick move to DickinsonGrad - for testing only
                                                //Player.Pos = LocationDickinsongrad;

                                            //}
                                            //else
                                            ///{
                                            //    Player.CurrentHealth < 1
                                            //    displayMessage(7, 90);
                                            //}
                                            //Player.Pos = LocationGargoyle;
                                            break;
                                        case 25: //santa
                                            // if puzzle was solved and axe was picked up...
                                            if (puzzleSolved && !NPC[9].alive)
                                            {
                                                displayMessage(7, 90);
                                                NPC[i].alive = false;
                                                // make jester alive
                                                NPC[27].alive = true;
                                                
                                            }
                                            else
                                            {
                                                Player.CurrentHealth = 0;
                                                displayMessage(8, 90);
                                            }
                                            
                                            //NPC[i].alive = false;
                                            //Player.Pos = LocationGargoyle;
                                            break;
                                        case 26: //catapult
                                            displayMessage(10, 90);
                                            NPC[i].alive = false; //kill catapult
                                            break;
                                        case 27: // jester
                                            displayMessage(9, 90);
                                            NPC[26].alive = true; //make catapult visible
                                            break;

                                    }
                                    // gargoyle treasure area
                                    if (i >= 5 && i <= 8)
                                    {

                                        if (!gargoyleActivated[i - 5])
                                        {
                                            gargoyleActivated[i - 5] = true;
                                            gargoyleActivationOrder[i - 5] = numGargoylesActivated;
                                            numGargoylesActivated++;
                                            try
                                            {
                                                soundsPlaying[4] = sounds[4].Play();
                                            }
                                            catch (Exception e)
                                            {
                                                e.ToString();
                                            }
                                        }

                                    }
                                    if (i >= 20 && i <= 22)
                                    {
                                        RandomQuest.SageQuestInitiated = true;
                                        //treasureGotten++;
                                        //displayMessage(100, 90);
                                        //NPC[i].alive = false;
                                    }
                                    if (i >= 10 && i <= 19)
                                    {
                                        treasureGotten++;
                                        displayMessage(100, 90);
                                        NPC[i].alive = false;
                                        try
                                        {
                                            soundsPlaying[6] = sounds[6].Play();
                                        }
                                        catch (Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }

                                }

                            }

                            if (!interacted)
                            {
                                displayMessage(currentMessage, 90);
                            }
                            if (gargoyleActivated[0] && gargoyleActivated[1] && gargoyleActivated[2] && gargoyleActivated[3] && !puzzleSolved)
                            {
                                if (gargoyleActivationOrder[0] == 3 && gargoyleActivationOrder[1] == 2 && gargoyleActivationOrder[2] == 0 && gargoyleActivationOrder[3] == 1)
                                {
                                    //gargoyle
                                    NPC[9].alive = true;
                                    puzzleSolved = true;
                                }
                                else
                                {
                                    try
                                    {
                                        soundsPlaying[5] = sounds[5].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                    for (int i = 0; i < MAX_PUZZLE_GARGOYLES; i++)
                                    {
                                        gargoyleActivated[i] = false;
                                        gargoyleActivationOrder[i] = -1;
                                    }
                                    numGargoylesActivated = 0;
                                }


                            }

                            //MediaPlayer.Stop();// Play(songs[currentSong++]);
                            lastButtonPressed = 0;

                        }
                    }
                    else
                    {
                        interactPressed = 0;
                    }
                    #endregion
                    break;
                #endregion
                #region CurrentScreenMode == 2
                case 2:
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
#endif
                            Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        if (Network.AvailableSessions != null &&
                            CurrentMenuOption > Network.AvailableSessions.Count - 1)
                            CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
#endif
                            Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        if (Network.AvailableSessions != null &&
                            CurrentMenuOption < 0)
                            CurrentMenuOption = Network.AvailableSessions.Count - 1;
                    }

#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 0;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        TimeLastScreenModeChange = Environment.TickCount;
                        Network.JoinGame(CurrentMenuOption);
                        Player.Pos = LocationBHamHam;
                        CurrentMenuOption = 0;
                        CurrentScreenMode = 1;
                    }
                    break;
                #endregion
                #region CurrentScreenMode == 3
                case 3:
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        if (CurrentMenuOption > 5)
                            CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        if (CurrentMenuOption < 0)
                            CurrentMenuOption = 5;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 1;
                        CurrentMenuOption = 0;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        TimeLastScreenModeChange = Environment.TickCount;
                        switch (CurrentMenuOption)
                        {
                            case 0:
                                CurrentScreenMode = 1;
                                Network.CreateGame();
                                break;
                            case 1:
                                if (MusicOn)
                                {
                                    currentSong++;
                                    if (currentSong >= songs.Count) currentSong = 0;
                                    try
                                    {
                                        MediaPlayer.Play(songs[currentSong]);
                                        MediaPlayer.Volume = 1.0f;
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                break;
                            case 2:
                                TimeLastMenuChange = Environment.TickCount;
                                CurrentScreenMode = 4;
                                break;
                            case 3:
                                CurrentScreenMode = 1;
                                try
                                {
                                    MediaPlayer.Stop();
                                }
                                catch (Exception e)
                                {
                                    e.ToString();
                                }
                                MusicOn = MusicOn ? !MusicOn : MusicOn;
                                break;
                            case 4:
                                Network.ShutDown();
                                StorageUnit.DoSaveGame(Player);
                                CurrentScreenMode = 0;
                                break;
                            case 5:
                                StorageUnit.DoSaveGame(Player);
                                this.Exit();
                                break;
                        }
                        CurrentMenuOption = 0;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 1;
                        CurrentMenuOption = 0;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
                    break;
                #endregion
                #region CurrentScreenMode == 4
                case 4:
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                    Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 3;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption > 2)
                            CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption < 0)
                            CurrentMenuOption = 2;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > StickMod &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 500 &&
                        Player.UnassignedStatPoints > 0)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        Player.UnassignedStatPoints--;
                        switch (CurrentMenuOption)
                        {
                            case 0:
                                Player.Strength++;
                                break;
                            case 1:
                                Player.Dexterity++;
                                break;
                            case 2:
                                Player.Speed++;
                                break;
                        }
                        Network.PlayerStatsUpdate(Player);
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastMenuChange > 1000)
                        if (Player.UnassignedStatPoints >= 5)
                        {
                            Player.UnassignedStatPoints -= 5;
                            switch (CurrentMenuOption)
                            {
                                case 0:
                                    Player.Strength += 5;
                                    break;
                                case 1:
                                    Player.Dexterity += 5;
                                    break;
                                case 2:
                                    Player.Speed += 5;
                                    break;
                            }
                        }
                    //CHEATS
#if(!WINDOWS)
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .3f &&
                        GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
                        GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
#else
                    if(Keyboard.GetState().IsKeyDown(Keys.Left) &&
                        Keyboard.GetState().IsKeyDown(Keys.Enter) &&
                        Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
#endif
                        Player.XP+=100;
                    break;
                #endregion
                #region CurrentScreenMode == 5
                case 5:
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
#endif
                            Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        if (CurrentMenuOption > StorageUnit.PlayersLoaded.Count-1)
                            CurrentMenuOption = 0;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
#endif
                            Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        if (CurrentMenuOption < 0)
                            CurrentMenuOption = StorageUnit.PlayersLoaded.Count-1;
                    }

#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 0;
                        CurrentMenuOption = 0;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
#if (WINDOWS)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) &&
#else
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
#endif
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        TimeLastScreenModeChange = Environment.TickCount;
                        Player = (LivingEntity.PlayerEntity)StorageUnit.PlayersLoaded[CurrentMenuOption];
                        StorageUnit = new Storage();
                        CurrentMenuOption = 0;
                        CurrentScreenMode = 0;
                    }
                    break;
                #endregion
                #region CurrentScreenMode == 6
                case 6:
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.4f &&
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption++;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption > 3)
                            CurrentMenuOption = 0;
                    }
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .4f &&
                        Environment.TickCount - TimeLastMenuChange > 500)
                    {
                        TimeLastMenuChange = Environment.TickCount;
                        CurrentMenuOption--;
                        try
                        {
                            soundsPlaying[7] = sounds[7].Play();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        if (CurrentMenuOption < 0)
                            CurrentMenuOption = 3;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
                        Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        TimeLastScreenModeChange = Environment.TickCount;
                        switch (CurrentMenuOption)
                        {
                            case 0: //Bhamham
                                //Network.FindGames();
                                //CurrentScreenMode = 2;
                                if (visitedBHamHam)
                                {
                                    Player.Pos = LocationBHamHam;
                                    CurrentScreenMode = 1;
                                    TimeLastScreenModeChange = Environment.TickCount;
                                    try
                                    {
                                        soundsPlaying[9] = sounds[9].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                break;
                            case 1: //Jroberton
                                //Network.CreateGame();
                                //CurrentScreenMode = 1;
                                if (visitedJRoberton)
                                {
                                    Player.Pos = LocationJRoberton;
                                    CurrentScreenMode = 1;
                                    TimeLastScreenModeChange = Environment.TickCount;
                                    try
                                    {
                                        soundsPlaying[9] = sounds[9].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                break;
                            case 2: //Dickinsongrad
                                //StorageUnit.DoLoadGame();
                                //CurrentScreenMode = 5;
                                if (visitedDickinsongrad)
                                {
                                    Player.Pos = LocationDickinsongrad;
                                    CurrentScreenMode = 1;
                                    TimeLastScreenModeChange = Environment.TickCount;
                                    try
                                    {
                                        soundsPlaying[9] = sounds[9].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                break;
                            case 3: //Gargoyles
                                //this.Exit();
                                if (visitedGargoyle)
                                {
                                    Player.Pos = LocationGargoyle;
                                    CurrentScreenMode = 1;
                                    TimeLastScreenModeChange = Environment.TickCount;
                                    try
                                    {
                                        soundsPlaying[9] = sounds[9].Play();
                                    }
                                    catch (Exception e)
                                    {
                                        e.ToString();
                                    }
                                }
                                break;
                        }
                        CurrentMenuOption = 0;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed &&
                    Environment.TickCount - TimeLastScreenModeChange > 500)
                    {
                        CurrentScreenMode = 1;
                        TimeLastScreenModeChange = Environment.TickCount;
                    }
                    break;

                #endregion
                //#region CurrentScreenMode == 7 
                //case 7: // intro text
                //    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed &&
                //        Environment.TickCount - TimeLastScreenModeChange > 500)
                //    {
                //        TimeLastScreenModeChange = Environment.TickCount;
                //        CurrentScreenMode = 1;
                        
                //    }
                //#endregion

            }
            if(MusicOn)
                MusicUpdate(gameTime);
            Network.Update();
            base.Update(gameTime);
        }
        protected void displayMessage(int messageNum, int desiredTime)
        {
            currentMessage = messageNum;
            messagetime = desiredTime;
        }
    }
}
