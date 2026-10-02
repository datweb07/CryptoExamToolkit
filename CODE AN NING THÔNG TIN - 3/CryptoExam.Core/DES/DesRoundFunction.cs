namespace CryptoExam.Core.DES;

public static class DesRoundFunction
{
    public static DesRoundFunctionResult Calculate(uint right, ulong subKey)
    {
        var expanded = DesBitUtils.Permute(right, 32, DesTables.Expansion);
        var xor = expanded ^ subKey;
        uint combined = 0;
        var steps = new List<DesSBoxStep>(8);
        for (var box = 0; box < 8; box++)
        {
            var block = (byte)((xor >> (42 - box * 6)) & 0x3F);
            var row = ((block & 0x20) >> 4) | (block & 1);
            var column = (block >> 1) & 0xF;
            var value = DesTables.SBoxes[box, row, column];
            combined = (combined << 4) | (uint)value;
            steps.Add(new(box + 1, block, row, column, value));
        }
        var permuted = (uint)DesBitUtils.Permute(combined, 32, DesTables.P);
        return new(expanded, subKey, xor, combined, permuted, steps);
    }
}
