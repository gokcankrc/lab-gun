using System;

namespace Ky.ModifierSystem
{
    [Serializable]
    public class AdvancedNumberModifier
    {
        public string id;
        public bool isUnique;
        private AdvancedNumber modifier;
        public Action modifierChanged;

        public AdvancedNumber Modifier
        {
            get => modifier;
            set
            {
                modifier = value;
                modifierChanged?.Invoke();
            }
        }

        public AdvancedNumberModifier(string identifier, AdvancedNumber modifier, bool isUnique = false)
        {
            id = identifier;
            this.modifier = modifier;
            this.isUnique = isUnique;
        }

        public static AdvancedNumberModifier operator +(AdvancedNumberModifier a, AdvancedNumberModifier b)
        {
            return new AdvancedNumberModifier(a.id, a.modifier + b.modifier, a.isUnique || b.isUnique);
        }

        public static AdvancedNumberModifier operator -(AdvancedNumberModifier a, AdvancedNumberModifier b)
        {
            return new AdvancedNumberModifier(a.id, a.modifier - b.modifier, a.isUnique || b.isUnique);
        }
    }
}