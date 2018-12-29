using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDestroyed : MonoBehaviour {
    public GameObject smokePrefab;
    public int smokeCount = 10;
    public float smokeSpawnTimeInterval = 0.66f;
    public float smokeSpawnRadius = 0.25f;

    void Awake()
    {
        for (int i = 0; i < smokeCount; ++i)
            Invoke("CreateSmoke", i * smokeSpawnTimeInterval);
    }

    void CreateSmoke()
    {
        Instantiate(smokePrefab, transform.position +
            new Vector3(Random.Range(-smokeSpawnRadius, smokeSpawnRadius), Random.Range(-smokeSpawnRadius, smokeSpawnRadius), 0),
            Quaternion.Euler(0f, 0f, 0f));
    }
}
