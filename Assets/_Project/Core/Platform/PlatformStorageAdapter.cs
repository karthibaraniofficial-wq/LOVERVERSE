using System;
using System.IO;
using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Cross-platform filesystem storage adapter normalizing paths across Android, iOS, Windows, and Linux.
    /// Strictly eliminates hardcoded path separators and platform-specific root assumptions.
    /// </summary>
    public static class PlatformStorageAdapter
    {
        public static string GetSafeStoragePath(string subDirectory = "")
        {
            string root = Application.persistentDataPath;
            if (string.IsNullOrEmpty(subDirectory))
            {
                return root;
            }

            // Normalize path using Path.Combine and Path.DirectorySeparatorChar
            string normalizedSub = subDirectory.Replace('\\', Path.DirectorySeparatorChar)
                                               .Replace('/', Path.DirectorySeparatorChar)
                                               .TrimStart(Path.DirectorySeparatorChar);

            string fullPath = Path.Combine(root, normalizedSub);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return fullPath;
        }

        public static string GetMemoriesDirectory()
        {
            return GetSafeStoragePath("Memories");
        }

        public static string GetSettingsFilePath()
        {
            return Path.Combine(GetSafeStoragePath("Config"), "settings.json");
        }

        public static string GetCacheDirectory()
        {
            string cacheRoot = Path.Combine(Application.temporaryCachePath, "LoveverseCache");
            if (!Directory.Exists(cacheRoot))
            {
                Directory.CreateDirectory(cacheRoot);
            }
            return cacheRoot;
        }

        public static bool SafeFileExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return false;
            return File.Exists(filePath);
        }

        public static void SafeDeleteFile(string filePath)
        {
            if (SafeFileExists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

