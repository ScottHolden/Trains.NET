using System.Collections.Generic;
using Trains.NET.Engine;

namespace Trains.NET.Rendering
{
    public static class TerrainColourLookup
    {  
        public static Color DefaultColour => Colors.Green;

        private static readonly List<Color> s_heightOrderedColours = new List<Color> {
                Colors.DarkBlue,
                Colors.LightBlue,
                Colors.LightYellow,
                Colors.LightGreen,
                Colors.Green,
                Colors.DarkGreen,
                Colors.Gray,
                Colors.DirtyWhite
        };

        private static readonly float[] s_nonLinearHeightMap = new [] {
            0.0f,
            0.2f,   // 0.2 worth of Dark Blue
            0.25f,  // 0.05 worth of Light Blue
            0.3f,   // 0.05 worth of Light Yellow
            0.35f,  // 0.05 worth of Light Green
            0.7f,   // 0.35 worth of Green
            0.75f,  // 0.05 worth of Dark Green
            0.875f  // 0.125 worth of Gray
                    // 0.125 worth of DirtyWhite
        };

        public static int GetWaterLevel()
        {
            int heightPerTerrainType = (Terrain.MaxHeight / s_heightOrderedColours.Count) + 1;
            return s_heightOrderedColours.IndexOf(Colors.LightYellow) * heightPerTerrainType - 1;
        }

        public static Color GetTerrainColour2(Terrain terrain)
        {
            float currentHeight = (float)terrain.Height / Terrain.MaxHeight;

            for(int i=1; i< s_nonLinearHeightMap.Length; i++)
            {
                if(currentHeight < s_nonLinearHeightMap[i])
                {
                    return s_heightOrderedColours[i - 1];
                }
            }

            return s_heightOrderedColours[^1];
        }

        public static Color GetTerrainColour(Terrain terrain)
        {
            int heightPerTerrainType = (Terrain.MaxHeight / s_heightOrderedColours.Count) + 1;

            int colourLookup = terrain.Height / heightPerTerrainType;
            return s_heightOrderedColours[colourLookup];
        }
    }
}
