using UnityEngine;
using Sirenix.OdinInspector;

namespace Ky
{
    [CreateAssetMenu(menuName = "Ky/Term")]
    public class Term : ScriptableObject
    {
        // Could add hash, ID, etc.
        [ShowInInspector] public string Name => name;
        public string Id => name;

        public static implicit operator string(Term term)
        {
            return term.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool IsEmpty()
        {
            return this == "_";
        }
    }
}