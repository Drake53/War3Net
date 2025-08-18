// ------------------------------------------------------------------------------
// <copyright file="DynamicTestDataAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DynamicTestDataAttribute : Attribute, ITestDataSource
    {
        private readonly MapFiles _mapFilesToOpen;

        public DynamicTestDataAttribute(MapFiles mapFilesToOpen)
        {
            _mapFilesToOpen = mapFilesToOpen;
        }

        public virtual IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            foreach (var data in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)data[0], out var map, _mapFilesToOpen) &&
                    map.Info is not null &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script) &&
                    AdditionalChecks(map))
                {
                    yield return data;
                }
            }
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data.Length == 1 && data[0] is string mapFilePath)
            {
                return GetFileDisplayName(mapFilePath);
            }

            return methodInfo.Name;
        }

        protected string GetFileDisplayName(string mapFilePath)
        {
            return Path.GetFileName(mapFilePath);
        }

        private bool AdditionalChecks(Map map)
        {
            if (_mapFilesToOpen.HasFlag(MapFiles.Sounds))
            {
                if (map.Sounds is null ||
                    map.Sounds.Sounds.Count == 0)
                {
                    return false;
                }
            }

            if (_mapFilesToOpen.HasFlag(MapFiles.Cameras))
            {
                if (map.Cameras is null ||
                    map.Cameras.Cameras.Count == 0)
                {
                    return false;
                }
            }

            if (_mapFilesToOpen.HasFlag(MapFiles.Regions))
            {
                if (map.Regions is null ||
                    map.Regions.Regions.Count == 0)
                {
                    return false;
                }
            }

            if (_mapFilesToOpen.HasFlag(MapFiles.Triggers))
            {
                if (map.Triggers is null ||
                    map.Triggers.FormatVersion == MapTriggersFormatVersion.v3 ||
                    map.Triggers.FormatVersion == MapTriggersFormatVersion.v6 ||
                    (map.Triggers.Variables.Count == 0 &&
                    !map.Triggers.TriggerItems.Any(triggerItem => triggerItem is not DeletedTriggerItem)))
                {
                    return false;
                }
            }

            if (_mapFilesToOpen.HasFlag(MapFiles.Units))
            {
                if (map.Units is null ||
                    map.Units.Units.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}