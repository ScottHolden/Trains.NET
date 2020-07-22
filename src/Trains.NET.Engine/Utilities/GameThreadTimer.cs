﻿using System;
using System.Diagnostics;
using System.Threading;

namespace Trains.NET.Engine
{
    public class GameThreadTimer : ITimer
    {
        public double Interval { get; set; }
        public long TimeSinceLastTick { get; private set; }

        public event EventHandler? Elapsed;

        // Milliseconds before invocation that we should switch from a slow waiting timer, to fast yeilds
        //  On faster PC's, this can be set WAY down, but 12ms seems like a good balance. Below this the GitHub executions engine started to fail!
        private const int CoarseSleepThreshold = 12;

        private bool _elapsedEventEnabled = false;
        private long _lastTick = 0;
        private bool _threadLoopEnabled = true;
        private long _nextInvoke = 0;

        private const int MaxTimeSinceLastTickIntervalMultiplier = 2;

        private readonly Thread _gameThread;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public GameThreadTimer()
        {
            _gameThread = new Thread(ThreadLoop);
            _gameThread.Start();
        }

        private void ThreadLoop()
        {
            while(_threadLoopEnabled)
            {
                // If we are not enabled, or we have more than the CoarseSleepThreshold ms left until we need to invoke, sleep for 1 ms
                while (_threadLoopEnabled && 
                    (!_elapsedEventEnabled ||
                    _stopwatch.ElapsedMilliseconds + CoarseSleepThreshold < _nextInvoke))
                {
                    Thread.Sleep(1);
                }
                // If we are not yet ready to invoke, then be kind to other threads & let them have some pie, but don't sleep as we are close
                while (_threadLoopEnabled && _stopwatch.ElapsedMilliseconds < _nextInvoke)
                {
                    Thread.Sleep(0);
                }
                if (_threadLoopEnabled && _elapsedEventEnabled)
                {
                    long time = _stopwatch.ElapsedMilliseconds;

                    this.TimeSinceLastTick = time - _lastTick;

                    long maximumTimeSinceLastTick = (long)(MaxTimeSinceLastTickIntervalMultiplier * this.Interval);
                    if (this.TimeSinceLastTick > maximumTimeSinceLastTick)
                    {
                        this.TimeSinceLastTick = maximumTimeSinceLastTick;
                    }

                    _lastTick = time;
                    Elapsed?.Invoke(null, null);

                    _nextInvoke = time + (int)this.Interval;
                }
            }
        }

        public void Dispose()
        {
            _threadLoopEnabled = false;
        }

        public void Start() => _elapsedEventEnabled = true;

        public void Stop() => _elapsedEventEnabled = false;
    }
}
