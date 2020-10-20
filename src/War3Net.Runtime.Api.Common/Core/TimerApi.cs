// ------------------------------------------------------------------------------
// <copyright file="TimerApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class TimerApi
    {
        public static Timer CreateTimer() => new Timer();

        public static void DestroyTimer(Timer? whichTimer) => whichTimer.Dispose();

        public static void TimerStart(Timer? whichTimer, float timeout, bool periodic, Action? handlerFunc) => whichTimer.Start(timeout, periodic, handlerFunc);

        public static float TimerGetElapsed(Timer? whichTimer) => whichTimer.ElapsedTime;

        public static float TimerGetRemaining(Timer? whichTimer) => whichTimer.RemainingTime;

        public static float TimerGetTimeout(Timer? whichTimer) => whichTimer.Timeout;

        public static void PauseTimer(Timer? whichTimer) => whichTimer.Pause();

        public static void ResumeTimer(Timer? whichTimer) => whichTimer.Resume();

        public static Timer? GetExpiredTimer() => Timer.ExpiredTimer;
    }
}