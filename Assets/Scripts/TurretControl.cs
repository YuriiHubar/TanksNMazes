using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour {
    public GameObject destroySoundObject;
    public GameObject destroyedPrefab;

    private AudioSource audioSource;
    public bool HasShot
    {
        get { return cannon.CanShoot; }
    }

    private CannonControl cannon;
    private GameController gameController;

    void Start()
    {
        cannon = transform.GetComponentInChildren<CannonControl>();
        audioSource = GetComponent<AudioSource>();
        gameController = GameController.instance;
    }


    // Access methods for tank controlling
    public void TurnCannonTo(ref float angle)
    {
        cannon.TurnTo(ref angle);
    }
   
    public bool TryFire()
    {
        if (!HasShot)       // Won't shoot if has "no ammo"
            return false;

        if (!cannon.Fire())
            return false;

        return true;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Projectile"))
        {
            if (gameController != null)
            {
                if (col.collider.gameObject.GetComponent<ShellBehaviour>().masterTag == "Player")                
                    gameController.Murder(gameObject);    // Add points only if it's Player work                                               
                else                
                    gameController.Death(gameObject);
                BlowUp(); // Make it blow up! (If no GameController, all the tanks are invincible!)
            }
        }
    }


    private void BlowUp()
    {
        Instantiate(destroySoundObject, transform.position, transform.rotation);
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // public utility methos
    public float GetOrientation(ref Vector2 target)
    {
        return Mathf.Atan2(-target.x, target.y) * Mathf.Rad2Deg;
    }
}
