using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public AttributesManager playerAtm;
    public AttributesManager enemyAtm;
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerAtm.DealDamage(enemyAtm.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            enemyAtm.DealDamage(playerAtm.gameObject);
        }
    }
}
