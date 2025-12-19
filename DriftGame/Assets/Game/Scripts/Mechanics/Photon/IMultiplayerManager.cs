using System;
using Photon.Realtime;

public interface IMultiplayerManager
{
    event Action OnConnectedToMasterEvent;
    event Action OnLobbyJoinedEvent;
    event Action OnOnlineRoomJoined;
    event Action<Player> OnPlayerJoinedEvent;
    event Action<Player> OnPlayerLeftEvent;

    void Connect();
    void JoinRandomRoom();
    void CreateRoom();
    void LeaveRoom();
    void Disconnect();
}
