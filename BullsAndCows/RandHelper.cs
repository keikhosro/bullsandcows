using System;
using System.Security.Cryptography;

namespace BullsAndCows
{
    public static class RandHelper
    {
        public static int GenerateRandomInt(int minVal = 0, int maxVal = 100)
        {
            var rnd = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);
            var i = Math.Abs(BitConverter.ToInt32(rnd, 0));
            return Convert.ToInt32(i % (maxVal - minVal + 1) + minVal);
        }
    }
}
