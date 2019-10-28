using System.Collections.Generic;
using System.Linq;

namespace BullsAndCows
{
    public static class GameHelper
    {
        public static GameObject GetNewGameObject()
        {
            var possibilities = new List<string>();
            for (var i = 1000; i <= 9876; i++)
            {
                var digits = i.ToString();
                if (digits.IndexOf('0') >= 0)
                {
                    continue;
                }
                if (digits.Distinct().ToArray().Length < 4)
                {
                    continue;
                }
                possibilities.Add(digits);
            }

            return new GameObject
            {
                Possibilities = possibilities.Shuffle(),
                Count = 0
            };
        }
    }
}
