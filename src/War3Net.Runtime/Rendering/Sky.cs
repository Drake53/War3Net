// ------------------------------------------------------------------------------
// <copyright file="Sky.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Rendering;

namespace War3Net.Runtime.Rendering
{
    public static class Sky
    {
        private static ModelInstance? _model;

        public static void SetModel(string? skyModelFile)
        {
            Destroy();

            if (!string.IsNullOrWhiteSpace(skyModelFile))
            {
                _model = new ModelInstance(skyModelFile, ModelType.Sky);
                Update();
            }
        }

        public static void Update()
        {
            var camera = Camera.LocalCamera;
            _model?.SetTranslation(camera.EyeX, camera.EyeY, camera.EyeZ);
        }

        public static void Destroy()
        {
            _model?.Destroy();
            _model = null;
        }
    }
}