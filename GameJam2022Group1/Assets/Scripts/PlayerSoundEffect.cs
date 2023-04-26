using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    CharacterController player;
    private AudioSource footStepsEff;
    public AudioClip JumpEff;
    public AudioClip CrouchEff;

    private bool footStepActive;
    private bool CrouchActive;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        footStepsEff = GetComponent<AudioSource>();
      
        footStepActive = false;
        footStepsEff.loop = false;
        //footStepsEff.enabled = false;

        CrouchActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isGrounded == true && footStepActive == false)
        {
            footStepsEff.Play();
            footStepActive = true;
            footStepsEff.loop = true;
            footStepsEff.volume = Random.Range(0.7f, 1.0f);
            footStepsEff.pitch = Random.Range(0.7f, 1.2f);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            footStepsEff.loop = false;
            footStepsEff.PlayOneShot(JumpEff);
            footStepActive = false;

        }
        if(Input.GetKeyDown(KeyCode.C) && CrouchActive == false)
        {
            footStepsEff.loop = false;
            footStepsEff.PlayOneShot(CrouchEff);
            footStepActive = false;
            CrouchActive = true;
        }
        else if(!(Input.GetKeyDown(KeyCode.C)))
        {
            CrouchActive = false;
        }



        if (DeathCheck.isDead)
        {
            footStepsEff.enabled = false;
        }
    }
}
