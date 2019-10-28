using System.Collections.Generic;

namespace BullsAndCows
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var k = RandHelper.GenerateRandomInt(0, i);
                var value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
            return list;
        }
    }
}
