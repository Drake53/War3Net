// ------------------------------------------------------------------------------
// <copyright file="InternalCameraField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Runtime.Common.Rendering
{
    internal sealed class InternalCameraField
    {
        private readonly bool _affectsPerspectiveMatrix;

        private float _currentValue;
        private float _targetValue;
        private float _remaining;

        public InternalCameraField(float value, bool affectsPerspectiveMatrix = false)
        {
            _affectsPerspectiveMatrix = affectsPerspectiveMatrix;

            _currentValue = value;
            _targetValue = value;
            _remaining = -1f;
        }

        public float Value => _currentValue;

        public bool AffectsPerspectiveMatrix => _affectsPerspectiveMatrix;

        public void SetTarget(float value, float duration)
        {
            if (_remaining == 0f)
            {
                _currentValue = _targetValue;
            }

            _targetValue = value;
            _remaining = duration >= 0 ? duration : 0;
        }

        public bool Update(float deltaSeconds)
        {
            if (deltaSeconds >= _remaining)
            {
                _currentValue = _targetValue;
                _remaining = -1f;
                return true;
            }
            else
            {
                _currentValue += (_targetValue - _currentValue) * (deltaSeconds / _remaining);
                _remaining -= deltaSeconds;
                return false;
            }
        }
    }
}