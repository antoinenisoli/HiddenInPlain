using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MovableCharacter
{
    string guestName;
    TextMeshPro displayName;
    BotGenerator generator => FindObjectOfType<BotGenerator>();

    private void Awake()
    {
        displayName = GetComponentInChildren<TextMeshPro>();
    }

    public override IEnumerator Move(Vector3 pos)
    {
        while (Vector2.Distance(transform.position, pos) > 0.01f)
        {
            yield return null;
            transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
        }
    }

    public void SetName(string name)
    {
        gameObject.name = name;
        displayName.text = name;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            if (screenPos.x < generator.gameRect.size.x/2 && screenPos.x > -generator.gameRect.size.x/2 && screenPos.y < generator.gameRect.size.y / 2 && screenPos.y > -generator.gameRect.size.y / 2)
            {
                
            }
        }
    }
}
