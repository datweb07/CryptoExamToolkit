namespace CryptoExam.Core.DES;

public sealed record MultiDesResult(string Step1, string Step2, string? Step3, string FinalCiphertext);

public static class TripleDesService
{
    public static MultiDesResult DoubleEncrypt(string plaintext, string key1, string key2)
    {
        var step1 = DesCipher.Encrypt(plaintext, key1);
        var step2 = DesCipher.Encrypt(step1, key2);
        return new(step1, step2, null, step2);
    }

    public static MultiDesResult DoubleDecrypt(string ciphertext, string key1, string key2)
    {
        var step1 = DesCipher.Decrypt(ciphertext, key2);
        var step2 = DesCipher.Decrypt(step1, key1);
        return new(step1, step2, null, step2);
    }

    public static MultiDesResult EncryptEde(string plaintext, string key1, string key2, string? key3 = null)
    {
        key3 ??= key1;
        var step1 = DesCipher.Encrypt(plaintext, key1);
        var step2 = DesCipher.Decrypt(step1, key2);
        var step3 = DesCipher.Encrypt(step2, key3);
        return new(step1, step2, step3, step3);
    }

    public static MultiDesResult DecryptEde(string ciphertext, string key1, string key2, string? key3 = null)
    {
        key3 ??= key1;
        var step1 = DesCipher.Decrypt(ciphertext, key3);
        var step2 = DesCipher.Encrypt(step1, key2);
        var step3 = DesCipher.Decrypt(step2, key1);
        return new(step1, step2, step3, step3);
    }

    public static MultiDesResult EncryptEee(string plaintext, string key1, string key2, string key3)
    {
        var step1 = DesCipher.Encrypt(plaintext, key1);
        var step2 = DesCipher.Encrypt(step1, key2);
        var step3 = DesCipher.Encrypt(step2, key3);
        return new(step1, step2, step3, step3);
    }
}
