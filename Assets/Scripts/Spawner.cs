using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int numOfSpawns;
    public float timeBetweenSpawns;
    
    [SerializeField] public GameObject[] spawnpoint;
    [SerializeField] public GameObject[] zombie;
    
    private float timer = 0f;
    // Start is called before the first frame update
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenSpawns)
        {
            timer = 0f;
            StartCoroutine(spawner());
        }
        
    }

    IEnumerator spawner()
    {
        var picksp = spawnpoint[Random.Range(0, spawnpoint.Length)];
        var pickzombie = zombie[Random.Range(0, zombie.Length)];
        
        var pick = Instantiate(pickzombie, picksp.transform.position, Quaternion.identity);

        yield return null;
    }
}
