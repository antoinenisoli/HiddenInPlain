using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MovableCharacter
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 destination = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (destination.x < MainGameplay.instance.range.x / 2 && destination.x > -MainGameplay.instance.range.x / 2 && destination.y < MainGameplay.instance.range.y / 2 && destination.y > -MainGameplay.instance.range.y / 2)
            {
                MoveMessage.Send(destination.x, destination.y);
            }
        }
    }
}
