using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerIOClient;
using UnityEngine.EventSystems;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public Connection m_IOconnection;
    [SerializeField] bool local = true;
    public Client c_client;

    List<Message> msgList = new List<Message>(); //  Message queue implementation
    Dictionary<string, BaseMessage> messages = new Dictionary<string, BaseMessage>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Application.runInBackground = true;
    }

    public void Initialize(string id)
    {
        InitializeMessages();
        PlayerIO.Authenticate(
            "test-rbsgucjwdkyuokbfjora",            //Your game id
            "public",                               //Your connection id
            new Dictionary<string, string> {        //Authentication arguments
				{ "userId", id },
            },
            null,                                   //PlayerInsight segments
            OnPlayerIOConnected,
            OnErrorConnection
        );
    }

    void InitializeMessages()
    {
        messages.Add(StartGameplayMessage.MessageType, new StartGameplayMessage());
        messages.Add(MoveMessage.MessageType, new MoveMessage());
        messages.Add(SpawnPositionMessage.MessageType, new SpawnPositionMessage());
        messages.Add(PlayerJoinedMessage.MessageType, new PlayerJoinedMessage());
        messages.Add(PlayerLeftMessage.MessageType, new PlayerLeftMessage());
    }

    private void OnApplicationQuit()
    {
        m_IOconnection.Disconnect();
    }

    void OnErrorConnection(PlayerIOError error)
    {
        print("Error connecting: " + error.ToString());
    }

    void HandleMessage(object sender, Message m)
    {
        msgList.Add(m);
    }

    void OnRoomJoined(Connection connection)
    {
        // We successfully joined a room so set up the message handler
        m_IOconnection = connection;
        print("Joined Room.");
        m_IOconnection.OnMessage += HandleMessage;
    }

    void OnRoomJoinedError(PlayerIOError error)
    {
        print("Error Joining Room: " + error.ToString());
    }

    void TryJoinRoom(Client client)
    {
        c_client = client;
        //Create or join the room 
        client.Multiplayer.CreateJoinRoom(
            "MainRoom",                    //Room id. If set to null a random roomid is used
            "HiddenGameplay",                   //The room type started on the server
            true,                               //Should the room be visible in the lobby?
            null,
            null,
            OnRoomJoined,
            OnRoomJoinedError
        );
    }

    private void OnPlayerIOConnected(Client client)
    {
        print("Successfully connected to Player.IO");

        // Comment out the line below to use the live servers instead of your development server
        if (local)
            client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        c_client = client;
        TryJoinRoom(client);
    }

    public void SendNetworkMessage(string type, params object[] parameters)
    {
        m_IOconnection.Send(type, parameters);
    }

    private void Update()
    {
        foreach (Message m in msgList)
        {
            if (messages.TryGetValue(m.Type, out BaseMessage message))
            {
                message.Receive(m);
                print(message.ToString());
                MainGameplay.instance.ChangePlayerCount();
            }
        }

        msgList.Clear();
    }
}
