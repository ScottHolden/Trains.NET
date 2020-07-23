using Trains.NET.Engine;

namespace Trains.NET.Rendering
{
    public interface ITrackRenderer
    {
        void FlushCache();
        void Render(ICanvas canvas, Track track);
    }
}
