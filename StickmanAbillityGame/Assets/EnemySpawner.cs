using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float TimeBeforeSpawning = 1;
    public GameObject[] Enemies;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnemySpawning");
    }

    // Update is called once per frame
    void Update()
    {
    }
    private IEnumerator EnemySpawning()
    {
        yield return new WaitForSeconds(TimeBeforeSpawning);
        PhotonNetwork.Instantiate(Enemies[Random.Range(0, Enemies.Length)].name, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, 0);
        yield return new WaitForSeconds(Random.Range(12, 20));
        StartCoroutine("EnemySpawning");
    }
}
