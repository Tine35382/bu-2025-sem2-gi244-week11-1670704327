using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    public Rigidbody box;

    private Coroutine goodByeRoutine;



    void Start()
    {
        //InvokeRepeating(nameof(RandomSpawn), 0, 5f);

        //StartCoroutine(MoveBox());
        StartCoroutine(SpawnRoutine());
    }


    void RandomSpawn()
    {
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }



    IEnumerator MoveBox()
    {

        while (true)
        {
            box.linearVelocity = 10 * Vector3.up;
            yield return new WaitForSeconds(3);
            box.linearVelocity = 10 * Vector3.right;
            yield return new WaitForSeconds(3);
            box.linearVelocity = 10 * Vector3.down;
            yield return new WaitForSeconds(3);
            box.linearVelocity = 10 * Vector3.left;
            yield return new WaitForSeconds(3);

        }
    }

    /*private void Update()
    {
        if (Time.time > 5)
        {
            if (goodByeRoutine != null)
            {
                StopCoroutine(goodByeRoutine);

            }
            
        }
    }*/

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            RandomSpawn();
            yield return new WaitForSeconds(3);
        }

    }

}


