using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks, IMultiplayerManager
{
    // EVENTS (для GameController, CarSpawner)
    public event Action OnConnectedToMasterEvent;
    public event Action OnLobbyJoinedEvent;
    public event Action OnOnlineRoomJoined;
    public event Action<Player> OnPlayerJoinedEvent;
    public event Action<Player> OnPlayerLeftEvent;


    [Header("Settings")]
    [SerializeField] private string gameplaySceneName = "FirstMap";
    [SerializeField] private byte maxPlayers = 2;
    

    private void Awake()
    {
        // Photon settings
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.KeepAliveInBackground = 60;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ==========================
    //       PUBLIC API
    // ==========================

    /// <summary>
    /// Підключення до мастера Photon.
    /// Викликається з меню → перед Join/Create.
    /// </summary>
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("[MP] Connecting to Photon...");
        }
    }

    /// <summary>
    /// Спроба зайти в будь-яку відкриту кімнату.
    /// Викликається після входу в лобі.
    /// </summary>
    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("[MP] Joining random room...");
        }
        else
        {
            Debug.LogWarning("[MP] Cannot join random: not connected.");
        }
    }

    /// <summary>
    /// Створює нову кімнату з рендомним ім'ям.
    /// Викликається якщо JoinRandomRoom не зміг знайти кімнату.
    /// </summary>
    public void CreateRoom()
    {
        string roomName = "Room_" + UnityEngine.Random.Range(1000, 9999);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsVisible = true,
            IsOpen = true,
            PublishUserId = true
        };

        Debug.Log("[MP] Creating room " + roomName);
        PhotonNetwork.CreateRoom(roomName, options);
    }

    /// <summary>
    /// Вихід з кімнати.
    /// Викликати при виході в меню.
    /// </summary>
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("[MP] Leaving room...");
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// Повне відключення від Photon.
    /// Викликати при виході з гри.
    /// </summary>
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("[MP] Disconnecting...");
            PhotonNetwork.Disconnect();
        }
    }

    // ==========================
    //     PHOTON CALLBACKS
    // ==========================

    public override void OnConnectedToMaster()
    {
        Debug.Log("[MP] Connected to master.");
        PhotonNetwork.JoinLobby();            // ← Після цього можна викликати JoinRandomRoom
        OnConnectedToMasterEvent?.Invoke();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[MP] Joined lobby.");
        OnLobbyJoinedEvent?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("[MP] JoinRandom failed. Creating room.");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[MP] Joined room: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[MP] MasterClient → loading scene...");
            PhotonNetwork.LoadLevel(gameplaySceneName);
        }
        else
        {
            OnOnlineRoomJoined?.Invoke();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("[MP] Player joined: " + newPlayer.NickName);
        OnPlayerJoinedEvent?.Invoke(newPlayer);
        OnOnlineRoomJoined?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("[MP] Player left: " + otherPlayer.NickName);
        OnPlayerLeftEvent?.Invoke(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[MP] Left room → loading menu.");
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("[MP] Disconnected: " + cause);
    }

    // ==========================
    //    SCENE LOADED CALLBACK
    // ==========================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplaySceneName && PhotonNetwork.InRoom)
        {
            Debug.Log("[MP] Gameplay scene loaded → safe to spawn cars.");
            OnOnlineRoomJoined?.Invoke();     // ← тут спавнимо машини
        }
    }
}
