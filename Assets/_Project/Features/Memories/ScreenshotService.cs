using System;
using UnityEngine;

namespace Loveverse.Memories
{
    [Serializable]
    public sealed class MemoryMetadata
    {
        public string MemoryId;
        public string WorldId;
        public string AuthorId;
        public string Caption;
        public string LocationTag;
        public string IsoTimestamp;
    }

    /// <summary>
    /// Captures the screen buffer without active HUD layers and creates ready-to-save Texture2D snapshots.
    /// </summary>
    public static class ScreenshotService
    {
        public static Texture2D CaptureViewport()
        {
            int w = Screen.width;
            int h = Screen.height;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
