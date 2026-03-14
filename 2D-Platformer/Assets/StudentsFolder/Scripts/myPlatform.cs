using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlatform : MonoBehaviour
{
    #region Variables
    private int direction = 1;
    public Transform[] position;
    #endregion

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - position[0].position).magnitude;
        float distance1 = (transform.position - position[1].position).magnitude;

        if (direction == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                position[0].position, Speed);
        }
    }
}
