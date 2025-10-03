using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCode : MonoBehaviour
{
    /*
    1) private-public
    2) int-float-bool-string
    3) name
    private bool Alive;
    */
    public float age;
    public string name;
    public bool kid;

    private int number;

    void Start()
    {
        Debug.Log(name + age + kid);
    }
    void Update()
    {
        if (Deal == true && Health > 0)
            DealDamage();
    }

    public bool Deal;
    public int Health;
    void DealDamage()
    {
        Health -= 10;
        Debug.Log("New health is " + Health);
        if (Health == 0)
            Debug.Log("Player is dead");
        else
            Deal = false;
    }
}
