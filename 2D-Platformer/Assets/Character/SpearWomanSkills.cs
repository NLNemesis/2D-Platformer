using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWomanSkills : MonoBehaviour
{
    #region Variables
    [Header("Transformation")]
    public float RequiredMana;
    public float ManaReductionDuration;
    public float ManaReduction;

    [Header("References")]
    private PlayerMovement PM;
    private AnimController AC;
    #endregion

    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        AC = GetComponent<AnimController>();
    }

    private void Update()
    {
        #region Transformation
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!AC.animator.GetBool("Transformation_Activity"))
            {
                AC.animator.SetBool("Transformation_Activity", true);
                AC.animator.SetTrigger("Transform");
            }
            else
            {
                AC.animator.SetBool("Transformation_Activity", false);
                AC.animator.SetTrigger("Transform");
            }
        }
        #endregion
    }
}
