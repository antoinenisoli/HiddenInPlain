using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableCharacter : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 3;
    protected Camera mainCam => Camera.main;

    public abstract IEnumerator Move(Vector3 pos);
}
