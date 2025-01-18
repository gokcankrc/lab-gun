using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Ky
{
    public static class HelperFunctions
    {
        public static void RemoveAtFast<T>(this List<T> list, int index)
        {
            int lastElementIndex = list.Count - 1;

            list[index] = list[lastElementIndex];
            list.RemoveAt(lastElementIndex);
        }

        public static T GetRandomElement<T>(this T[] arr)
        {
            return arr[Random.Range(0, arr.Length)];
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static void Shuffle<T>(this T[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int randomIndex = Random.Range(i, arr.Length);

                (arr[i], arr[randomIndex]) = (arr[randomIndex], arr[i]);
            }
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0, length = list.Count; i < length - 1; i++)
            {
                int randomIndex = Random.Range(i, length);

                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}