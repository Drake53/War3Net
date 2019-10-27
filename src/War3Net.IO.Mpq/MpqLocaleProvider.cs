// ------------------------------------------------------------------------------
// <copyright file="MpqLocaleProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.IO.Mpq
{
    public static class MpqLocaleProvider
    {
        public const string ChineseLocaleDirectory = "zh-TW";
        public const string CzechLocaleDirectory = "cs";
        public const string GermanLocaleDirectory = "de";
        public const string EnglishLocaleDirectory = "en-US";
        public const string SpanishLocaleDirectory = "es";
        public const string FrenchLocaleDirectory = "fr";
        public const string ItalianLocaleDirectory = "it";
        public const string JapaneseLocaleDirectory = "ja";
        public const string KoreanLocaleDirectory = "ko";
        public const string PolishLocaleDirectory = "pl";
        public const string PortugueseLocaleDirectory = "pt";
        public const string RussianLocaleDirectory = "ru";
        public const string BritishLocaleDirectory = "en-GB";

        private static readonly Lazy<Dictionary<string, MpqLocale>> _localeLookupDictionary = new Lazy<Dictionary<string, MpqLocale>>(GetMpqLocaleStrings);

        public static MpqLocale GetPathLocale(string path, out string trimmedPath)
        {
            if (path.Contains('\\'))
            {
                var localizationDirectory = path.Split('\\')[0];
                if (_localeLookupDictionary.Value.TryGetValue(localizationDirectory, out var locale))
                {
                    trimmedPath = path.Substring(localizationDirectory.Length + 1);
                    return locale;
                }
            }

            trimmedPath = path;
            return MpqLocale.Neutral;
        }

        private static Dictionary<string, MpqLocale> GetMpqLocaleStrings()
        {
            var result = new Dictionary<string, MpqLocale>();

            result.Add(ChineseLocaleDirectory, MpqLocale.Chinese);
            result.Add(CzechLocaleDirectory, MpqLocale.Czech);
            result.Add(GermanLocaleDirectory, MpqLocale.German);
            result.Add(EnglishLocaleDirectory, MpqLocale.English);
            result.Add(SpanishLocaleDirectory, MpqLocale.Spanish);
            result.Add(FrenchLocaleDirectory, MpqLocale.French);
            result.Add(ItalianLocaleDirectory, MpqLocale.Italian);
            result.Add(JapaneseLocaleDirectory, MpqLocale.Japanese);
            result.Add(KoreanLocaleDirectory, MpqLocale.Korean);
            result.Add(PolishLocaleDirectory, MpqLocale.Polish);
            result.Add(PortugueseLocaleDirectory, MpqLocale.Portuguese);
            result.Add(RussianLocaleDirectory, MpqLocale.Russian);
            result.Add(BritishLocaleDirectory, MpqLocale.EnglishUK);

            return result;
        }
    }
}