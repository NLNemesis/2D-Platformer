using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
            animator.Play("Move");
        else
            animator.Play("Idle");
    }
}
