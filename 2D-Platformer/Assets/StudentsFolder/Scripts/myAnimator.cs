using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
<<<<<<< HEAD
    private Animator animator;
    private myPlayer player;

=======
    public Animator animator;
    public myPlayer player;
<<<<<<< Updated upstream
=======
>>>>>>> 0ff38a3bfa8dfd332af1a923e402f0b70d7207a6
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<myPlayer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        if (!player.frozen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("Attack");
                player.Freeze();
=======
>>>>>>> Stashed changes
        if (player.inAir == false)
        {
            if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
                animator.Play("Move");
            else
                animator.Play("Idle");

            if (Input.GetButtonDown("Jump"))
            {
                player.inAir = true;
                animator.Play("Jump");
<<<<<<< Updated upstream
=======
>>>>>>> 0ff38a3bfa8dfd332af1a923e402f0b70d7207a6
>>>>>>> Stashed changes
            }
        }
    }
}
