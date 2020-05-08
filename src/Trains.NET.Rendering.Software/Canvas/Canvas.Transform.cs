using System;
using System.Collections.Generic;

namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        private Transform _currentTransform;
        private readonly Stack<Transform> _transformStack;

        public void Rotate(float radians) => _currentTransform = _currentTransform.Rotate(radians);
        public void Translate(float x, float y) => _currentTransform = _currentTransform.Translate(x, y);
        public void Save() => _transformStack.Push(_currentTransform.Clone());
        public void Restore()
        {
            if (_transformStack.Count < 1)
            {
                throw new Exception("Nothing saved");
            }
            _currentTransform = _transformStack.Pop();
        }
    }
}
