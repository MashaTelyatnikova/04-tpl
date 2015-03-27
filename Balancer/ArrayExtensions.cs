using System;

namespace Balancer
{
    public static class ArrayExtensions
    {
        public static T[] Shuffle<T>(this T[] array)
        {
            var newArray = (T[])array.Clone();

            var random = new Random();
            for (var i = 0; i < newArray.Length; i++)
            {
                var j = random.Next(newArray.Length);
                Swap(ref newArray[i], ref newArray[j]);
            }

            return newArray;
        }

        private static void Swap<T>(ref T first, ref T second)
        {
            var buf = first;
            first = second;
            second = buf;
        }
    }
}
