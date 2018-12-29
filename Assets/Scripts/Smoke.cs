using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {
    public float timeToLive = 5;

	// Use this for initialization
	void Start () {
        if (timeToLive > 0)
            Destroy(gameObject, timeToLive);
	}
}
