using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossCage : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    public TextMeshProUGUI BossType;
    public TextMeshProUGUI EnemyName;
    public Slider HealthSlider;
    private bool Dead;

    [Header("References")]
    private AIMove aiMove;

    public UnityEvent SpawnEvent;
    public UnityEvent DeathEvent;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        aiMove = GetComponentInChildren<AIMove>();
        aiMove.AIFreeze = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (aiMove.Health <= 0 && Dead == false)
        {
            Dead = true;
            DeathEvent.Invoke();
        }
        else
        {
            HealthSlider.value = aiMove.Health;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Dead == false)
        {
            aiMove.AIFreeze = false;
            BossType.text = aiMove.enemyType.ToString();
            EnemyName.text = aiMove.gameObject.name;
            HealthSlider.maxValue = aiMove.Health;
            SpawnEvent.Invoke();
        }
    }
}
