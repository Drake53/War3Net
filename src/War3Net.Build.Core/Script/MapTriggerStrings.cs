// ------------------------------------------------------------------------------
// <copyright file="MapTriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace War3Net.Build.Script
{
    public sealed class MapTriggerStrings
    {
        public const string FileName = "war3map.wts";

        private static readonly List<Encoding> _possibleEncodings = GetPossibleEncodings().ToList();

        private readonly List<MapTriggerString> _strings;

        private Encoding _encoding;

        private MapTriggerStrings()
        {
            _strings = new List<MapTriggerString>();
        }

        public static bool IsRequired => false;

        public MapTriggerString this[uint key] => _strings.First(s => s.Key == key);

        public static MapTriggerStrings Parse(Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            try
            {
                var triggerStrings = new MapTriggerStrings();

                var position = stream.Position;
                var decoderExceptions = new List<DecoderFallbackException>();

                var encodings = new List<Encoding>();
                if (encoding is not null)
                {
                    encodings.Add(encoding);
                }

                encodings.AddRange(_possibleEncodings);
                foreach (var encodingToTry in encodings)
                {
                    try
                    {
                        using (var reader = new StreamReader(stream, encodingToTry, leaveOpen: true))
                        {
                            while (!reader.EndOfStream)
                            {
                                triggerStrings._strings.Add(MapTriggerString.ReadFrom(reader));
                            }

                            triggerStrings._encoding = reader.CurrentEncoding;
                        }

                        return triggerStrings;
                    }
                    catch (DecoderFallbackException e)
                    {
                        decoderExceptions.Add(e);
                        triggerStrings._strings.Clear();
                        stream.Position = position;
                    }
                }

                throw new AggregateException("Unknown encoding.", decoderExceptions);
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The '{FileName}' file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The '{FileName}' file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!leaveOpen)
                {
                    stream.Dispose();
                }
            }
        }

        public static void Serialize(MapTriggerStrings mapTriggerStrings, Stream stream, bool leaveOpen = false)
        {
            mapTriggerStrings.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new StreamWriter(stream, _encoding, leaveOpen: leaveOpen))
            {
                foreach (var triggerString in _strings)
                {
                    triggerString.WriteTo(writer);
                }
            }
        }

        private static IEnumerable<Encoding> GetPossibleEncodings()
        {
            yield return new UTF8Encoding(false, true);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var eucKR = CodePagesEncodingProvider.Instance.GetEncoding(51949, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
            if (eucKR is not null)
            {
                yield return eucKR;
            }
        }
    }
}