using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MovableCharacter
{
    BotGenerator generator;
    [SerializeField] float cooldown = 1;
    [SerializeField] Vector2 range = new Vector2(7, 7);
    Rect gameRect = new Rect();

    public void Awake()
    {
        generator = FindObjectOfType<BotGenerator>();
        gameRect = new Rect(transform.parent.position, range);
        StartCoroutine(Move(BotGenerator.RandomPoint(gameRect.position, gameRect.size)));
    }

    public override IEnumerator Move(Vector3 pos)
    {
        while (Vector2.Distance(transform.position, pos) > 0.01f)
        {
            yield return null;
            Debug.DrawLine(transform.position, pos, Color.red);
            transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
        }

        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Move(BotGenerator.RandomPoint(gameRect.position, gameRect.size)));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (generator)
            Gizmos.DrawWireCube(generator.transform.position, range);
        else
            Gizmos.DrawWireCube(transform.position, range);
    }
}
