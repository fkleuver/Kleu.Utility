using System.Collections.Generic;

namespace Kleu.Utility.Common
{
    public static class ByteExtensions
    {
        public static IEnumerable<bool> ToBitArray(this byte b)
        {
            for (var i = 0; i < 8; i++)
            {
                yield return (b & 0x80) != 0;
                b *= 2;
            }
        }
    }
}
