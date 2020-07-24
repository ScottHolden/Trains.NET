namespace Trains.NET.Rendering
{
    public class TrainParameters : ITrainParameters
    {
        private readonly ITrackParameters _trackParameters;

        public float RearHeight => 22 * _trackParameters.DrawScale;

        public float RearWidth => 10 * _trackParameters.DrawScale;

        public float HeadWidth => 25 * _trackParameters.DrawScale;

        public float HeadHeight => 16 * _trackParameters.DrawScale;

        public float OutlineWidth => 2 * _trackParameters.DrawScale;

        public TrainParameters(ITrackParameters trackParameters)
        {
            _trackParameters = trackParameters;
        }
    }
}
