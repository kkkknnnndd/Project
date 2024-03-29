using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Im_Di : MonoBehaviour
{
    public List<AttackSO> hit_State;
    public AudioClip HitSFX;
    private Animator anim;
    private bool ene_Attack = false;

    private AudioSource audioFx;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameObject tempchar = GameObject.FindGameObjectWithTag("Player");
        audioFx = tempchar.GetComponent<CharacterController>().GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ene_Attack = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && ene_Attack)
        {
            anim.runtimeAnimatorController = hit_State[0].aniOV;
            anim.Play("Impact", 0, 0);
            audioFx.PlayOneShot(HitSFX);
            ene_Attack = false;
        }
    }
}
