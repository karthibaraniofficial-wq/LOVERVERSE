using System.Collections.Generic;
using UnityEngine;

namespace Loveverse.Networking
{
    /// <summary>
    /// Tracks shared session properties such as room code, capacity, and connected partner membership.
    /// </summary>
    public sealed class NetworkRoomState : MonoBehaviour
    {
        [SerializeField] private string _roomCode;
        [SerializeField] private int _maxCapacity = 2;
        [SerializeField] private List<string> _memberIds = new List<string>();

        public string RoomCode => _roomCode;
        public int MaxCapacity => _maxCapacity;
        public IReadOnlyList<string> MemberIds => _memberIds;

        public bool IsFull => _memberIds.Count >= _maxCapacity;

        public void SetRoom(string roomCode)
        {
            _roomCode = roomCode;
            _memberIds.Clear();
        }

        public bool AddMember(string memberId)
        {
            if (IsFull || _memberIds.Contains(memberId)) return false;
            _memberIds.Add(memberId);
            return true;
        }

        public bool RemoveMember(string memberId)
        {
            return _memberIds.Remove(memberId);
        }
    }
}
