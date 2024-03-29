using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttribute : MonoBehaviour
{
    public AttributesManager atm;
    private AudioManager audioManager;
    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy(transform.GetComponent<Rigidbody>());
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Dragon>().TakeDamage1(PlayerStats._attackD);
            audioManager.PlaySFX(audioManager.enemyImpact);
        }    
    }
}
