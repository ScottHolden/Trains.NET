using System;

namespace Trains.NET.Engine
{
    public static class TrackDirectionExtensions
    {
        public static bool IsThreeWay(this TrackDirection direction) => direction == TrackDirection.RightUpDown ||
                                                                direction == TrackDirection.LeftRightDown ||
                                                                direction == TrackDirection.LeftUpDown ||
                                                                direction == TrackDirection.LeftRightUp;

        public static float TrackRotationAngle(this TrackDirection direction) => direction switch
        {
            TrackDirection.LeftUp => 0,
            TrackDirection.LeftRightUp => 0,

            TrackDirection.RightUp => 90,
            TrackDirection.RightUpDown => 90,

            TrackDirection.RightDown => 180,
            TrackDirection.LeftRightDown => 180,

            TrackDirection.LeftDown => 270,
            TrackDirection.LeftUpDown => 270,

            _ => 0
        };

        public static TrackDirection[] SeperateSubtracks(this TrackDirection direction) => direction switch
        {
            
            TrackDirection.Horizontal => new[] { TrackDirection.Horizontal },
            TrackDirection.Vertical => new[] { TrackDirection.Vertical },
            TrackDirection.LeftUp => new[] { TrackDirection.LeftUp },
            TrackDirection.RightUp => new[] { TrackDirection.RightUp },
            TrackDirection.RightDown => new[] { TrackDirection.RightDown },
            TrackDirection.LeftDown => new[] { TrackDirection.LeftDown },
            TrackDirection.RightUpDown => new[] { TrackDirection.RightUp, TrackDirection.RightDown },
            TrackDirection.LeftRightDown => new[] { TrackDirection.RightDown, TrackDirection.LeftDown },
            TrackDirection.LeftUpDown => new[] { TrackDirection.LeftDown, TrackDirection.LeftUp },
            TrackDirection.LeftRightUp => new[] { TrackDirection.LeftUp, TrackDirection.RightUp },
            TrackDirection.HorizontalLeftUp => new[] { TrackDirection.Horizontal, TrackDirection.LeftUp },
            TrackDirection.HorizontalRightUp => new[] { TrackDirection.Horizontal, TrackDirection.RightUp },
            TrackDirection.HorizontalLeftDown => new[] { TrackDirection.Horizontal, TrackDirection.LeftDown },
            TrackDirection.HorizontalRightDown => new[] { TrackDirection.Horizontal, TrackDirection.RightDown },
            TrackDirection.VerticalLeftUp => new[] { TrackDirection.Vertical, TrackDirection.LeftUp },
            TrackDirection.VerticalRightUp => new[] { TrackDirection.Vertical, TrackDirection.RightUp },
            TrackDirection.VerticalLeftDown => new[] { TrackDirection.Vertical, TrackDirection.LeftDown },
            TrackDirection.VerticalRightDown => new[] { TrackDirection.Vertical, TrackDirection.RightDown },
            TrackDirection.Cross => new[] { TrackDirection.Horizontal, TrackDirection.Vertical },

            TrackDirection.Undefined => Array.Empty<TrackDirection>(),
            _ => Array.Empty<TrackDirection>()
        };
    }
}
