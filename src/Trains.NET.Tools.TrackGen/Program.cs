using System;
using System.Numerics;

namespace Trains.NET.Tools.TrackGen
{
    class Program
    {
        static void Main(string[] args)
        {
            int w = 100;
            int h = 20;
            SimpleTrainsPerlin p = new SimpleTrainsPerlin(w, h, 1);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                    Console.Write(PtoChar(p[x, y]));
                Console.WriteLine();
            }
        }
        static char PtoChar(float p) => " .:-=+*#%@@"[(int)(Math.Min(10,Math.Max(0,(0.2+p) * 4)))];
    }

    class SimpleTrainsPerlin
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Vector2[] _data;
        public SimpleTrainsPerlin(int width, int height, int seed)
        {
            _width = width;
            _height = height;
            _data = GenerateBaseData((_width + 1) * (_height + 1), seed);
        }
        private static Vector2[] GenerateBaseData(int size, int seed)
        {
            var data = new Vector2[size];
            var r = new Random(seed);
            for (int i = 0; i < size; i++)
                data[i] = new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1));
            return data;
        }
        public float this[int x, int y] => PerlinAt(x,y);
        public float PerlinAt(int xA, int yA)
        {
            if (xA >= _width || xA < 0) throw new ArgumentOutOfRangeException(nameof(xA));
            if (yA >= _height || yA < 0) throw new ArgumentOutOfRangeException(nameof(yA));

            float x = xA + 0.0f;
            float y = yA + 0.0f;

            int x0 = (int)x;
            int x1 = x0 + 1;
            int y0 = (int)y;
            int y1 = y0 + 1;

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - (float)x0;
            float sy = y - (float)y0;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;

            n0 = DotGridGradient(x0, y0, x, y);
            n1 = DotGridGradient(x1, y0, x, y);
            ix0 = Lerp(n0, n1, sx);

            n0 = DotGridGradient(x0, y1, x, y);
            n1 = DotGridGradient(x1, y1, x, y);
            ix1 = Lerp(n0, n1, sx);

            value = Lerp(ix0, ix1, sy);
            return value;

        }
        private static float Lerp(float start, float end, float pos) => (1.0f - pos) * start + pos * end;

        private float DotGridGradient(int ix, int iy, float x, float y)
        {
            // Compute the distance vector
            float dx = x - (float)ix;
            float dy = y - (float)iy;

            int index = iy * _width + ix;

            // Compute the dot-product
            return (dx * _data[index].X + dy * _data[index].Y);
        }
    }
}
