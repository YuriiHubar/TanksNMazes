using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    public GameObject ShellPrefab;
    public float coolDown = 1f;

    public AudioClip ShootSound;
    public float minVolume = 0.5f;
    public float maxVolume = 1.0f;

    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanShoot
    {
        get { return (lastShot + coolDown < Time.time); }
    }

    float lastShot = 0;
    public bool Fire()
    {
        if (!CanShoot)
            return false;

        audioSource.PlayOneShot(ShootSound, Random.Range(minVolume, maxVolume));

        GameObject shell = Instantiate(ShellPrefab, transform.Find("ShellSpawnPoint").transform.position, transform.rotation);
        shell.GetComponent<ShellSafiety>().master = this.GetComponentInParent<Collider2D>();
        shell.GetComponent<ShellBehaviour>().SetMaster(this.GetComponentInParent<Collider2D>());
        lastShot = Time.time;
        return true;
    }

    public void TurnTo(ref float angle)
    {
        Vector3 euAngles = transform.eulerAngles;
        euAngles.Set(0, 0, angle);
        transform.eulerAngles = euAngles;
    }
}

