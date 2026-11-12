using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Loveverse.Core
{
    /// <summary>
    /// Structured logger with formatting and conditional compilation.
    /// </summary>
    public static class Lg
    {
        private const string Prefix = "[LOVEVERSE] ";

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(string message)
        {
            Debug.Log($"{Prefix}{message}");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Warning(string message)
        {
            Debug.LogWarning($"{Prefix}{message}");
        }

        public static void Error(string message)
        {
            Debug.LogError($"{Prefix}{message}");
        }
    }
}
