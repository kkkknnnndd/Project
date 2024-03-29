using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    public int HP;
    private int _souls;
    public Slider healthBar;
    public Animator animator;
    public BoxCollider _boxColli;
    public int _attackDamage;
    private bool IsDead = false;
    private PlayerStats _playerStats;
    private bool _isAddSoul = false;
    private void Start()
    {
        _boxColli.enabled = false;
        GameObject tempchar = GameObject.FindGameObjectWithTag("Player");
        _playerStats = tempchar.GetComponent<PlayerStats>();
        _souls = HP;
    }
    private void Update()
    {
        healthBar.value = HP;
        if (HP <= 0 && !IsDead)
        {
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            _boxColli.enabled = false;
            IsDead = true;
            _isAddSoul = true;
        }
        else if (HP <= 0 && IsDead)
        {
            _playerStats.HealHP();
            if(_isAddSoul)
            {
                _playerStats.AddSouls(_souls);
                _isAddSoul = false;
            }
        }
    }

    public void TakeDamage1(int damageAmount)
    {
        if(HP > 0)
        {
            HP -= damageAmount;
            animator.SetTrigger("damage");
        }
    }

    public void enableColliD(float num)
    {
        if(num == 1)
        {
            _boxColli.enabled = true;
        }
        else
        {
            _boxColli.enabled = false;
        }
    }    
}
