using System;
using System.Collections.Generic;

class Result
{
    private static int[][] prefixCount;
    private static int MOD = 1000000007;
    private static long[] fact;
    private static long[] invFact;

    // Precompute prefix sums and factorials
    public static void initialize(string s)
    {
        int n = s.Length;

        // Prefix sums for each character
        prefixCount = new int[26][];
        for (int i = 0; i < 26; i++)
            prefixCount[i] = new int[n + 1];

        for (int i = 0; i < n; i++)
        {
            int c = s[i] - 'a';
            for (int j = 0; j < 26; j++)
                prefixCount[j][i + 1] = prefixCount[j][i] + (j == c ? 1 : 0);
        }

        // Precompute factorials and inverse factorials
        fact = new long[n + 1];
        invFact = new long[n + 1];
        fact[0] = 1;
        for (int i = 1; i <= n; i++)
            fact[i] = (fact[i - 1] * i) % MOD;

        for (int i = 0; i <= n; i++)
            invFact[i] = ModInverse(fact[i], MOD);
    }

    // Answer a query
    public static int answerQuery(int l, int r)
    {
        int oddCount = 0;
        int halfLength = 0;
        List<int> halfCounts = new List<int>();

        for (int i = 0; i < 26; i++)
        {
            int count = prefixCount[i][r] - prefixCount[i][l - 1];
            if (count > 0)
            {
                int half = count / 2;
                if (half > 0)
                    halfCounts.Add(half);
                halfLength += half;
                if (count % 2 != 0)
                    oddCount++;
            }
        }

        if (halfLength == 0)
            return Math.Max(oddCount, 1);

        long numerator = fact[halfLength];
        long denominator = 1;
        foreach (var c in halfCounts)
            denominator = (denominator * fact[c]) % MOD;

        long result = (numerator * ModInverse(denominator, MOD)) % MOD;
        result = (result * Math.Max(oddCount, 1)) % MOD;

        return (int)result;
    }

    private static long ModInverse(long a, long mod)
    {
        return ModPow(a, mod - 2, mod);
    }

    private static long ModPow(long x, long n, long mod)
    {
        long result = 1;
        x %= mod;
        while (n > 0)
        {
            if ((n & 1) == 1)
                result = (result * x) % mod;
            x = (x * x) % mod;
            n >>= 1;
        }
        return result;
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        string s = Console.ReadLine();
        Result.initialize(s);

        int q = Convert.ToInt32(Console.ReadLine().Trim());

        for (int qItr = 0; qItr < q; qItr++)
        {
            string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

            int l = Convert.ToInt32(firstMultipleInput[0]);
            int r = Convert.ToInt32(firstMultipleInput[1]);

            int result = Result.answerQuery(l, r);

            Console.WriteLine(result);
        }
    }
}
