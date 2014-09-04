using UnityEngine;
using System.Collections;

public class BatSpawner : MonoBehaviour
{
    public GameObject BatPrefab = null;
    public Transform BatTarget = null;
    public int SpawnCount = 2;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

        for (int i = 0; i < SpawnCount; ++i)
        {
            GameObject newBat = Instantiate(BatPrefab) as GameObject;
            newBat.transform.position = transform.position;

            Vector3 target = BatTarget.position + new Vector3(Random.Range(-15, 15), Random.Range(-25, 25), 0);

            iTween.MoveTo(newBat.gameObject, target, 2.0f);
        }
    }
}
