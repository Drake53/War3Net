// ------------------------------------------------------------------------------
// <copyright file="MapFileHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;

using War3Net.Build.Info;

namespace War3Net.Build.Common
{
    /// <summary>
    /// This class grants access to static members that should be in every MapFile class.
    /// </summary>
    internal sealed class MapFileHandler<TMapFile>
    {
        private const string ParseMethodName = nameof(MapInfo.Parse);
        private const string SerializeMethodName = nameof(MapInfo.Serialize);
        private const string DefaultPropertyName = nameof(MapInfo.Default);
        private const string FileNameConstantName = nameof(MapInfo.FileName);
        private const string IsRequiredPropertyName = nameof(MapInfo.IsRequired);

        private readonly MethodInfo _parseMethod;
        private readonly MethodInfo _serializeMethod;
        private readonly MethodInfo _defaultProperty;

        private readonly string _fileName;
        private readonly bool _isRequired;

        public MapFileHandler()
        {
            var mapFileType = typeof(TMapFile);
            var e = new InvalidCastException($"The type '{mapFileType.Name}' is not a MapFile class.");

            _parseMethod = mapFileType.GetMethod(ParseMethodName, new[] { typeof(Stream), typeof(bool) }) ?? throw e;
            _serializeMethod = mapFileType.GetMethod(SerializeMethodName, new[] { mapFileType, typeof(Stream), typeof(bool) }) ?? throw e;
            _defaultProperty = mapFileType.GetProperty(DefaultPropertyName, mapFileType)?.GetMethod ?? throw e;
            _fileName = (string)mapFileType.GetField(FileNameConstantName)?.GetValue(null) ?? throw e;
            _isRequired = (bool)(mapFileType.GetProperty(IsRequiredPropertyName, typeof(bool))?.GetMethod.Invoke(null, null) ?? throw e);
        }

        public string FileName => _fileName;

        public bool IsRequired => _isRequired;

        public TMapFile GetDefault()
        {
            return (TMapFile)_defaultProperty.Invoke(null, null);
        }

        public TMapFile Parse(Stream stream, bool leaveOpen = false)
        {
            return (TMapFile)_parseMethod.Invoke(null, new object[] { stream, leaveOpen });
        }

        public void Serialize(TMapFile mapFile, Stream stream, bool leaveOpen = false)
        {
            _serializeMethod.Invoke(null, new object[] { mapFile, stream, leaveOpen });
        }
    }
}