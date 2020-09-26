// ------------------------------------------------------------------------------
// <copyright file="InternalCameraField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Runtime.Rendering
{
    internal sealed class InternalCameraField
    {
        private readonly bool _affectsPerspectiveMatrix;

        private float _previousValue;
        private float _currentValue;
        private float _targetValue;
        private float _remaining;

        public InternalCameraField(float value, bool affectsPerspectiveMatrix = false)
        {
            _affectsPerspectiveMatrix = affectsPerspectiveMatrix;

            _previousValue = value;
            _currentValue = value;
            _targetValue = value;
            _remaining = -1f;
        }

        public float Value => _previousValue;

        public bool AffectsPerspectiveMatrix => _affectsPerspectiveMatrix;

        public void SetTarget(float value, float duration)
        {
            if (duration > 0)
            {
                _targetValue = value;
                _remaining = duration;
            }
            else
            {
                _currentValue = value;
                _targetValue = value;
                _remaining = -1f;
            }
        }

        public bool Update(float deltaSeconds)
        {
            if (deltaSeconds >= _remaining)
            {
                _currentValue = _targetValue;
                _remaining = -1f;
                return false;
            }
            else
            {
                _currentValue += (_targetValue - _currentValue) * (deltaSeconds / _remaining);
                _remaining -= deltaSeconds;
                return true;
            }
        }

        internal float GetNewValue()
        {
            _previousValue = _currentValue;
            return _currentValue;
        }
    }
}