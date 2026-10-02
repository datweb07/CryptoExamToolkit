namespace CryptoExam.Core.AES;

public static class AesTransformations
{
    public static void AddRoundKey(byte[] state, byte[] roundKey)
    {
        for (var i = 0; i < 16; i++) state[i] ^= roundKey[i];
    }

    public static void SubBytes(byte[] state, bool inverse = false)
    {
        var box = inverse ? AesTables.InvSBox : AesTables.SBox;
        for (var i = 0; i < 16; i++) state[i] = box[state[i]];
    }

    public static void ShiftRows(byte[] state, bool inverse = false)
    {
        var copy = (byte[])state.Clone();
        for (var row = 0; row < 4; row++)
            for (var column = 0; column < 4; column++)
            {
                var sourceColumn = inverse ? (column - row + 4) % 4 : (column + row) % 4;
                state[row + 4 * column] = copy[row + 4 * sourceColumn];
            }
    }

    public static void MixColumns(byte[] state, bool inverse = false)
    {
        for (var column = 0; column < 4; column++)
        {
            var i = column * 4;
            var a0 = state[i]; var a1 = state[i + 1]; var a2 = state[i + 2]; var a3 = state[i + 3];
            if (!inverse)
            {
                state[i] = (byte)(Multiply(a0, 2) ^ Multiply(a1, 3) ^ a2 ^ a3);
                state[i + 1] = (byte)(a0 ^ Multiply(a1, 2) ^ Multiply(a2, 3) ^ a3);
                state[i + 2] = (byte)(a0 ^ a1 ^ Multiply(a2, 2) ^ Multiply(a3, 3));
                state[i + 3] = (byte)(Multiply(a0, 3) ^ a1 ^ a2 ^ Multiply(a3, 2));
            }
            else
            {
                state[i] = (byte)(Multiply(a0, 14) ^ Multiply(a1, 11) ^ Multiply(a2, 13) ^ Multiply(a3, 9));
                state[i + 1] = (byte)(Multiply(a0, 9) ^ Multiply(a1, 14) ^ Multiply(a2, 11) ^ Multiply(a3, 13));
                state[i + 2] = (byte)(Multiply(a0, 13) ^ Multiply(a1, 9) ^ Multiply(a2, 14) ^ Multiply(a3, 11));
                state[i + 3] = (byte)(Multiply(a0, 11) ^ Multiply(a1, 13) ^ Multiply(a2, 9) ^ Multiply(a3, 14));
            }
        }
    }

    private static byte Multiply(byte value, byte factor)
    {
        byte result = 0;
        while (factor > 0)
        {
            if ((factor & 1) != 0) result ^= value;
            value = (byte)((value << 1) ^ ((value & 0x80) != 0 ? 0x1B : 0));
            factor >>= 1;
        }
        return result;
    }
}
