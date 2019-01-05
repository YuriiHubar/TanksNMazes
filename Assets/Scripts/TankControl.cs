using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Interface for tank control either by player or AI
/// </summary>
public class TankControl : MonoBehaviour
{
    public AudioClip MoveSound;
    public float MoveSoundVolume = 0.15f;

    public GameObject destroySoundObject;

    public float movenmentSpeed = 3f;
    public float maxMovenmentDuration = 5f;
    public GameObject movenmentTargetPrefab;
    public GameObject destroyedPrefab;

    private AudioSource audioSource;

    public bool IsMoving
    {
        get { return isMoving; }
    }
    public bool HasShot
    {
        get { return hasShot && cannon.CanShoot; }
    }


    private bool isMoving = false;
    private bool stopAfterCollision = true; 
    private bool hasShot = true;
    private float endMovenment;

    private CannonControl cannon;
    private Rigidbody2D rb2d;
    private Collider2D bodyCollider;
    private GameObject movenmentTarget;
    private Collider2D movenmentTargetTrigger;

    private GameController gameController;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        cannon = transform.GetComponentInChildren<CannonControl>();
        movenmentTarget = Instantiate(movenmentTargetPrefab, transform.position, transform.rotation);
        movenmentTargetTrigger = movenmentTarget.GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        gameController = GameController.instance;
    }

    // Access methods for tank controlling

    /// <summary>
    /// Turns a cannon facing towards a direction
    /// </summary>
    /// <param name="newOrientation"></param>
    public void TurnCannonTo(ref float angle)
    {
        cannon.TurnTo(ref angle);
    }

    /// <summary>
    /// Shots or not according to game mechanics constraints
    /// </summary>
    public bool TryFire()
    {
        if (!isMoving)      // Won't shoot if not moving
            return false;
        if (!HasShot)       // Won't shoot if has "no ammo"
            return false;

        if (!cannon.Fire())
            return false;

        hasShot=false;
        return true;
    }


    /// <summary>
    /// Sets movenment target, movenment vector and rotation according to movenment vector
    /// </summary>
    /// <param name="target"></param>
    public void TryMoveAndTurnTo(ref Vector2 target)
    {
        if (isMoving)
            return;             // No new movenment if tank still moving

        Vector2 relativeTarget = target - (Vector2)transform.position;
        float newOrientation = GetOrientation(ref relativeTarget);
        SetMovenmentTarget(ref target);

        TurnTo(ref newOrientation);

        MoveTo(ref relativeTarget);
        hasShot = true;
    }


    public void BlowUp()
    {
        Instantiate(destroySoundObject, transform.position, transform.rotation);
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    // Private methods

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col == movenmentTargetTrigger)
            FinishMovenment();       
    }

    // Can occur in case if target set was already in the tank collider
    void OnTriggerStay2D(Collider2D col)
    {
        //if (col == movenmentTargetTrigger)
        //    FinishMovenment();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Obstacle"))
        {
            if (transform.InverseTransformPoint(col.contacts[0].point).y < 0)
                stopAfterCollision = false;
            return;
        }

        if (col.collider.CompareTag("Projectile"))
        {
            if (gameController != null)
            {
                if (col.collider.gameObject.GetComponent<ShellBehaviour>().masterTag == "Player")
                    gameController.Murder(gameObject);    // Add points only if it's Player work                                               
                else
                    gameController.Death(gameObject);

                BlowUp(); // Make it blow up!
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (stopAfterCollision)        
            FinishMovenment();
        else
            stopAfterCollision = true;
        
    }


    /// <summary>
    /// Stops any movenment
    /// </summary>
    private void FinishMovenment()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        isMoving = false;
        movenmentTarget.SetActive(false);
        //controlsOff = false;
    }

    /// <summary>
    /// Sets trigger position for movenment interruption when movenment target is reached
    /// </summary>
    /// <param name="target">Trigger position in world coordinates</param>
    private void SetMovenmentTarget(ref Vector2 target)
    {
        movenmentTarget.SetActive(true);
        movenmentTarget.transform.position = target;
    }

    /// <summary>
    /// Sets a velocity to RB in target direction
    /// </summary>
    /// <param name="target">Place where tank will be moving towards (in local coords)</param>
    private void MoveTo(ref Vector2 target)
    {
        rb2d.AddForce(target.normalized * movenmentSpeed, ForceMode2D.Impulse);
        isMoving = true;
        audioSource.PlayOneShot(MoveSound, MoveSoundVolume);

        if (maxMovenmentDuration > 0)
        {
            endMovenment = Time.time + maxMovenmentDuration;
            Invoke("FinishMovenmentByTimer", maxMovenmentDuration + 0.1f);
        }
    }

    private void FinishMovenmentByTimer()
    {
        if (!isMoving)                                  // If we're already not moving, then this isn't needed
            return;

        float timeLeft = endMovenment - Time.time;  // Can be invoked before it was needed, let's check.

        if (timeLeft < 0)     // If function was invoked before time needed or we're in the middle of collision
        {
            FinishMovenment();
            return;
        }
        
        Invoke("FinishMovenmentByTimer", timeLeft + 0.1f);  // Call it a bit late as schedulled
    }

    /// <summary>
    /// Turns a tank facing towards a direction
    /// </summary>
    /// <param name="angle">Direction (z-part of rotation)</param>
    private void TurnTo(ref float angle)
    {
        Vector3 euAngles = transform.eulerAngles;
        euAngles.Set(0, 0, angle);
        transform.eulerAngles = euAngles;
    }


    // public utility methos


    public float GetOrientation(ref Vector2 target)
    {
        return Mathf.Atan2(-target.x, target.y) * Mathf.Rad2Deg;
    }
}
