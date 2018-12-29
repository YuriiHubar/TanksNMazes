using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour {

    public float flySpeed = 10;
    public float timeToLive = 1;
    public bool safiety = true;
    public Collider2D master;
    public string masterTag;
    public GameObject smokePrefab;

    public AudioClip HitSound;
    public float minVolume = 0.5f;
    public float maxVolume = 1.0f;

    private Rigidbody2D rb2d;
    private AudioSource audioSource;
    private bool exploding = false;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(transform.up * flySpeed, ForceMode2D.Impulse);
        audioSource = GetComponent<AudioSource>();
        Invoke("Explode", timeToLive);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (safiety)
            return;
        FinishMovenment();
        Explode();
    }

    public void SetMaster(Collider2D master)
    {
        this.master = master;
        this.masterTag = master.gameObject.tag;

    }

    private void FinishMovenment()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
    }

    public void Explode()
    {
        if (exploding)
            return;
        exploding = true;

        audioSource.PlayOneShot(HitSound, Random.Range(minVolume, maxVolume));
        FinishMovenment();
        GetComponent<Collider2D>().enabled = false;
        Animator a = GetComponent<Animator>();
        a.SetTrigger("Explode");
        // Animator will invoke a DestroyObject(); in case of Explode trigger
    }

    void DestroyObject()
    {
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, HitSound.length);
        Instantiate(smokePrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
    }
}
