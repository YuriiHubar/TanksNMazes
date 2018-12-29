using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSafiety : MonoBehaviour {
    // This entire script is needed ONLY beacuse of fact, that shell can (and in our case need)
    //   to be spawned INSIDE of the tank cannon so while shell will not leave
    //   his master, we don't want it to explode due to tanks collider
    //   But in case of ricochet implementing we want shell to be able
    //   to do damage to his owner AFTER it's left owner body.
    //
    // We can't use in this case trigger/collider enter/exit because sometimes 
    //   at some point spawning a shell will not fire a TriggerEnter and  
    //   and shellCollider.IsTouching(master) (maybe because at this point some 
    //   colliders were not processed by physics even 1 time so there is no detected
    //   collision)
    // My solution is to not even touch a safiety untill physics was computed at least 1 time.
    //   So we listen to FixedUpdate() and after it was run we begin to check a collisions with master.
    //   If there no such, then set shell safiety off ane re-enable normal behaviour script.
    //   (at this point shell left a tank body and is either in the air, or colliding with something else,
    //   what will be detected by normal shell behaviour script.)

    public Collider2D master;           // Shell owner collider

    private ShellBehaviour b;           // Other script
    private Collider2D shellCollider;   // Shell own collider
    private bool preSafiety = true; 

    // Use this for initialization
    void Start ()
    {
        //Debug.Log("===========");
        b = GetComponent<ShellBehaviour>();
        shellCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        preSafiety = false;
    }

    void Update()
    {
        if (preSafiety)
            return;

        if (shellCollider.IsTouching(master))
        {
            //Debug.Log("Safiety Update: touching");
        }
        else
        {
            //Debug.Log("Safiety Update: not touching");
            SafietyOff();
        }
    }

    private void SafietyOff()
    {
        //Debug.Log("Safiety off!");
        shellCollider.isTrigger = false;
        b.safiety = false;
        b.enabled = true;

        Destroy(this);      // TriggerEnter will not fire now 
    }

    // This is still needed in case of near object 
    // (case when shell entered an another object before safiety was set off)
    // Solution is simply blow up it immediately (still with safiety on, we don't care)
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col == master)
            return;
        if (col.gameObject.tag == "MovenmentTarget")
            return;

        //SafietyOff(); // Shell will ricochet   
        //b.Explode();  // Doesn't work well if bot is shooting close into a wall - suicide     

        Destroy(this);  // If shell is on safiety - delete it silently without effects
        Destroy(gameObject);
    }
}
