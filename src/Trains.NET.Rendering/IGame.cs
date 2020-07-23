
namespace Trains.NET.Rendering
{
    public interface IGame
    {
        void AdjustViewPortIfNecessary();
        void FlushCache();
        void Render(ICanvas canvas);
        void SetSize(int width, int height);
    }
}
