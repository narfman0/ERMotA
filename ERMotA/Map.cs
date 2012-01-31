using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ERMotA
{
    /*
     * Map class to randomly generate two dimensional array with random numbers
     * Plows paths for town areas and paths.
     * 0 = passable grassland
     * 1 = passable woods
     * 2 = impassable wood cliffs
     * 3 = passable desert
     * 4 = impassable cliffs
     * 5 = water
     * 6 = passable dirt path
     * 100 = house top left (generic locals)
     * 101 = house top right
     * 102 = house bottom left
     * 103 = house bottom right
     * 110 = blacksmith top left
     * 111 = blacksmith top right
     * 112 = blacksmith bottom left
     * 113 = blacksmith bottom right
     * */
    public class Map
    {
        Random Rand;
        public const Int32 MapSize = 512;   //MapSize=1024 yields 2 meg

        public Int16[,] MapArray = new Int16[MapSize, MapSize];
        public Map()
        {
            Rand = new Random();
            //Add randomness first
            for (Int32 i = 0; i < MapSize; i++)
                for (Int32 j = 0; j < MapSize; j++)
                {
                    float PathSwitch = (1.6f + (float)(Rand.Next()) % 3) / 1.5f;
                    MapArray[i, j] = (Int16)Math.Floor(PathSwitch);
                }

            //Add random rivers
            for (int i = 0; i < 16; ++i)
            {
                int x = 5 + Rand.Next() % (MapSize - 10);
                int y = 5 + Rand.Next() % (MapSize - 10);
                int Length = 12 + Rand.Next() % 28;
                MapArray[x, y - 1] = 5;
                while (y < MapSize - 2 && Length > 0)
                {
                    Length--;
                    MapArray[x, y] = 5;
                    if (Rand.Next() % 8 != 0)
                        MapArray[x + 1, y] = 5;
                    if (Rand.Next() % 8 != 0)
                        MapArray[x - 1, y] = 5;
                    y++;
                }
                MapArray[x, y + 1] = 5;
            }


            //Add JRoberton desert
            int RightmostDesertBoundary = MapSize / 2;
            for (Int32 y = 0; y < MapSize; ++y)
            {
                if (Rand.Next(3) == 0) --RightmostDesertBoundary;
                for (Int32 x = 0; x < RightmostDesertBoundary; ++x)
                    MapArray[x, y] = 3;
            }

            //Force add grassland for Bhamham
            for (Int32 i = MapSize / 2 - MapSize / 16; i < MapSize / 2 + MapSize / 16; i++)
                for (Int32 j = MapSize - MapSize / 8; j < MapSize; j++)
                    MapArray[i, j] = 0;

            //Force add grassland for Dickinsongrad
            for (Int32 x = (Int32)Game1.LocationDickinsongrad.X - 5; x < (Int32)Game1.LocationDickinsongrad.X + 5; x++)
                for (Int32 y = (Int32)Game1.LocationDickinsongrad.Y - 5; y < (Int32)Game1.LocationDickinsongrad.Y + 5; y++)
                    MapArray[x, y] = 0;


            //Force add grassland for Gargoyle
            for (Int32 x = (Int32)Game1.LocationGargoyle.X - 5; x < (Int32)Game1.LocationGargoyle.X + 10; x++)
                for (Int32 y = (Int32)Game1.LocationGargoyle.Y - 5; y < (Int32)Game1.LocationGargoyle.Y + 10; y++)
                    MapArray[x, y] = 0;

            DrawPath(Game1.LocationBHamHam, Game1.LocationDickinsongrad);
            DrawPath(Game1.LocationJRoberton, Game1.LocationBHamHam);
            DrawPath(Game1.LocationJRoberton, Game1.LocationDickinsongrad);
            DrawPath(Game1.LocationJRoberton, Game1.LocationGargoyle);
            DrawPath(Game1.LocationBHamHam, Game1.LocationGargoyle);
            DrawPath(Game1.LocationGargoyle, Game1.LocationDickinsongrad);

            AddTown((int)Game1.LocationBHamHam.X, (int)Game1.LocationBHamHam.Y);
            AddTown((int)Game1.LocationJRoberton.X, (int)Game1.LocationJRoberton.Y);
            AddTown((int)Game1.LocationDickinsongrad.X, (int)Game1.LocationDickinsongrad.Y);

            //Force add treeline/water for boundaries
            //Add North treeline
            for (Int32 i = 0; i < MapSize; i++)
                for (Int32 j = 0; j < 6; j++)
                    MapArray[i, j] = 5;
            //Add South treeline
            for (Int32 i = 0; i < MapSize; i++)
                for (Int32 j = MapSize - 6; j < MapSize; j++)
                    MapArray[i, j] = 2;
            //Add West side treeline
            for (Int32 i = 0; i < 6; i++)
                for (Int32 j = 0; j < MapSize; j++)
                    MapArray[i, j] = 5;
            //Add East treeline
            for (Int32 i = MapSize - 6; i < MapSize; i++)
                for (Int32 j = 0; j < MapSize; j++)
                    MapArray[i, j] = 5;
        }
        private void AddTown(int CenterX, int CenterY)
        {
            int StructureX, StructureY;

            StructureX = CenterX - 2 - Rand.Next() % 2;
            StructureY = CenterY - 2 - Rand.Next() % 2;
            MapArray[StructureX, StructureY] = 100; //first house top left
            MapArray[StructureX + 1, StructureY] = 101;
            MapArray[StructureX, StructureY + 1] = 102;
            MapArray[StructureX + 1, StructureY + 1] = 103;

            StructureX = CenterX + 2 + Rand.Next() % 2;
            StructureY = CenterY + 2 + Rand.Next() % 2;
            MapArray[StructureX, StructureY] = 100; //second house top left
            MapArray[StructureX + 1, StructureY] = 101;
            MapArray[StructureX, StructureY + 1] = 102;
            MapArray[StructureX + 1, StructureY + 1] = 103;

            StructureX = CenterX + 2 + Rand.Next() % 2;
            StructureY = CenterY - 2 - Rand.Next() % 2;
            MapArray[StructureX, StructureY] = 110; //second house top left
            MapArray[StructureX + 1, StructureY] = 111;
            MapArray[StructureX, StructureY + 1] = 112;
            MapArray[StructureX + 1, StructureY + 1] = 113;
        }
        private void DrawPath(Vector2 LeftTownPos, Vector2 RightTownPos)
        {
           
            Vector2 CurrentPos = Vector2.Zero; 
            CurrentPos.X = LeftTownPos.X + 5;
            if (LeftTownPos.Y < RightTownPos.Y)
                CurrentPos.Y = LeftTownPos.Y + 5;
            else
                CurrentPos.Y = LeftTownPos.Y - 5;
            Vector2 Slope = new Vector2(RightTownPos.X - CurrentPos.X, RightTownPos.Y - CurrentPos.Y);
            Slope.Normalize();
            Slope *= 2;

            while (CurrentPos.X < RightTownPos.X - 5 &&
                CurrentPos.Y < RightTownPos.Y - 5 ||
                CurrentPos.Y > RightTownPos.Y + 5)
            {
                for (int x = (int)CurrentPos.X - 2; x < (int)CurrentPos.X + 1; ++x)
                    for (int y = (int)CurrentPos.Y - 2; y < (int)CurrentPos.Y + 1; ++y)
                        if(x>0 && x<MapSize && y>0 && y<MapSize)
                            MapArray[x, y] = 6;
                CurrentPos += Slope;
            }
        }
    }
}