// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerDefinition : TriggerItem
    {
        private readonly List<TriggerFunction> _functions;

        private string _name;
        private string _description;
        private bool _isComment;
        private int _id;
        private bool _isEnabled;
        private bool _isCustomTextTrigger;
        private bool _isInitiallyOn;
        private bool _runOnMapInit;
        private int _parentId;

        private TriggerDefinition(TriggerItemType type)
            : base(type)
        {
            _functions = new List<TriggerFunction>();
        }

        public override string Name
        {
            get => _name;
            set => _name = value;
        }

        public override int Id
        {
            get => _id;
            set => _id = value;
        }

        public override int ParentId
        {
            get => _parentId;
            set => _parentId = value;
        }

        public static TriggerDefinition Parse(Stream stream, TriggerData triggerData, MapTriggersFormatVersion formatVersion, TriggerItemType? type, bool leaveOpen)
        {
            var useNewFormat = type != null;
            var trigger = new TriggerDefinition(type ?? TriggerItemType.Gui);
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                trigger._name = reader.ReadChars();
                trigger._description = reader.ReadChars();
                if (formatVersion >= MapTriggersFormatVersion.Tft)
                {
                    trigger._isComment = reader.ReadBool();
                }

                if (useNewFormat)
                {
                    trigger._id = reader.ReadInt32();
                }

                trigger._isEnabled = reader.ReadBool();
                trigger._isCustomTextTrigger = reader.ReadBool();
                trigger._isInitiallyOn = !reader.ReadBool();
                trigger._runOnMapInit = reader.ReadBool();
                trigger._parentId = reader.ReadInt32();

                var guiFunctionCount = reader.ReadInt32();
                if (trigger._isCustomTextTrigger && guiFunctionCount > 0)
                {
                    throw new InvalidDataException($"Custom text trigger should not have any GUI functions.");
                }

                for (var j = 0; j < guiFunctionCount; j++)
                {
                    trigger._functions.Add(TriggerFunction.Parse(stream, triggerData, formatVersion, false, true));
                }
            }

            return trigger;
        }

        public override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.WriteString(_name);
            writer.WriteString(_description);
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.Write(_isComment ? 1 : 0);
            }

            if (useNewFormat)
            {
                writer.Write(_id);
            }

            writer.Write(_isEnabled ? 1 : 0);
            writer.Write(_isCustomTextTrigger ? 1 : 0);
            writer.Write(_isInitiallyOn ? 0 : 1);
            writer.Write(_runOnMapInit ? 1 : 0);
            writer.Write(_parentId);

            writer.Write(_functions.Count);
            foreach (var function in _functions)
            {
                function.WriteTo(writer, formatVersion);
            }
        }
    }
}