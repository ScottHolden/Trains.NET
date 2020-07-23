namespace Trains.NET.Rendering
{
    public interface ITrackParameters
    {
        int CellSize { get; set; }
        float PlankLength { get; }
        float PlankWidth { get; }
        int NumPlanks { get;}
        int NumCornerPlanks { get; }
        int TrackWidth { get; }
        float RailWidth { get; }
        float RailTopWidth { get; }
        float DrawScale { get; }
    }
}
