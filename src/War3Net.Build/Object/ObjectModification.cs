// ------------------------------------------------------------------------------
// <copyright file="ObjectModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class ObjectModification : IEnumerable<ObjectDataModification>
    {
        private readonly Dictionary<long, ObjectDataModification> _modifications;

        private int _oldId;
        private int _newId;

        public ObjectModification(int oldId, int newId, params ObjectDataModification[] modifications)
        {
            _modifications = new Dictionary<long, ObjectDataModification>(modifications.Select(mod => new KeyValuePair<long, ObjectDataModification>(ToKey(mod.Id, mod.Level ?? 0), mod)));

            _oldId = oldId;
            _newId = newId;
        }

        public ObjectModification(int oldId, string newRawcode, params ObjectDataModification[] modifications)
            : this(oldId, newRawcode.FromRawcode(), modifications)
        {
        }

        public ObjectModification(string oldRawcode, int newId, params ObjectDataModification[] modifications)
            : this(oldRawcode.FromRawcode(), newId, modifications)
        {
        }

        public ObjectModification(string oldRawcode, string newRawcode, params ObjectDataModification[] modifications)
            : this(oldRawcode.FromRawcode(), newRawcode.FromRawcode(), modifications)
        {
        }

        private ObjectModification()
        {
            _modifications = new Dictionary<long, ObjectDataModification>();
        }

        public ObjectDataModification this[int key] => _modifications.TryGetValue(ToKey(key, 0), out var value) ? value : null;

        public ObjectDataModification this[int rawcode, int level] => _modifications.TryGetValue(ToKey(rawcode, level), out var value) ? value : null;

        public int OldId => _oldId;

        public int NewId => _newId;

        public int ModificationCount => _modifications.Count;

        public static ObjectModification Parse(Stream stream, bool readLevelData, bool leaveOpen = false)
        {
            var data = new ObjectModification();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                var oldId = reader.ReadInt32();
                var newId = reader.ReadInt32();
                var modificationCount = reader.ReadInt32();

                data._oldId = oldId;
                data._newId = newId;

                for (var i = 0; i < modificationCount; i++)
                {
                    var mod = ObjectDataModification.Parse(stream, oldId, newId, readLevelData, true);
                    data._modifications.Add(ToKey(mod.Id, mod.Level ?? 0), mod);
                }
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_oldId);
            writer.Write(_newId);

            writer.Write(_modifications.Count);
            foreach (var modification in _modifications)
            {
                modification.Value.WriteTo(writer);
            }
        }

        public ObjectModification AddModification(ObjectDataModification modification)
        {
            var key = ToKey(modification.Id, modification.Level ?? 0);
            if (_modifications.ContainsKey(key))
            {
                _modifications[key] = modification;
            }
            else
            {
                _modifications.Add(key, modification);
            }

            return this;
        }

        public IEnumerator<ObjectDataModification> GetEnumerator()
        {
            return ((IEnumerable<ObjectDataModification>)_modifications.Values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ObjectDataModification>)_modifications.Values).GetEnumerator();
        }

        private static long ToKey(int rawcode, int level)
        {
            return rawcode | ((long)level << 32);
        }
    }
}