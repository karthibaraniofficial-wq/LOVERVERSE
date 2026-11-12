using System;

namespace Loveverse.Memories
{
    [Serializable]
    public sealed class MemoryItem
    {
        public string MemoryId;
        public string WorldId;
        public string CreatedBy;
        public string Caption;
        public string LocationId;
        public string LocalFilePath;
        public string CreatedAt;
    }
}
