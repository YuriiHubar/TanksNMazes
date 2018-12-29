using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI tank control class. Type 0: random movenment, random shooting
/// </summary>
public class AITankControl0 : MonoBehaviour {
    public float minTurnMovenmentDistance = 2f;
    public float maxTurnMovenmentDistance = 5f;
    public float maxShotDelay = 2f;

    private TankControl tankControl;
    private GameObject player;

    private float timeToShoot = 0;
    void Start()
    {
        tankControl = GetComponent<TankControl>();
    }

    void Update()
    {
        if (!tankControl.IsMoving)
        {
            float angle = Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 movenmentVector = transform.position + rotation * Vector2.up * Random.Range(minTurnMovenmentDistance, maxTurnMovenmentDistance);
            tankControl.TryMoveAndTurnTo(ref movenmentVector);
            if(!tankControl.HasShot)
                timeToShoot = Time.time + Random.Range(0, maxShotDelay);
        }
        else if(tankControl.HasShot && Time.time > timeToShoot)
        {
            float angle = Random.Range(0, 360);
            tankControl.TurnCannonTo(ref angle);
            tankControl.TryFire();
        }
    }
}
