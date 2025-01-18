using System;
using System.Collections.Generic;
using Ky.ModifierSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ky
{
    [Serializable]
    public class Stat
    {
        public string key;
        [SerializeField] private AdvancedNumber baseNumber = new();
        private List<AdvancedNumberModifier> modifiers;
        private AdvancedNumber cachedNumber = new();
        [ShowInInspector] private bool dirty = true;

        [NonSerialized] public Action onStatGotDirty;
        [NonSerialized] public Action onStatRefresh;

        [ShowInInspector]
        public AdvancedNumber Value
        {
            get
            {
                RefreshIfDirty();
                return cachedNumber;
            }
        }

        public Stat(string key, float baseValue)
        {
            this.key = key;
            baseNumber = new AdvancedNumber(baseValue);
            modifiers = new List<AdvancedNumberModifier>();
        }

        public Stat(string key, AdvancedNumber baseNumber)
        {
            this.key = key;
            this.baseNumber = baseNumber;
            modifiers = new List<AdvancedNumberModifier>();
        }

        public void AddModifier(AdvancedNumberModifier modifier)
        {
            modifiers.Add(modifier);
            modifier.modifierChanged += SetDirty;
            cachedNumber += modifier.Modifier;
        }

        public void RemoveModifier(AdvancedNumberModifier modifier)
        {
            modifiers.Remove(modifier);
            modifier.modifierChanged += SetDirty;
            cachedNumber -= modifier.Modifier;
        }

        public void SetDirty()
        {
            dirty = true;
            onStatGotDirty?.Invoke();
        }

        public void RefreshIfDirty()
        {
            if (dirty)
                Refresh();
        }

        [Button]
        protected void Refresh()
        {
            cachedNumber = baseNumber;
            foreach (AdvancedNumberModifier modifier in modifiers)
                cachedNumber += modifier.Modifier;
            dirty = false;
            onStatRefresh?.Invoke();
        }
    }
}