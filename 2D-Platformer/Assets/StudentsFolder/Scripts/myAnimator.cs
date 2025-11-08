using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
    private Animator animator;
    private myPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<myPlayer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.frozen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("Attack");
                player.Freeze();

            }
        }
    }
}
