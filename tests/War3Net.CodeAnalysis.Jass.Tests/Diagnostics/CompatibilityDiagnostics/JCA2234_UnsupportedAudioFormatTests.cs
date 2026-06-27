namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCompatibilityDiagnosticsTests
    {
        private const string NativesReferenceCode = @"
native PlayMusic takes string musicName returns nothing
native PlayMusicEx takes string musicName, integer frommsecs, integer fadeinmsecs returns nothing
native PlayThematicMusic takes string musicFileName returns nothing
native CreateSound takes string fileName, boolean looping, boolean is3D, boolean stopwhenoutofrange, integer fadeInRate, integer fadeOutRate, string eaxSetting returns handle
native GetSoundFileDuration takes string musicFileName returns integer
native I2S takes integer i returns string
";

        [TestMethod]
        [DynamicData(nameof(GetUnsupportedAudioFormatTests), DynamicDataSourceType.Method)]
        public void TestUnsupportedAudioFormatDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCompatibilityDiagnostics.UnsupportedAudioFormat.Id,
                NativesReferenceCode,
                markedCode);
        }

        private static IEnumerable<object?[]> GetUnsupportedAudioFormatTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call PlayMusic([|""war3mapImported\\BGM.flac""|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call PlayMusic([|""music\\theme.ogg""|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call PlayMusic([|""BGM"" + "".flac""|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 1
    call PlayMusic([|""BGM"" + I2S(i) + "".flac""|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call PlayMusic([|""war3mapImported\\BGM.FLAC""|])
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    constant string AUDIO_EXT = ""FLAC""
endglobals

function main takes nothing returns nothing
    call PlayMusic([|""BGM."" + AUDIO_EXT|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer d = GetSoundFileDuration([|""music.flac""|])
endfunction",
            };
        }
    }
}