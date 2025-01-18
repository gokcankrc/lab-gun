using UnityEngine;

namespace Ky
{
    public static class Dice
    {
        /// <summary>
        /// Check if the %chance possibility happens. Simple.
        /// </summary>
        /// <param name="chance"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Roll(float chance, float max = 100f)
        {
            var roll = Random.Range(0f, max);
            return roll < chance;
        }

        /// <summary>
        /// Rolls a d20. Added arguments for classic DnD rolls.
        /// </summary>
        /// <param name="difficulty">By standard: 1 for lowest, 20 for highest.</param>
        /// <param name="advantage">Below zero for disadvantage, above zero for advantage.</param>
        /// <param name="flatBonus">+ or -, added to difficulty.</param>
        /// <param name="max">20 by default.</param>
        public static bool RollD20(int difficulty, int advantage = 0, int flatBonus = 0, int max = 20)
        {
            Logger.Log($"not tested yet", Logger.DomainType.Critical);
            var roll = Random.Range(1, max + 1);
            if (advantage > 0)
                roll = Mathf.Max(roll, Random.Range(1, max + 1));
            else if (advantage < 0)
                roll = Mathf.Min(roll, Random.Range(1, max + 1));
            return (roll + flatBonus) >= difficulty;
        }

        /// <summary>
        /// Flips a coin. Duh.
        /// </summary>
        public static bool FlipACoin()
        {
            var roll = Random.value;
            return roll < 0.5f;
        }
    }
}