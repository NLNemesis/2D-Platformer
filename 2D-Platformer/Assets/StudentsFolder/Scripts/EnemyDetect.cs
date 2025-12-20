using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    #region Variables
    public int damage;
    public myPlayer player;
    public myEnemy enemy;

    private bool dealt;
    #endregion
    private void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Player" && !dealt)
        {
            dealt = true;
            player.LoseHP(damage);
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        dealt = false;
    }
}
