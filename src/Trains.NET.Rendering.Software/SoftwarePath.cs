using System.Collections.Generic;
using System.Linq;

namespace Trains.NET.Rendering.Software
{
    public class SoftwarePath : IPath
    {
        private readonly List<IPathElement> _pathElements;

        public SoftwarePath()
        {
            _pathElements = new List<IPathElement>();
        }

        public void ArcTo(float radiusX, float radiusY, int xAxisRotate, PathArcSize arcSize, PathDirection direction, float x, int y) =>
            _pathElements.Add(new ArcToPathElement(x, y, radiusX, direction == PathDirection.CounterClockwise));

        public void LineTo(float x, float y) => _pathElements.Add(new LineToPathElement(x, y));

        public void MoveTo(float x, float y) => _pathElements.Add(new MoveToPathElement(x, y));

        internal IEnumerable<IPathElement> GetPathElements() => _pathElements.AsEnumerable();
    }
}
