using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;

namespace ERMotA
{
    /// <summary>
    /// Generic living entity. This will encompass player, enemies, friends.
    /// To be general I figure shouldn't add stuff like currentquest, currentweapon, etc.
    /// and for ease of implementation i figure we wait for stuff like experience, level,
    /// etc, and see if we have the resources to even put them in the game.
    /// </summary>
    public class LivingEntity
    {
        #region Defines
        public const Int16 LivingEntityRadius = 16;     //making these constants so that we don't have to calculate 
        public const Int16 LivingEntityRadiusT2 = 32;   //LivingEntityRadiusT2 4*7*9 times = 252 times per frame.
        public Int32 XP;
        public Int16 Health, CurrentAnimation, CurrentFrame, CurrentHealth, Strength,
            Dexterity, Speed, Level, UnassignedStatPoints;
        public Int16 CurrentAnimationAttack, CurrentFrameAttack;
        public Rectangle[][] rectAnimation;
        public Rectangle[][] rectAnimationAttacks;
        public Vector2 Pos, Velocity;
        public int TimeOfLastAttack, TimeOfLastAnimStateChange, TimeOfLastHealthRegen;
        public Texture2D texLE;
        public const Int16 lengthSabin = 30;
        public int Flag;
        public const Int16 widthSabin = 20;
        public bool isAttacking = false;
        public Ability[] Abilities;
        public Texture2D[][] texAnimation;
        #endregion

