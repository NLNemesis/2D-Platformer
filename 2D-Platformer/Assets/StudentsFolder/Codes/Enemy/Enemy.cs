using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public Transform[] PatrolPoint;
    public float Speed;
    private float OriginalSpeed;
    private bool Right = true;
    private float Distance;

    [Header("Stats")]
    public float Health;
    #endregion
    private void Start()
    {
        OriginalSpeed = Speed;
    }
    private void Update()
    {
        #region Distance Handler
        if (Right)
           Distance = (this.gameObject.transform.position - PatrolPoint[0].position).magnitude;
        else
            Distance = (this.gameObject.transform.position - PatrolPoint[1].position).magnitude;
        #endregion

        #region Movement Handler
        if (Distance == 0 && Right) 
            Right = false;
        else if (Distance == 0 && !Right)
            Right = true;

        if (Right)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, PatrolPoint[0].position, Speed);
            this.transform.localScale = PatrolPoint[0].localScale;
        }
        else
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, PatrolPoint[1].position, Speed);
            this.transform.localScale = PatrolPoint[1].localScale;
        }
        #endregion
    }

    public void TakeDamage(float Value)
    {
        Health -= Value;
        if (Health <= 0)
            this.gameObject.SetActive(false);
    }
}
