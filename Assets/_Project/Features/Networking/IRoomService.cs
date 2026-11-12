using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loveverse.Core;

namespace Loveverse.Networking
{
    public interface IRoomService : IGameService
    {
        string CurrentRoomId { get; }
        string CurrentInviteCode { get; }
        bool IsRoomOwner { get; }
        IReadOnlyList<string> RoomMemberIds { get; }

        event Action<string> OnMemberJoined;
        event Action<string> OnMemberLeft;
        event Action OnRoomClosed;

        Task<string> CreatePrivateRoomAsync();
        Task<bool> JoinWithInviteCodeAsync(string inviteCode);
        Task LeaveCurrentRoomAsync();
    }
}
