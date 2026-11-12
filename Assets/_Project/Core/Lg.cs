using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Loveverse.Core
{
    /// <summary>
    /// Structured logger with formatting, conditional compilation, and correlation ID support.
    /// </summary>
    public static class Lg
    {
        private const string Prefix = "[LOVEVERSE] ";
        public static string CurrentCorrelationId { get; set; } = string.Empty;

        private static string FormatMessage(string message, string correlationId)
        {
            var cid = !string.IsNullOrEmpty(correlationId) ? correlationId : CurrentCorrelationId;
            return !string.IsNullOrEmpty(cid) 
                ? $"{Prefix}[CID:{cid}] {message}" 
                : $"{Prefix}{message}";
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(string message, string correlationId = null)
        {
            Debug.Log(FormatMessage(message, correlationId));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Warning(string message, string correlationId = null)
        {
            Debug.LogWarning(FormatMessage(message, correlationId));
        }

        public static void Error(string message, string correlationId = null)
        {
            Debug.LogError(FormatMessage(message, correlationId));
        }
    }
}
