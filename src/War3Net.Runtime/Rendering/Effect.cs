// ------------------------------------------------------------------------------
// <copyright file="Effect.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Rendering;
using War3Net.Runtime.Core;

namespace War3Net.Runtime.Rendering
{
    public sealed class Effect : Agent
    {
        private static readonly HashSet<Effect> _createdEffects = new HashSet<Effect>();

        private readonly ModelInstance _model;

        public Effect(string modelPath, float x, float y)
        {
            _model = new ModelInstance(modelPath);
            _model.SetTranslation(x, y, 0f);

            _createdEffects.Add(this);
        }

        public static void DisposeAllEffects()
        {
            foreach (var effect in _createdEffects)
            {
                effect._model.Destroy();
            }

            _createdEffects.Clear();
        }

        public void SetTimeScale(float timeScale)
        {
            _model.SetTimeScale(timeScale);
        }

        public void SetTime(float time)
        {
            _model.SetTime(time);
        }

        public void SetTranslation(float x, float y, float z)
        {
            _model.SetTranslation(x, y, z);
        }

        public void SetOrientation(float yaw, float pitch, float roll)
        {
            _model.SetOrientation(yaw, pitch, roll);
        }

        public void SetScale(float scale)
        {
            _model.SetScale(scale);
        }

        public override void Dispose()
        {
            _model.Destroy();
            _createdEffects.Remove(this);
        }
    }
}