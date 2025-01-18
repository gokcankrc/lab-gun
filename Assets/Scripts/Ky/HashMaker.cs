using UnityEngine;

namespace Ky
{
    public class HashMaker : MonoBehaviour
    {
        private static int i = 0;

        public static Hash128 GenerateHash()
        {
            Hash128 hash128 = new Hash128();
            hash128.Append(i);
            i += 1;
            return hash128;
        }
    }
}