using System;
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

        private bool _elapsedEventEnabled;
        private long _lastTick;
        private bool _threadLoopEnabled = true;
        private long _nextInvoke;

        private const int MaxTimeSinceLastTickIntervalMultiplier = 4;

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
                while (_threadLoopEnabled && _stopwatch.ElapsedMilliseconds < _nextInvoke) ;

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
