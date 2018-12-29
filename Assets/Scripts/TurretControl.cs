using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour {
    public AudioClip DestroySound;
    public float DestroySoundVolume = 0.33f;

    public GameObject destroyedPrefab;

    private AudioSource audioSource;
    public bool HasShot
    {
        get { return cannon.CanShoot; }
    }

    private CannonControl cannon;
    private GameController gameController;
    private bool controlsOff = true;

    void Start()
    {
        cannon = transform.GetComponentInChildren<CannonControl>();
        audioSource = GetComponent<AudioSource>();
        gameController = GameController.instance;
        controlsOff = false;
    }


    // Access methods for tank controlling
    public void TurnCannonTo(ref float angle)
    {
        if (controlsOff)    // Case for example when tank was blown up and before it's destroyed, cannon still works
            return;
        cannon.TurnTo(ref angle);
    }
   
    public bool TryFire()
    {
        if (controlsOff)    // Case for example when turret was blown up and before it's destroyed, cannon still works
            return false;
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
        controlsOff = true;
        GetComponent<Collider2D>().enabled = false;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
            r.enabled = false;

        audioSource.PlayOneShot(DestroySound, DestroySoundVolume);

        Destroy(gameObject, DestroySound.length);
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
    }

    // public utility methos
    public float GetOrientation(ref Vector2 target)
    {
        return Mathf.Atan2(-target.x, target.y) * Mathf.Rad2Deg;
    }
}
