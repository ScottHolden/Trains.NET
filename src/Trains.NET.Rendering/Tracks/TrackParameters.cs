namespace Trains.NET.Rendering
{
    internal class TrackParameters : ITrackParameters
    {
        public int CellSize { get; }

        public int NumPlanks => 3;
        public int NumCornerPlanks => this.NumPlanks + 1;

        public float PlankLength => 26 * this.DrawScale;
        public float PlankWidth => 4.0f * this.DrawScale;
        public int TrackWidth => (int)(12 * this.DrawScale);
        public float RailWidth => 4f * this.DrawScale;
        public float RailTopWidth => 2.75f * this.DrawScale;

        public float DrawScale => this.CellSize / 40.0f;

        public TrackParameters()
        {
            this.CellSize = 200;
        }
    }
}
