using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class MovableCharacter : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5;
    protected Camera mainCam => Camera.main;
    protected TextMeshPro displayName;

    private void Awake()
    {
        displayName = GetComponentInChildren<TextMeshPro>();
    }

    public void SetName(string name)
    {
        if (displayName == null)
            return;

        gameObject.name = name;
        displayName.text = name;
    }

    public virtual IEnumerator Move(Vector3 pos)
    {
        while (Vector2.Distance(transform.position, pos) > 0.01f)
        {
            yield return null;
            transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
        }
    }
}
