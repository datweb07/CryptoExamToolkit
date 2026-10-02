using System.Collections.Generic;

namespace CryptoExam.Core.Common
{
    public static class FormattingUtils
    {
        public static string FormatBlocks(string text, int blockSize, string separator = " ")
        {
            if (string.IsNullOrEmpty(text)) return text;
            
            var blocks = new List<string>();
            for (int i = 0; i < text.Length; i += blockSize)
            {
                int length = System.Math.Min(blockSize, text.Length - i);
                blocks.Add(text.Substring(i, length));
            }
            return string.Join(separator, blocks);
        }
    }
}
