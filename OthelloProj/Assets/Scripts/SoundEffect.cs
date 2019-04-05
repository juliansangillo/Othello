using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        AudioSource audio = animator.GetComponentInParent(typeof(AudioSource)) as AudioSource;

        audio.Play();

    }

}
