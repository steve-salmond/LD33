using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public Vector2 Period = Vector2.one;

    public GameObject Prefab;

	void Start ()
	{
	    StartCoroutine(SpawnRoutine());
	}

    IEnumerator SpawnRoutine()
    {
        while (gameObject.activeSelf)
        {
            var wait = Random.Range(Period.x, Period.y);
            yield return new WaitForSeconds(wait);

            Spawn();
        }
	}

    void Spawn()
    { Instantiate(Prefab, transform.position, Quaternion.identity); }
}
