using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Loveverse.Editor
{
    /// <summary>
    /// Automated build profiles supporting all four official targets: Android, iOS, Windows, and Linux.
    /// Callable via CLI (e.g. -executeMethod Loveverse.Editor.PlatformBuildProfiles.BuildAndroidRelease).
    /// </summary>
    public static class PlatformBuildProfiles
    {
        private static readonly string[] DefaultScenes = new[]
        {
            "Assets/_Project/Scenes/Bootstrap.unity",
            "Assets/_Project/Scenes/MainMenu.unity",
            "Assets/_Project/Scenes/LoveTown.unity"
        };

        private static string GetBuildOutputDirectory(string platformName)
        {
            string buildRoot = Path.Combine(Directory.GetCurrentDirectory(), "Builds", platformName);
            if (!Directory.Exists(buildRoot))
            {
                Directory.CreateDirectory(buildRoot);
            }
            return buildRoot;
        }

        // --- Android Builds ---
        public static void BuildAndroidDevelopment()
        {
            BuildPlayer(
                BuildTarget.Android,
                Path.Combine(GetBuildOutputDirectory("Android"), "Loveverse_Dev.apk"),
                BuildOptions.Development | BuildOptions.AllowDebugging
            );
        }

        public static void BuildAndroidRelease()
        {
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPlayer(
                BuildTarget.Android,
                Path.Combine(GetBuildOutputDirectory("Android"), "Loveverse_Release.aab"),
                BuildOptions.None
            );
        }

        // --- iOS Builds ---
        public static void BuildIOSDevelopment()
        {
            BuildPlayer(
                BuildTarget.iOS,
                Path.Combine(GetBuildOutputDirectory("iOS"), "Xcode_Dev"),
                BuildOptions.Development | BuildOptions.AllowDebugging
            );
        }

        public static void BuildIOSRelease()
        {
            BuildPlayer(
                BuildTarget.iOS,
                Path.Combine(GetBuildOutputDirectory("iOS"), "Xcode_Release"),
                BuildOptions.None
            );
        }

        // --- Windows Standalone Builds ---
        public static void BuildWindowsDevelopment()
        {
            BuildPlayer(
                BuildTarget.StandaloneWindows64,
                Path.Combine(GetBuildOutputDirectory("Windows"), "Loveverse_Dev.exe"),
                BuildOptions.Development | BuildOptions.AllowDebugging
            );
        }

        public static void BuildWindowsRelease()
        {
            BuildPlayer(
                BuildTarget.StandaloneWindows64,
                Path.Combine(GetBuildOutputDirectory("Windows"), "Loveverse.exe"),
                BuildOptions.None
            );
        }

        // --- Linux Standalone Builds ---
        public static void BuildLinuxDevelopment()
        {
            BuildPlayer(
                BuildTarget.StandaloneLinux64,
                Path.Combine(GetBuildOutputDirectory("Linux"), "Loveverse_Dev.x86_64"),
                BuildOptions.Development | BuildOptions.AllowDebugging
            );
        }

        public static void BuildLinuxRelease()
        {
            BuildPlayer(
                BuildTarget.StandaloneLinux64,
                Path.Combine(GetBuildOutputDirectory("Linux"), "Loveverse.x86_64"),
                BuildOptions.None
            );
        }

        private static void BuildPlayer(BuildTarget target, string locationPath, BuildOptions options)
        {
            Debug.Log($"[LOVEVERSE Build] Starting build for target: {target} at {locationPath}");

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = DefaultScenes,
                locationPathName = locationPath,
                target = target,
                options = options
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[LOVEVERSE Build] Succeeded: {summary.totalSize} bytes in {summary.totalTime.TotalSeconds:F1}s.");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[LOVEVERSE Build] Failed with {summary.totalErrors} errors.");
            }
        }
    }
}
