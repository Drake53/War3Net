// ------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed class Sound
    {
        private string _variableName;
        private string _soundName; // 'SoundName' in .slk files? (reforged only, can be different from name in filepath, eg DeathHumanLargeBuilding = BuildingDeathLargeHuman.wav).
        private string _filePath;
        private string _eaxSetting; // TODO: enum?

        private SoundFlags _flags;

        private int _fadeInRate;
        private int _fadeOutRate;
        private int _volume;
        private float _pitch;

        private float _pitchVariance; // used when RANDOMPITCH flag is set
        private int _priority;

        private SoundChannel _channel;
        private float _minDistance;
        private float _maxDistance;
        private float _distanceCutoff;

        private float _coneInside;
        private float _coneOutside;
        private int _coneOutsideVolume;
        private Vector3 _coneOrientation;

        public string Name
        {
            get => _variableName;
            set => _variableName = value;
        }

        public string SoundName
        {
            get => _soundName;
            set => _soundName = value;
        }

        public string FilePath
        {
            get => _filePath;
            set => _filePath = value;
        }

        public string EaxSetting
        {
            get => _eaxSetting;
            set => _eaxSetting = value;
        }

        public SoundFlags Flags
        {
            get => _flags;
            set => _flags = value;
        }

        public int FadeInRate
        {
            get => _fadeInRate;
            set => _fadeInRate = value;
        }

        public int FadeOutRate
        {
            get => _fadeOutRate;
            set => _fadeOutRate = value;
        }

        public int Volume
        {
            get => _volume;
            set => _volume = value;
        }

        public float Pitch
        {
            get => _pitch;
            set => _pitch = value;
        }

        public SoundChannel Channel
        {
            get => _channel;
            set => _channel = value;
        }

        public float MinDistance
        {
            get => _minDistance;
            set => _minDistance = value;
        }

        public float MaxDistance
        {
            get => _maxDistance;
            set => _maxDistance = value;
        }

        public float DistanceCutoff
        {
            get => _distanceCutoff;
            set => _distanceCutoff = value;
        }

        public float ConeAngleInside
        {
            get => _coneInside;
            set => _coneInside = value;
        }

        public float ConeAngleOutside
        {
            get => _coneOutside;
            set => _coneOutside = value;
        }

        public int ConeOutsideVolume
        {
            get => _coneOutsideVolume;
            set => _coneOutsideVolume = value;
        }

        public Vector3 ConeOrientation
        {
            get => _coneOrientation;
            set => _coneOrientation = value;
        }

        public static Sound Parse(Stream stream, bool leaveOpen)
        {
            return Parse(stream, MapSoundsFormatVersion.Normal, leaveOpen);
        }

        public static Sound Parse(Stream stream, MapSoundsFormatVersion formatVersion, bool leaveOpen)
        {
            var sound = new Sound();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                sound._variableName = reader.ReadChars();
                sound._filePath = reader.ReadChars();
                sound._eaxSetting = reader.ReadChars();
                sound._flags = (SoundFlags)reader.ReadInt32();

                sound._fadeInRate = reader.ReadInt32();
                sound._fadeOutRate = reader.ReadInt32();

                sound._volume = reader.ReadInt32();
                sound._pitch = reader.ReadSingle();

                sound._pitchVariance = reader.ReadSingle();
                sound._priority = reader.ReadInt32();

                sound._channel = (SoundChannel)reader.ReadInt32();
                sound._minDistance = reader.ReadSingle();
                sound._maxDistance = reader.ReadSingle();
                sound._distanceCutoff = reader.ReadSingle();

                sound._coneInside = reader.ReadSingle();
                sound._coneOutside = reader.ReadSingle();
                sound._coneOutsideVolume = reader.ReadInt32();
                sound._coneOrientation = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                if (formatVersion >= MapSoundsFormatVersion.Reforged)
                {
                    var repeatVariableName = reader.ReadChars();
                    sound._soundName = reader.ReadChars();
                    var repeatSoundPath = reader.ReadChars();

                    if (repeatVariableName != sound.Name || repeatSoundPath != sound.FilePath)
                    {
                        throw new InvalidDataException();
                    }

                    var unk2 = reader.ReadInt32();
                    var unk3 = reader.ReadByte();
                    var unk4 = reader.ReadInt32();
                    var unk5 = reader.ReadByte();
                    var unk6 = reader.ReadInt32();
                    var unk7 = reader.ReadInt32();
                }
            }

            return sound;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteString(_variableName);
            writer.WriteString(_filePath);
            writer.WriteString(_eaxSetting);
            writer.Write((int)_flags);

            writer.Write(_fadeInRate);
            writer.Write(_fadeOutRate);
            writer.Write(_volume);
            writer.Write(_pitch);

            writer.Write(_pitchVariance);
            writer.Write(_priority);

            writer.Write((int)_channel);
            writer.Write(_minDistance);
            writer.Write(_maxDistance);
            writer.Write(_distanceCutoff);

            writer.Write(_coneInside);
            writer.Write(_coneOutside);
            writer.Write(_coneOutsideVolume);
            writer.Write(_coneOrientation.X);
            writer.Write(_coneOrientation.Y);
            writer.Write(_coneOrientation.Z);

            if (_soundName != null)
            {
                // Write reforged sound data.
                writer.WriteString(_variableName);
                writer.WriteString(_soundName);
                writer.WriteString(_filePath);

                writer.Write(-1);
                writer.Write((byte)0);
                writer.Write(-1);
                writer.Write((byte)0);
                writer.Write(0);
                writer.Write(0);
            }
        }
    }
}