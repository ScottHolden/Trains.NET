﻿
namespace Trains.NET.Engine
{
    public class Train
    {
        public int Column { get; internal set; }
        public int Row { get; internal set; }
        public float Angle { get; internal set; }
        public float RelativeLeft { get; internal set; } = 0.5f;
        public float RelativeTop { get; internal set; } = 0.5f;

        internal float Move(float distance, Track track)
        {
            int newColumn = this.Column;
            int newRow = this.Row;

            TrainPosition position = new TrainPosition(this.RelativeLeft, this.RelativeTop, this.Angle, distance);

            track.Move(position);

            if (position.RelativeLeft < 0.0f)
            {
                newColumn--;
                position.RelativeLeft = 1.0f;
            } 
            else if (position.RelativeLeft > 1.0f)
            {
                newColumn++;
                position.RelativeLeft = 0.0f;
            }
            if (position.RelativeTop < 0.0f)
            {
                newRow--;
                position.RelativeTop = 1.0f;
            }
            else if (position.RelativeTop > 1.0f)
            {
                newRow++;
                position.RelativeTop = 0.0f;
            }

            // Wrap this in detection logic
            this.Column = newColumn;
            this.Row = newRow;
            this.RelativeLeft = position.RelativeLeft;
            this.RelativeTop = position.RelativeTop;
            this.Angle = position.Angle;
            return position.Distance;
        }
    }
}
