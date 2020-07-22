﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Trains.NET.Engine.Tracks
{
    public interface ITrackLayout : IEnumerable<Track>
    {
        event EventHandler TracksChanged;

        bool TryGet(int column, int row, [NotNullWhen(true)] out Track? track);
        void SetTracks(IEnumerable<Track> tracks);
        void Clear();
        void AddTrack(int column, int row);
        void RemoveTrack(int column, int row);
        void ToggleTrack(int column, int row);
    }
}
