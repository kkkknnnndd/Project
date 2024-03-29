using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep_Control : MonoBehaviour
{
    private AudioSource SFXSource;

    [Header("---------W.R Audio Clip---------")]
    public List<AudioClip> walk;
    public List<AudioClip> run;
    public AudioClip dodge;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        CharacterController controller = temp.GetComponent<CharacterController>();
        SFXSource = temp.GetComponent<AudioSource>();
    }
    public void PlayFootStepSound(float num)
    {
        if(num == 0)
        {
            SFXSource.PlayOneShot(walk[Random.Range(0, walk.Count)]);
        }    
        else
        {
            SFXSource.PlayOneShot(run[Random.Range(0, run.Count)]);
        }    
        
    }

    public void PlayDodgeSound()
    {
        SFXSource.PlayOneShot(dodge);
    }    
}
