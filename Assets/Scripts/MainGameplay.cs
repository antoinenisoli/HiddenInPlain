using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerIOClient;
using UnityEngine.EventSystems;

public class MainGameplay : MonoBehaviour
{
	public static MainGameplay instance;
	public Dictionary<string, MovableCharacter> currentPlayers = new Dictionary<string, MovableCharacter>();
	[SerializeField] int playersMax = 4;
	[SerializeField] GameObject splash;

	[Header("Spawn")]
	public GameObject playerPrefab;
	public GameObject playerDistantPrefab;

	[Header("Display")]
	[SerializeField] Text globalChat;
	[SerializeField] Text displayPlayers;

	[Header("Bot")]
	[SerializeField] GameObject botPrefab;
	public Vector2 range = new Vector2(13, 9);

	private void Awake()
    {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

    private void OnDrawGizmosSelected()
    {
		Gizmos.DrawWireCube(transform.position, range);
	}

    private void Start()
    {
		Application.runInBackground = true;
		System.Random random = new System.Random();
		string userid = "Guest" + random.Next(0, 10000);
		NetworkManager.Instance.Initialize(userid);
	}

	public void CreateBot(string id, float posx, float posy)
    {
		GameObject newBot = Instantiate(playerDistantPrefab, new Vector2(posx, posy), Quaternion.identity);
		MovableCharacter bot = newBot.GetComponent<MovableCharacter>();
		bot.SetName(id);
		currentPlayers.Add(id, bot);
	}

	public void CreatePlayer(string id, float posx, float posy)
	{
		GameObject newPlayer = Instantiate(playerPrefab, new Vector2(posx, posy), Quaternion.identity);
		MovableCharacter player = newPlayer.GetComponent<MovableCharacter>();
		player.SetName(id);
		currentPlayers.Add(id, player);
	}

	public void MovePlayer(string id, Vector2 destination)
    {
		if (currentPlayers.TryGetValue(id, out MovableCharacter player))
		{
			MovableCharacter pl = player.GetComponent<MovableCharacter>();
			pl.StopAllCoroutines();
			pl.StartCoroutine(pl.Move(destination));

			if (NetworkManager.Instance.c_client.ConnectUserId == id)
            {
				Instantiate(splash, destination, Quaternion.identity);
            }
		}
		else
        {
			Debug.LogError(id + " doesn't exist !");
        }
	}

	public void RemovePlayer(string id)
    {
		if (currentPlayers.TryGetValue(id, out MovableCharacter leftPlayer))
        {
			currentPlayers.Remove(id);
			Destroy(leftPlayer.gameObject);
		}
	}

    public void ChangePlayerCount()
    {
		displayPlayers.text = "Player : " + currentPlayers.Count + "/" + playersMax;
	}
}
