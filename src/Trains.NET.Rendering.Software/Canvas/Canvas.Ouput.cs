using System.IO;

namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        public void DrawArgb32(Stream output)
        {
            for (int i = 0; i < _canvas.Length; i++)
            {
                output.WriteByte(_canvas[i].Alpha);
                output.WriteByte(_canvas[i].Red);
                output.WriteByte(_canvas[i].Green);
                output.WriteByte(_canvas[i].Blue);
            }
        }
    }
}
