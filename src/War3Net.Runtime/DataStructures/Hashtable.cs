// ------------------------------------------------------------------------------
// <copyright file="Hashtable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.DataStructures
{
    public sealed class Hashtable : Agent
    {
        private readonly Dictionary<int, Dictionary<int, int>> _savedInts;
        private readonly Dictionary<int, Dictionary<int, float>> _savedFloats;
        private readonly Dictionary<int, Dictionary<int, bool>> _savedBools;
        private readonly Dictionary<int, Dictionary<int, string>> _savedStrings;
        private readonly Dictionary<int, Dictionary<int, Handle>> _savedHandles;

        public Hashtable()
        {
            _savedInts = new Dictionary<int, Dictionary<int, int>>();
            _savedFloats = new Dictionary<int, Dictionary<int, float>>();
            _savedBools = new Dictionary<int, Dictionary<int, bool>>();
            _savedStrings = new Dictionary<int, Dictionary<int, string>>();
            _savedHandles = new Dictionary<int, Dictionary<int, Handle>>();
        }

        public void SaveInt(int parentKey, int childKey, int value)
        {
            if (!_savedInts.TryGetValue(parentKey, out var keys))
            {
                keys = new Dictionary<int, int>();
                _savedInts.Add(parentKey, keys);
            }

            keys[childKey] = value;
        }

        public void SaveFloat(int parentKey, int childKey, float value)
        {
            if (!_savedFloats.TryGetValue(parentKey, out var keys))
            {
                keys = new Dictionary<int, float>();
                _savedFloats.Add(parentKey, keys);
            }

            keys[childKey] = value;
        }

        public void SaveBool(int parentKey, int childKey, bool value)
        {
            if (!_savedBools.TryGetValue(parentKey, out var keys))
            {
                keys = new Dictionary<int, bool>();
                _savedBools.Add(parentKey, keys);
            }

            keys[childKey] = value;
        }

        public void SaveString(int parentKey, int childKey, string value)
        {
            if (!_savedStrings.TryGetValue(parentKey, out var keys))
            {
                keys = new Dictionary<int, string>();
                _savedStrings.Add(parentKey, keys);
            }

            keys[childKey] = value;
        }

        public void SaveHandle(int parentKey, int childKey, Handle value)
        {
            if (!_savedHandles.TryGetValue(parentKey, out var keys))
            {
                keys = new Dictionary<int, Handle>();
                _savedHandles.Add(parentKey, keys);
            }

            keys[childKey] = value;
        }

        public int LoadInt(int parentKey, int childKey)
        {
            return _savedInts.TryGetValue(parentKey, out var keys)
                ? keys.TryGetValue(childKey, out var value) ? value : default
                : default;
        }

        public float LoadFloat(int parentKey, int childKey)
        {
            return _savedFloats.TryGetValue(parentKey, out var keys)
                ? keys.TryGetValue(childKey, out var value) ? value : default
                : default;
        }

        public bool LoadBool(int parentKey, int childKey)
        {
            return _savedBools.TryGetValue(parentKey, out var keys)
                ? keys.TryGetValue(childKey, out var value) ? value : default
                : default;
        }

        public string LoadString(int parentKey, int childKey)
        {
            return _savedStrings.TryGetValue(parentKey, out var keys)
                ? keys.TryGetValue(childKey, out var value) ? value : default
                : default;
        }

        public Handle LoadHandle(int parentKey, int childKey)
        {
            return _savedHandles.TryGetValue(parentKey, out var keys)
                ? keys.TryGetValue(childKey, out var value) ? value : default
                : default;
        }

        public bool HaveSavedInt(int parentKey, int childKey)
        {
            return _savedInts.TryGetValue(parentKey, out var keys) && keys.ContainsKey(childKey);
        }

        public bool HaveSavedFloat(int parentKey, int childKey)
        {
            return _savedFloats.TryGetValue(parentKey, out var keys) && keys.ContainsKey(childKey);
        }

        public bool HaveSavedBool(int parentKey, int childKey)
        {
            return _savedBools.TryGetValue(parentKey, out var keys) && keys.ContainsKey(childKey);
        }

        public bool HaveSavedString(int parentKey, int childKey)
        {
            return _savedStrings.TryGetValue(parentKey, out var keys) && keys.ContainsKey(childKey);
        }

        public bool HaveSavedHandle(int parentKey, int childKey)
        {
            return _savedHandles.TryGetValue(parentKey, out var keys) && keys.ContainsKey(childKey);
        }

        public void ClearSavedInt(int parentKey, int childKey)
        {
            if (_savedInts.TryGetValue(parentKey, out var keys))
            {
                keys.Remove(childKey);
            }
        }

        public void ClearSavedFloat(int parentKey, int childKey)
        {
            if (_savedFloats.TryGetValue(parentKey, out var keys))
            {
                keys.Remove(childKey);
            }
        }

        public void ClearSavedBool(int parentKey, int childKey)
        {
            if (_savedBools.TryGetValue(parentKey, out var keys))
            {
                keys.Remove(childKey);
            }
        }

        public void ClearSavedString(int parentKey, int childKey)
        {
            if (_savedStrings.TryGetValue(parentKey, out var keys))
            {
                keys.Remove(childKey);
            }
        }

        public void ClearSavedHandle(int parentKey, int childKey)
        {
            if (_savedHandles.TryGetValue(parentKey, out var keys))
            {
                keys.Remove(childKey);
            }
        }

        public void ClearParent(int parentKey)
        {
            _savedInts.Remove(parentKey);
            _savedFloats.Remove(parentKey);
            _savedBools.Remove(parentKey);
            _savedStrings.Remove(parentKey);
            _savedHandles.Remove(parentKey);
        }

        public void Clear()
        {
            _savedInts.Clear();
            _savedFloats.Clear();
            _savedBools.Clear();
            _savedStrings.Clear();
            _savedHandles.Clear();
        }

        public override void Dispose()
        {
            Clear();
        }
    }
}