namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCodeQualityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetBoolExprLeakTests), DynamicDataSourceType.Method)]
        public void TestBoolExprLeakDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCodeQualityDiagnostics.HandleLeak.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetBoolExprLeakTests()
        {
            yield return new object[]
            {
                @"
type boolexpr extends handle
native And takes boolexpr operandA, boolexpr operandB returns boolexpr
native DestroyBoolExpr takes boolexpr whichBoolExpr returns nothing
native Condition takes code func returns boolexpr

function myFilter takes nothing returns boolean
    return true
endfunction

function main takes nothing returns nothing
    local boolexpr [|be|] = Condition(function myFilter)
endfunction",
            };

            yield return new object[]
            {
                @"
type conditionfunc extends handle
native Condition takes code func returns conditionfunc
native DestroyCondition takes conditionfunc c returns nothing

function myFilter takes nothing returns boolean
    return true
endfunction

function main takes nothing returns nothing
    local conditionfunc [|c|] = Condition(function myFilter)
endfunction",
            };

            yield return new object[]
            {
                @"
type effect extends handle
native AddSpecialEffect takes string modelName, real x, real y returns effect
native DestroyEffect takes effect whichEffect returns nothing

function main takes nothing returns nothing
    local effect [|e|] = AddSpecialEffect(""model.mdl"", 0, 0)
endfunction",
            };

            yield return new object[]
            {
                @"
type filterfunc extends handle
native Filter takes code func returns filterfunc
native DestroyFilter takes filterfunc f returns nothing

function myFilter takes nothing returns boolean
    return true
endfunction

function main takes nothing returns nothing
    local filterfunc [|f|] = Filter(function myFilter)
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes nothing returns nothing
    local force [|f|] = CreateForce()
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes nothing returns nothing
    local force [|f|] = CreateForce()
    if true then
        call DestroyForce(f)
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes nothing returns nothing
    local force [|f|] = CreateForce()
    if true then
    else
        call DestroyForce(f)
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes nothing returns nothing
    local force [|f|] = CreateForce()
    if true then
        call DestroyForce(f)
    elseif false then
        call DestroyForce(f)
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function MaybeCreateForce takes boolean flag returns force
    local force [|f|] = CreateForce()
    if flag then
        return f
    endif
    return null
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes boolean flag returns nothing
    local force f = null
    if flag then
        set [|f|] = CreateForce()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
type force extends handle
native CreateForce takes nothing returns force
native DestroyForce takes force whichForce returns nothing

function main takes nothing returns nothing
    local force [|f|] = CreateForce()
    set f = CreateForce()
    call DestroyForce(f)
endfunction",
            };

            yield return new object[]
            {
                @"
type group extends handle
native CreateGroup takes nothing returns group
native DestroyGroup takes group whichGroup returns nothing

function main takes nothing returns nothing
    local group [|g|] = CreateGroup()
endfunction",
            };

            yield return new object[]
            {
                @"
type image extends handle
native CreateImage takes string file, real sizeX, real sizeY, real sizeZ, real posX, real posY, real posZ, real originX, real originY, real originZ, integer imageType returns image
native DestroyImage takes image whichImage returns nothing

function main takes nothing returns nothing
    local image [|img|] = CreateImage(""img.blp"", 64, 64, 0, 0, 0, 0, 0, 0, 0, 1)
endfunction",
            };

            yield return new object[]
            {
                @"
type lightning extends handle
native AddLightning takes string codeName, boolean checkVisibility, real x1, real y1, real x2, real y2 returns lightning
native DestroyLightning takes lightning whichBolt returns boolean

function main takes nothing returns nothing
    local lightning [|l|] = AddLightning(""CLPB"", true, 0, 0, 100, 100)
endfunction",
            };

            yield return new object[]
            {
                @"
type location extends handle
native Location takes real x, real y returns location
native RemoveLocation takes location whichLocation returns nothing

function main takes nothing returns nothing
    local location [|loc|] = Location(100.0, 200.0)
endfunction",
            };

            yield return new object[]
            {
                @"
type rect extends handle
native Rect takes real minx, real miny, real maxx, real maxy returns rect
native RemoveRect takes rect whichRect returns nothing

function main takes nothing returns nothing
    local rect [|r|] = Rect(0, 0, 100, 100)
endfunction",
            };

            yield return new object[]
            {
                @"
type region extends handle
native CreateRegion takes nothing returns region
native RemoveRegion takes region whichRegion returns nothing

function main takes nothing returns nothing
    local region [|r|] = CreateRegion()
endfunction",
            };

            yield return new object[]
            {
                @"
type texttag extends handle
native CreateTextTag takes nothing returns texttag
native DestroyTextTag takes texttag t returns nothing

function main takes nothing returns nothing
    local texttag [|tt|] = CreateTextTag()
endfunction",
            };

            yield return new object[]
            {
                @"
type ubersplat extends handle
native CreateUbersplat takes real x, real y, string name, integer red, integer green, integer blue, integer alpha, boolean forcePaused, boolean noBirthTime returns ubersplat
native DestroyUbersplat takes ubersplat whichSplat returns nothing

function main takes nothing returns nothing
    local ubersplat [|u|] = CreateUbersplat(0, 0, ""LSDS"", 255, 255, 255, 255, false, false)
endfunction",
            };
        }
    }
}