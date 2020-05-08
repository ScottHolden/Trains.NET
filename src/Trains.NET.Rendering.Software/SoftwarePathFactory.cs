namespace Trains.NET.Rendering.Software
{
    public class SoftwarePathFactory : IPathFactory
    {
        public IPath Create() => new SoftwarePath();
    }
}
