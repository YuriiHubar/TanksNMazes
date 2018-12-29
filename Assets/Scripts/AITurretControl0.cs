using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI tank control class. Type 0: random shooting
/// </summary>
public class AITurretControl0 : MonoBehaviour {
    public float maxShotDelay = 2f;

    private TurretControl turretControl;

    private float timeToShoot = 0;
    void Start()
    {
        turretControl = GetComponent<TurretControl>();
    }

    void Update()
    {
        if(turretControl.HasShot && Time.time > timeToShoot)
        {
            
            float angle = Random.Range(0, 360);
            turretControl.TurnCannonTo(ref angle);
            if(turretControl.TryFire())
                timeToShoot = Time.time + Random.Range(0, maxShotDelay);
        }
    }
}
