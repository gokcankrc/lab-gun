using System;
using Sirenix.OdinInspector;
using UnityEngine;

// Can be made static, but it is a MonoBehaviour so it can easily be accessed around DebugManager.
namespace Ky
{
    public class Logger : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<DomainType, Color> colors = new();
        public static SerializableDictionary<DomainType, Color> Colors { get; private set; } = new();

        [ShowInInspector]
        public static DomainType EnabledDomains
        {
            get => (DomainType)PlayerPrefs.GetInt("logger EnabledDomains", -1);
            protected set => PlayerPrefs.SetInt("logger EnabledDomains", (int)value);
        }

        private void Awake()
        {
            Colors = colors;
        }

        public static void Log(string message, UnityEngine.Object context = null)
        {
            DomainType domainType = DomainType.Misc;
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.Log(message, context);
        }

        public static void Log(string message, DomainType domainType, UnityEngine.Object context = null)
        {
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.Log(message, context);
        }

        public static void LogWarning(string message, UnityEngine.Object context = null)
        {
            DomainType domainType = DomainType.Misc;
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.LogWarning(message, context);
        }

        public static void LogWarning(string message, DomainType domainType, UnityEngine.Object context = null)
        {
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.LogWarning(message, context);
        }

        public static void LogError(string message, UnityEngine.Object context = null)
        {
            DomainType domainType = DomainType.Misc;
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.LogError(message, context);
        }

        public static void LogError(string message, DomainType domainType, UnityEngine.Object context = null)
        {
            if (!IsValid(domainType)) return;
            message = Prepare(message, domainType);
            Debug.LogError(message, context);
        }

        private static bool IsValid(DomainType domainType)
        {
            return (domainType & EnabledDomains) != 0;
        }

        private static string Prepare(string message, DomainType domainType)
        {
            Color c = Colors.TryGetValue(domainType, out var color) ? color : Color.white;
            return $"<color=#{c.ToHex()}>{domainType}:</color> {message}";
        }

        [Flags]
        public enum DomainType
        {
            None = 0,

            Critical = 1,
            System = 2,
            Misc = 4,
            Combat = 8,

            All = -1,
        }
    }
}