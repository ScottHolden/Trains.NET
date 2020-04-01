﻿using System;
using System.Collections.Generic;
using System.Timers;

namespace Trains.NET.Engine
{
    internal class GameBoard : IGameBoard, IDisposable
    {
        private const int GameLoopInterval = 16;
        private const int SpeedAdjustmentFactor = 10;

        private readonly Dictionary<(int, int), Track> _tracks = new Dictionary<(int, int), Track>();
        private readonly List<Train> _trains = new List<Train>();
        private readonly Timer _gameLoopTimer;

        public int Columns { get; set; }
        public int Rows { get; set; }

        public GameBoard()
        {
            _gameLoopTimer = new Timer(GameLoopInterval);
            _gameLoopTimer.Elapsed += GameLoopFire;
            _gameLoopTimer.Start();
        }

        public GameBoard(bool _)
        {
            _gameLoopTimer = new Timer(GameLoopInterval);
        }

        private void GameLoopFire(object sender, ElapsedEventArgs e)
        {
            _gameLoopTimer.Stop();

            float distance = 0.005f * SpeedAdjustmentFactor;

            GameLoopStep(distance);

            _gameLoopTimer.Start();
        }

        public void GameLoopStep(float initalDistance)
        {
            foreach (Train train in _trains)
            {
                float distance = initalDistance;

                while (distance > 0.0f)
                {
                    Track? track = GetTrackForTrain(train);
                    if (track != null)
                    {
                        distance = train.Move(distance, track);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public Track? GetTrackForTrain(Train train)
        {
            if (_tracks.TryGetValue((train.Column, train.Row), out Track track))
            {
                return track;
            }
            return null;
        }

        public void AddTrack(int column, int row)
        {
            if (_tracks.TryGetValue((column, row), out Track track))
            {
                track.SetBestTrackDirection(true);
            }
            else
            {
                track = new Track(this)
                {
                    Column = column,
                    Row = row
                };
                _tracks.Add((column, row), track);

                track.SetBestTrackDirection(false);
            }
        }

        public void RemoveTrack(int column, int row)
        {
            if (_tracks.TryGetValue((column, row), out Track track))
            {
                _tracks.Remove((column, row));
                track.RefreshNeighbors(true);
            }
        }

        public void AddTrain(int column, int row)
        {
            var train = new Train()
            {
                Column = column,
                Row = row
            };

            Track? track = GetTrackForTrain(train);
            if (track == null)
            {
                return;
            }

            _trains.Add(train);
        }

        public IEnumerable<(int, int, Track)> GetTracks()
        {
            foreach ((int col, int row, Track track) in _tracks)
            {
                yield return (col, row, track);
            }
        }

        public IEnumerable<Train> GetTrains()
        {
            foreach (Train train in _trains)
            {
                yield return train;
            }
        }

        public Track? GetTrackAt(int column, int row)
        {
            if (_tracks.TryGetValue((column, row), out Track track))
            {
                return track;
            }
            return null;
        }

        public void Dispose()
        {
            _gameLoopTimer.Dispose();
        }
    }
}
