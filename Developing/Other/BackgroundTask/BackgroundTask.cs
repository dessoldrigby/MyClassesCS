﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Developing.GeneralExtensions;

namespace Developing.Other
{
    internal class BackgroundTask : IDisposable
    {
        private protected Task? _timerTask;
        private protected readonly PeriodicTimer _timer;
        private protected readonly CancellationTokenSource _cts;

        public BackgroundTask(double interval) : this(TimeSpan.FromMilliseconds(interval))
        {

        }
        public BackgroundTask(double interval, CancellationTokenSource cts) : this(TimeSpan.FromMilliseconds(interval), cts)
        {

        }
        public BackgroundTask(TimeSpan interval) : this(interval, new CancellationTokenSource())
        {
            
        }
        public BackgroundTask(TimeSpan interval, CancellationTokenSource cts)
        {
            _timer = new PeriodicTimer(interval);
            _cts = cts;
        }

        public virtual void Start(Action func)
        {
            _timerTask = DoWorkAsync(func);
        }

        public virtual void Stop()
        {
            _cts.Cancel();
        }

        protected virtual async Task DoWorkAsync(Action func) 
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    func();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public async Task StopAsync()
        {
            if (_timerTask is null) return;
            
            await _timerTask;
            _cts.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if(_cts.Token.CanBeCanceled)
                _cts.Cancel();

            _timerTask?.Dispose();
            _timer?.Dispose();
            //_cts?.Dispose();
        }
    }
}
