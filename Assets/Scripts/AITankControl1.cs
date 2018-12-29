using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AI tank control class. Type 1: just moving and shooting towards player.
/// No path finding.
/// </summary>
public class AITankControl1 : MonoBehaviour {
    public float movenmentInaccuracy = 3f;
    public float shootingInaccuracy = 20f;

    enum EnemyType
    {
        Player = 0,
        Tank = 1,
        Turret = 2,
        None = 3
    };

    private TankControl tankControl;
    private GameObject currentEnemy = null;
    private EnemyType currentEnemyType = EnemyType.Player;

    void Start()
    {        
        tankControl = GetComponent<TankControl>();
        GetEnemy();

    }

    void Update()
    {

        if (!tankControl.HasShot && tankControl.IsMoving) // No shot and moving - wait 'till stop
            return;

        GetEnemy();
        if (currentEnemyType == EnemyType.None)
        {
            this.enabled = false;
            return;
        }

        Vector2 enemyPosition = currentEnemy.transform.position;
        Vector2 enemyRelativePosition = enemyPosition - (Vector2)transform.position;
        float newOrientation = tankControl.GetOrientation(ref enemyRelativePosition);
        newOrientation += Random.Range(-shootingInaccuracy, shootingInaccuracy);

        enemyPosition += new Vector2(
            Random.Range(-movenmentInaccuracy, movenmentInaccuracy), 
            Random.Range(-movenmentInaccuracy, movenmentInaccuracy));

        if (tankControl.HasShot)
        {
            tankControl.TurnCannonTo(ref newOrientation);
            tankControl.TryFire();
        }
        if (!tankControl.IsMoving)
            tankControl.TryMoveAndTurnTo(ref enemyPosition);       
    }

    private void GetEnemy()
    {
        if (currentEnemy != null)
            return;
        if (currentEnemyType == EnemyType.None)
            return;

        while (currentEnemyType != EnemyType.None &&!GetEnemyOfCurrentType())
            ++currentEnemyType;
        
    }

    private bool GetEnemyOfCurrentType()
    {
        GameObject[] possibleEnemies = GameObject.FindGameObjectsWithTag(currentEnemyType.ToString());
        foreach (GameObject possibleEnemie in possibleEnemies)
        {
            if (possibleEnemie == this.gameObject)
                continue;
            currentEnemy = possibleEnemie;
            return true;
        }
        return false;
    }
}
