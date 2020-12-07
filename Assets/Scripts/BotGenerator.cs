using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGenerator : MonoBehaviour
{
    [SerializeField] GameObject bot;
    [SerializeField] int unitToCreate = 50;
    public Vector2 spawnArea = new Vector2(13, 9);
    [HideInInspector] public Rect gameRect = new Rect();

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }

    public static Vector2 RandomPoint(Vector2 center, Vector2 size)
    {
        return center + new Vector2((Random.value - 0.5f) * size.x, (Random.value - 0.5f) * size.y);
    }

    public void GenerateGame()
    {
        gameRect = new Rect(transform.position, spawnArea);

        for (int i = 0; i < unitToCreate; i++)
        {
            Instantiate(bot, RandomPoint(gameRect.position, gameRect.size), Quaternion.identity, transform);
        }
    }
}
