using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mySpawnClone : MonoBehaviour
{
    [Header("References")]
    public GameObject bossObject;

    [Header("Clone Spawner")]
    public GameObject[] spawnPrefab;
    public Transform[] spawnTransform;

    public void CallCloneSpawn(float timer)
    {
        StartCoroutine(CallCloneSpawnRoutine(timer));
    }

    IEnumerator CallCloneSpawnRoutine(float timer)
    {
        bossObject.SetActive(false);
        myEnemy bossEnemy = bossObject.GetComponentInChildren<myEnemy>();
        for (int i = 0; i < spawnPrefab.Length; i++)
        {
            GameObject clone = Instantiate(spawnPrefab[i], spawnTransform[i].position, Quaternion.identity);
            myEnemy cloneEnemy = clone.GetComponentInChildren<myEnemy>();
            //Assign values
            cloneEnemy.player = bossEnemy.player;
            cloneEnemy.healthBar.maxValue = cloneEnemy.health;
            cloneEnemy.healthBar.value = cloneEnemy.health;
        }
        yield return new WaitForSeconds(timer);
        bossObject.SetActive(true);
    }
}
