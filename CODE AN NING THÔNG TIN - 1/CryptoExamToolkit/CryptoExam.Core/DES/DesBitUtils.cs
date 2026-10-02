using System.Text;

namespace CryptoExam.Core.DES
{
    public static class DesBitUtils
    {
        public static string Permute(string input, int[] table)
        {
            var sb = new StringBuilder(table.Length);
            for (int i = 0; i < table.Length; i++)
            {
                // DES tables are 1-indexed
                sb.Append(input[table[i] - 1]);
            }
            return sb.ToString();
        }
    }
}
