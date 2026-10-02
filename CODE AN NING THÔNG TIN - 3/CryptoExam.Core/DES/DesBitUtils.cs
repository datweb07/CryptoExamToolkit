namespace CryptoExam.Core.DES;

public static class DesBitUtils
{
    public static ulong Permute(ulong value, int inputWidth, IReadOnlyList<int> table)
    {
        ulong result = 0;
        foreach (var position in table)
        {
            result <<= 1;
            result |= (value >> (inputWidth - position)) & 1UL;
        }
        return result;
    }

    public static uint RotateLeft28(uint value, int count) => ((value << count) | (value >> (28 - count))) & 0x0FFFFFFF;
}
