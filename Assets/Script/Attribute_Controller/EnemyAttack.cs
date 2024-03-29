using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerStats _playerStat;
    private void Start()
    {
        GameObject tempChar = GameObject.FindGameObjectWithTag("Player");
        _playerStat = tempChar.GetComponent<PlayerStats>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerStat.TakeDamage(GameObject.FindGameObjectWithTag("Enemy").GetComponent<Dragon>()._attackDamage);
        }
    }
}
