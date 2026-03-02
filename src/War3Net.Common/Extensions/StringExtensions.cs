namespace War3Net.Common.Extensions
{
    public static class StringExtensions
    {
        public static int FromRawcode(this string code)
        {
            return code is not null && code.Length == 4
                ? code[0] | (code[1] << 8) | (code[2] << 16) | (code[3] << 24)
                : 0;
        }
    }
}