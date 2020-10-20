// ------------------------------------------------------------------------------
// <copyright file="Timer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Priority_Queue;

namespace War3Net.Runtime.Core
{
    public sealed class Timer : Agent
    {
        private const float MinimumTimeout = 0.0001f;

        private static readonly SimplePriorityQueue<Timer> _queue = new SimplePriorityQueue<Timer>();
        private static float _time;

        private static Timer? _expiredTimer;

        private float _elapsedTime;
        private float _timeout;
        private bool _periodic;
        private Action? _handlerFunc;

        public Timer()
        {
        }

        public static Timer? ExpiredTimer => _expiredTimer;

        public float ElapsedTime => _elapsedTime;

        public float RemainingTime => _timeout - _elapsedTime;

        public float Timeout => _timeout;

        public static void UpdateAllTimers(float deltaTime)
        {
            _time += deltaTime;
            foreach (var timer in _queue)
            {
                timer.UpdateTime(deltaTime);
            }

            while (_queue.TryFirst(out var timer) && _queue.TryGetPriority(timer, out var priority) && priority <= _time)
            {
                timer.OnExpire(priority);
            }
        }

        public static void DisposeAllTimers()
        {
            _queue.Clear();
        }

        public override void Dispose()
        {
            // TODO
        }

        public void Start(float timeout, bool periodic, Action? handlerFunc)
        {
            _elapsedTime = 0f;
            _timeout = timeout < MinimumTimeout ? MinimumTimeout : timeout;
            _periodic = periodic;
            _handlerFunc = handlerFunc;

            if (_queue.Contains(this))
            {
                _queue.UpdatePriority(this, _time + timeout);
            }
            else
            {
                _queue.Enqueue(this, _time + timeout);
            }
        }

        public void Pause()
        {
            if (_queue.Contains(this))
            {
                _queue.Remove(this);
            }
        }

        public void Resume()
        {
            if (!_queue.Contains(this))
            {
                _queue.Enqueue(this, _time + RemainingTime);
            }
        }

        private void OnExpire(float expireTime)
        {
            if (_periodic)
            {
                _queue.UpdatePriority(this, expireTime + _timeout);
                UpdateElapsedTime(_time - expireTime);
            }
            else
            {
                _queue.Dequeue();
            }

            _expiredTimer = this;
            _handlerFunc?.Invoke();
            _expiredTimer = null;
        }

        private void UpdateTime(float deltaTime)
        {
            UpdateElapsedTime(_elapsedTime + deltaTime);
        }

        private void UpdateElapsedTime(float newTime)
        {
            _elapsedTime = newTime > _timeout ? _timeout : newTime;
        }
    }
}