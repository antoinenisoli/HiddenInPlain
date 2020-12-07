using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerIOClient; 

public class MainGameplay : MonoBehaviour
{
	BotGenerator generator => FindObjectOfType<BotGenerator>();
	Connection m_IOconnection;
	[SerializeField] bool local = true;
	Client c_client;
	List<Message> msgList = new List<Message>(); //  Message queue implementation
	int playersCount;
	[SerializeField] int playersMax = 4;
	PlayerController newPlayer;
	string userid;

	[SerializeField] GameObject splash;
	[SerializeField] GameObject playerPrefab;

	[SerializeField] Text globalChat;
	[SerializeField] Text displayPlayers;

    private void Start()
    {
		Connect();
		generator.GenerateGame();
	}

    public void Connect()
    {
		Application.runInBackground = true;
		System.Random random = new System.Random();
		userid = "Guest" + random.Next(0, 10000);

		newPlayer = FindObjectOfType<PlayerController>();
		float x = (float)random.NextDouble() * (generator.spawnArea.x/2 - (-generator.spawnArea.x / 2)) - generator.spawnArea.x / 2;
		float y = (float)random.NextDouble() * (generator.spawnArea.y / 2 - (-generator.spawnArea.y / 2)) - generator.spawnArea.y / 2;
		newPlayer.transform.position = new Vector2(x, y);
		newPlayer.SetName(userid);

		PlayerIO.Authenticate(
			"test-rbsgucjwdkyuokbfjora",            //Your game id
			"public",                               //Your connection id
			new Dictionary<string, string> {        //Authentication arguments
				{ "userId", userid },
			},
			null,                                   //PlayerInsight segments
			OnPlayerIOConnected,
			OnErrorConnection
		);
	}

	void OnErrorConnection(PlayerIOError error)
	{
		print("Error connecting: " + error.ToString());
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

	void TryJoinRoom(Client client)
    {
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

    private void OnApplicationQuit()
    {
		m_IOconnection.Disconnect();
    }

    void OnRoomJoined(Connection connection)
    {
		print("Joined Room.");		
		// We successfully joined a room so set up the message handler
		m_IOconnection = connection;
		m_IOconnection.OnMessage += HandleMessage;
		m_IOconnection.Send("SentPlayerPos", c_client.ConnectUserId, newPlayer.transform.position.x, newPlayer.transform.position.y);
	}

	void OnRoomJoinedError(PlayerIOError error)
    {
		print("Error Joining Room: " + error.ToString());
	}

	void HandleMessage(object sender, Message m)
	{
		msgList.Add(m);
	}

    void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (destination.x < generator.spawnArea.x / 2 && destination.x > -generator.spawnArea.x / 2 && destination.y < generator.spawnArea.y / 2 && destination.y > -generator.spawnArea.y / 2)
			{
				m_IOconnection.Send("Move", destination.x, destination.y);
				Instantiate(splash, destination, Quaternion.identity);
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
        {
			m_IOconnection.Send("Chat", userid);
        }

		// process message queue
		foreach (Message m in msgList)
		{
			switch (m.Type)
			{
				case "PlayerJoined":
					playersCount = m.GetInt(0);
					if (playersCount <= playersMax)
                    {
						displayPlayers.text = "Player : " + playersCount + "/" + playersMax;
					}
					break;
				case "SendMessage":
					globalChat.text += "\n" + m.GetString(0) + " said something !";
					break;
				case "SpawnPosition":
					if (playersCount <= playersMax)
					{
						string name = m.GetString(0);
						float x = m.GetFloat(1);
						float y = m.GetFloat(2);
						GameObject newPlayer = Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
						PlayerController pl = newPlayer.GetComponent<PlayerController>();
						pl.SetName(name);
					}
					else
						print("full !");
					break;
				case "Move":
					GameObject player = GameObject.Find(m.GetString(0));
					PlayerController pl2 = player.GetComponent<PlayerController>();
					pl2.StopAllCoroutines();
					pl2.StartCoroutine(pl2.Move(new Vector2(m.GetFloat(1), m.GetFloat(2))));
					break;
			}
		}

		// clear message queue after it's been processed
		msgList.Clear();
	}
}
