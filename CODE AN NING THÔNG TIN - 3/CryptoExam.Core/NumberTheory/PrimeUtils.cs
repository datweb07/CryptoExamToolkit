using System.Numerics;

namespace CryptoExam.Core.NumberTheory;

public static class PrimeUtils
{
    public static bool IsPrime(BigInteger number)
    {
        if (number < 2) return false;
        if (number % 2 == 0) return number == 2;
        for (var divisor = new BigInteger(3); divisor * divisor <= number; divisor += 2)
            if (number % divisor == 0) return false;
        return true;
    }
}
