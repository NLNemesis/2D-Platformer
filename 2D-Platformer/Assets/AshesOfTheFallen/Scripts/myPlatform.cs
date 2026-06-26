using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlatform : MonoBehaviour
{
    #region Variables
    [Header("Controls")]
    public bool canMove;

    [Header("Platform Movement")]
    public float Speed;
    public Transform[] newTransform;
    private int direction = -1;
    #endregion
    private void Update()
    {
        if (!canMove) return;

        float Distance = (newTransform[0].position - this.transform.position).magnitude;
        float Distance1 = (newTransform[1].position - this.transform.position).magnitude;

        if (Distance == 0 && direction == -1)
            direction = 1;
        else if (Distance1 == 0 && direction == 1)
            direction = -1;

        if (direction == 1)
            this.transform.position = Vector2.MoveTowards(this.transform.position, newTransform[1].position, Speed);
        else if (direction == -1)
            this.transform.position = Vector2.MoveTowards(this.transform.position, newTransform[0].position, Speed);
    }

    public void Toggle_canMove(bool value)
    {
        canMove = value;
    }
}