        public LivingEntity()
        {
            Flag = 0;
            XP = UnassignedStatPoints = CurrentAnimation = CurrentFrame = 0;
            CurrentAnimationAttack = CurrentFrameAttack = 0;
            Level = 1;
            Speed = 8;
            Health = CurrentHealth = Strength = Dexterity = 8;
            TimeOfLastHealthRegen = TimeOfLastAnimStateChange = TimeOfLastAttack = Environment.TickCount;
            Pos = new Vector2(10, 10);
            Velocity = new Vector2(0, 0);
        }
        public LivingEntity(Vector2 Position)
            : this()
        {
            Pos = Position;
        }
        public LivingEntity(Texture2D tex)
            : this()
        {
            texLE = tex;
        }
        public LivingEntity(Texture2D tex, Vector2 Position)
            : this()
        {
            Pos = Position; texLE = tex;
        }
        public LivingEntity(Int16 Power)
            : this()
        {
            Dexterity = Strength = Health = CurrentHealth = (Int16)(Power * 6);
        }
        public Int16 Attack(LivingEntity opponent)
        {
            if (Environment.TickCount - TimeOfLastAttack > 5000 / Speed)
            {
                TimeOfLastAttack = Environment.TickCount;
                Int16 Damage = 0;
                try
                {
                    Damage = (Int16)Math.Ceiling((double)Strength / (opponent.Dexterity / 3));
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                opponent.CurrentHealth -= Damage;

                return Damage;
            }
            return 0;
        }
        public void Update()
        {
            //Level Up
            if (XP > Level * (100 + 2 * Level + Level * Level / 30))
            {
                UnassignedStatPoints += 5;
                Health += 2;
                CurrentHealth = Health;
                Level++;
            }

            //Animation
            if (Environment.TickCount - TimeOfLastAnimStateChange > 840 / Speed)
            {
                TimeOfLastAnimStateChange = Environment.TickCount;
                if (++CurrentFrame >= texAnimation[CurrentAnimation].Length)
                {
                    CurrentFrame = 0;
                    if (isAttacking)
                    {
                        if (++CurrentFrameAttack >= Abilities[CurrentAnimationAttack].texAbility.Length)
                        {
                            CurrentFrameAttack = 0;
                            isAttacking = false;
                        }
                    }
                }
            }

            //Movement
            const float MovementMod = 0.0008f;
            if (Velocity.X > MovementMod * (float)(Speed + 50))
                Velocity.X = MovementMod * (float)(Speed + 50);
            else if (Velocity.X < -MovementMod * (float)(Speed + 50))
                Velocity.X = -MovementMod * (float)(Speed + 50);
            if (Velocity.Y > MovementMod * (float)(Speed + 50))
                Velocity.Y = MovementMod * (float)(Speed + 50);
            else if (Velocity.Y < -MovementMod * (float)(Speed + 50))
                Velocity.Y = -MovementMod * (float)(Speed + 50);
            Velocity /= 1.05f;
            Pos += Velocity;

            //Health regen
            if (Environment.TickCount - TimeOfLastHealthRegen > 2000 &&  //if not in battle, regen
                CurrentHealth < Health)
            {
                CurrentHealth += (Int16)(Dexterity / 10);
                TimeOfLastHealthRegen = Environment.TickCount;
            }
            if (CurrentHealth > Health)
                CurrentHealth = Health;
        }
        public class PlayerEntity : LivingEntity
        {
            #region Defines

            public const int ATTACKLIGHT = 0;

            #endregion

            public PlayerEntity()
                : base()
            {
                CurrentAnimationAttack = 0;
                CurrentFrameAttack = 0;

                Abilities = new Ability[1];
                Abilities[0] = new AttackLight();

                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[4];
                texAnimation[1] = new Texture2D[4];
                texAnimation[2] = new Texture2D[4];
                texAnimation[3] = new Texture2D[4];

                texAnimation[0][0] = Game1.TexSabinSouth1of4;
                texAnimation[0][1] = Game1.TexSabinSouth2of4;
                texAnimation[0][2] = Game1.TexSabinSouth3of4;
                texAnimation[0][3] = Game1.TexSabinSouth4of4;

                texAnimation[1][0] = Game1.TexSabinNorth1of4;
                texAnimation[1][1] = Game1.TexSabinNorth2of4;
                texAnimation[1][2] = Game1.TexSabinNorth3of4;
                texAnimation[1][3] = Game1.TexSabinNorth4of4;

                texAnimation[2][0] = Game1.TexSabinWest1of4;
                texAnimation[2][1] = Game1.TexSabinWest2of4;
                texAnimation[2][2] = Game1.TexSabinWest3of4;
                texAnimation[2][3] = Game1.TexSabinWest4of4;

                texAnimation[3][0] = Game1.TexSabinEast1of4;
                texAnimation[3][1] = Game1.TexSabinEast2of4;
                texAnimation[3][2] = Game1.TexSabinEast3of4;
                texAnimation[3][3] = Game1.TexSabinEast4of4;

            }

        }
        public class EnemyEntity : LivingEntity
        {
            public EnemyEntity(Int16 Power)
                : base(Power)
            {
            }
        }
        public class GreenEnemy : EnemyEntity
        {
            const int SizeOfAnimWalk = 8;

            public GreenEnemy(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[8];
                texAnimation[1] = new Texture2D[8];
                texAnimation[2] = new Texture2D[8];
                texAnimation[3] = new Texture2D[8];

                texAnimation[0][0] = Game1.TexLinkSouth1of8;
                texAnimation[0][1] = Game1.TexLinkSouth2of8;
                texAnimation[0][2] = Game1.TexLinkSouth3of8;
                texAnimation[0][3] = Game1.TexLinkSouth4of8;
                texAnimation[0][4] = Game1.TexLinkSouth5of8;
                texAnimation[0][5] = Game1.TexLinkSouth6of8;
                texAnimation[0][6] = Game1.TexLinkSouth7of8;
                texAnimation[0][7] = Game1.TexLinkSouth8of8;

                texAnimation[1][0] = Game1.TexLinkNorth1of8;
                texAnimation[1][1] = Game1.TexLinkNorth2of8;
                texAnimation[1][2] = Game1.TexLinkNorth3of8;
                texAnimation[1][3] = Game1.TexLinkNorth4of8;
                texAnimation[1][4] = Game1.TexLinkNorth5of8;
                texAnimation[1][5] = Game1.TexLinkNorth6of8;
                texAnimation[1][6] = Game1.TexLinkNorth7of8;
                texAnimation[1][7] = Game1.TexLinkNorth8of8;

                texAnimation[2][0] = Game1.TexLinkWest1of8;
                texAnimation[2][1] = Game1.TexLinkWest2of8;
                texAnimation[2][2] = Game1.TexLinkWest3of8;
                texAnimation[2][3] = Game1.TexLinkWest4of8;
                texAnimation[2][4] = Game1.TexLinkWest5of8;
                texAnimation[2][5] = Game1.TexLinkWest6of8;
                texAnimation[2][6] = Game1.TexLinkWest7of8;
                texAnimation[2][7] = Game1.TexLinkWest8of8;

                texAnimation[3][0] = Game1.TexLinkEast1of8;
                texAnimation[3][1] = Game1.TexLinkEast2of8;
                texAnimation[3][2] = Game1.TexLinkEast3of8;
                texAnimation[3][3] = Game1.TexLinkEast4of8;
                texAnimation[3][4] = Game1.TexLinkEast5of8;
                texAnimation[3][5] = Game1.TexLinkEast6of8;
                texAnimation[3][6] = Game1.TexLinkEast7of8;
                texAnimation[3][7] = Game1.TexLinkEast8of8;

            }
        }
        public class EnemyFrog : EnemyEntity
        {
            const int SizeOfAnimWalk = 4;
            public EnemyFrog(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[4];
                texAnimation[1] = new Texture2D[4];
                texAnimation[2] = new Texture2D[4];
                texAnimation[3] = new Texture2D[4];

                texAnimation[0][0] = Game1.TexFrogSouth1;
                texAnimation[0][1] = Game1.TexFrogSouth2;
                texAnimation[0][2] = Game1.TexFrogSouth3;
                texAnimation[0][3] = Game1.TexFrogSouth4;

                texAnimation[1][0] = Game1.TexFrogNorth1;
                texAnimation[1][1] = Game1.TexFrogNorth2;
                texAnimation[1][2] = Game1.TexFrogNorth3;
                texAnimation[1][3] = Game1.TexFrogNorth4;

                texAnimation[2][0] = Game1.TexFrogWest1;
                texAnimation[2][1] = Game1.TexFrogWest2;
                texAnimation[2][2] = Game1.TexFrogWest3;
                texAnimation[2][3] = Game1.TexFrogWest4;

                texAnimation[3][0] = Game1.TexFrogEast1;
                texAnimation[3][1] = Game1.TexFrogEast2;
                texAnimation[3][2] = Game1.TexFrogEast3;
                texAnimation[3][3] = Game1.TexFrogEast4;
            }
        }
        public class EnemyWaterThug : EnemyEntity
        {
            const int SizeOfAnimWalk = 3;

            public EnemyWaterThug(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[3];
                texAnimation[1] = new Texture2D[3];
                texAnimation[2] = new Texture2D[3];
                texAnimation[3] = new Texture2D[3];

                texAnimation[0][0] = Game1.TexWaterThugSouth1;
                texAnimation[0][1] = Game1.TexWaterThugSouth2;
                texAnimation[0][2] = Game1.TexWaterThugSouth3;

                texAnimation[1][0] = Game1.TexWaterThugNorth1;
                texAnimation[1][1] = Game1.TexWaterThugNorth2;
                texAnimation[1][2] = Game1.TexWaterThugNorth3;

                texAnimation[2][0] = Game1.TexWaterThugWest1;
                texAnimation[2][1] = Game1.TexWaterThugWest2;
                texAnimation[2][2] = Game1.TexWaterThugWest3;

                texAnimation[3][0] = Game1.TexWaterThugEast1;
                texAnimation[3][1] = Game1.TexWaterThugEast2;
                texAnimation[3][2] = Game1.TexWaterThugEast3;
            }
        }
        public class EnemyZombie : EnemyEntity
        {
            const int SizeOfAnimWalk = 3;

            public EnemyZombie(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[3];
                texAnimation[1] = new Texture2D[3];
                texAnimation[2] = new Texture2D[3];
                texAnimation[3] = new Texture2D[3];

                texAnimation[0][0] = Game1.TexZombieSouth1;
                texAnimation[0][1] = Game1.TexZombieSouth2;
                texAnimation[0][2] = Game1.TexZombieSouth3;

                texAnimation[1][0] = Game1.TexZombieNorth1;
                texAnimation[1][1] = Game1.TexZombieNorth2;
                texAnimation[1][2] = Game1.TexZombieNorth3;

                texAnimation[2][0] = Game1.TexZombieWest1;
                texAnimation[2][1] = Game1.TexZombieWest2;
                texAnimation[2][2] = Game1.TexZombieWest2;  //TODO index out of bounds, so hack it!

                texAnimation[3][0] = Game1.TexZombieEast1;
                texAnimation[3][1] = Game1.TexZombieEast2;
                texAnimation[3][2] = Game1.TexZombieEast2;
            }
        }
        public class EnemyDarkStalker : EnemyEntity
        {
            const int SizeOfAnimWalk = 4;

            public EnemyDarkStalker(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[4];
                texAnimation[1] = new Texture2D[4];
                texAnimation[2] = new Texture2D[4];
                texAnimation[3] = new Texture2D[4];

                texAnimation[0][0] = Game1.TexDarkStalkerSouth2;
                texAnimation[0][1] = Game1.TexDarkStalkerSouth1;
                texAnimation[0][2] = Game1.TexDarkStalkerSouth2;
                texAnimation[0][3] = Game1.TexDarkStalkerSouth3;

                texAnimation[1][0] = Game1.TexDarkStalkerNorth2;
                texAnimation[1][1] = Game1.TexDarkStalkerNorth1;
                texAnimation[1][2] = Game1.TexDarkStalkerNorth2;
                texAnimation[1][3] = Game1.TexDarkStalkerNorth3;

                texAnimation[2][0] = Game1.TexDarkStalkerWest2;
                texAnimation[2][1] = Game1.TexDarkStalkerWest1;
                texAnimation[2][2] = Game1.TexDarkStalkerWest2;
                texAnimation[2][3] = Game1.TexDarkStalkerWest3;

                texAnimation[3][0] = Game1.TexDarkStalkerEast2;
                texAnimation[3][1] = Game1.TexDarkStalkerEast1;
                texAnimation[3][2] = Game1.TexDarkStalkerEast2;
                texAnimation[3][3] = Game1.TexDarkStalkerEast3;
            }
        }
        public class EnemyNeedlion : EnemyEntity
        {
            const int SizeOfAnimWalk = 3;

            public EnemyNeedlion(Int16 Power)
                : base(Power)
            {
                texAnimation = new Texture2D[4][];
                texAnimation[0] = new Texture2D[3];
                texAnimation[1] = new Texture2D[3];
                texAnimation[2] = new Texture2D[3];
                texAnimation[3] = new Texture2D[3];

                texAnimation[0][0] = Game1.TexNeedlionSouth1;
                texAnimation[0][1] = Game1.TexNeedlionSouth2;
                texAnimation[0][2] = Game1.TexNeedlionSouth3;

                texAnimation[1][0] = Game1.TexNeedlionNorth1;
                texAnimation[1][1] = Game1.TexNeedlionNorth2;
                texAnimation[1][2] = Game1.TexNeedlionNorth3;

                texAnimation[2][0] = Game1.TexNeedlionWest1;
                texAnimation[2][1] = Game1.TexNeedlionWest2;
                texAnimation[2][2] = Game1.TexNeedlionWest3;

                texAnimation[3][0] = Game1.TexNeedlionEast1;
                texAnimation[3][1] = Game1.TexNeedlionEast2;
                texAnimation[3][2] = Game1.TexNeedlionEast3;
            }
        }
        public class Ability
        {
            public Texture2D[] texAbility;
            public Int16 numDamage;
            public Int16 timeRefresh;
            public readonly Int16 heightAbility;
            public readonly Int16 widthAbility;
            public readonly Int16 sizeofAnimation;

            public Ability()
            { }

            public Ability(Int16 nDamage, Int16 tRefresh, Int16 hA, Int16 wA, Int16 nSize)
            {
                //texAbility=tAbility;
                //rectAbility = aRect;
                numDamage = nDamage;
                timeRefresh = tRefresh;
                heightAbility = hA;
                widthAbility = wA;
                sizeofAnimation = nSize;
            }
        }
        public class AttackLight : Ability
        {
            const Int16 csizeofAnimation = 5;
            const Int16 cwidthAbility = 42;
            const Int16 cheightAbility = 46;
            const Int16 cnumDamage = 5;
            const Int16 ctimeRefresh = 8;
            //Texture2D TextureAttackLight;


            public AttackLight()
                : base(cnumDamage, ctimeRefresh, cheightAbility, cwidthAbility, csizeofAnimation)
            {
                texAbility = new Texture2D[5];
                texAbility[0] = Game1.TextureAttackLight1of5;
                texAbility[1] = Game1.TextureAttackLight2of5;
                texAbility[2] = Game1.TextureAttackLight3of5;
                texAbility[3] = Game1.TextureAttackLight4of5;
                texAbility[4] = Game1.TextureAttackLight5of5;
            }
        }
        public class NPCEntity : LivingEntity
        {

            #region Defines
            public bool alive;
    
            #endregion

            public NPCEntity(Texture2D tex)
                : base()
            {
                alive = true;
                texLE = tex;
                rectAnimation = new Rectangle[4][];
                rectAnimation[0] = new Rectangle[4];
                rectAnimation[1] = new Rectangle[4];
                rectAnimation[2] = new Rectangle[4];
                rectAnimation[3] = new Rectangle[4];

                for (Int16 i = 0; i < 4; ++i)
                {
                    for (Int16 j = 0; j < 3; ++j)
                    {
                        rectAnimation[i][j] = new Rectangle(i * widthSabin + i * 10, j * lengthSabin, widthSabin, lengthSabin);
                    }
                }

                for (Int16 i = 0; i < 4; ++i)
                {
                    rectAnimation[i][3] = new Rectangle(i * widthSabin + i * 10, 0, widthSabin, lengthSabin);
                }
            }
        }
    }
}
