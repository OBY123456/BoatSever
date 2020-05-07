using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvent : MonoBehaviour
{

    public Animator animator;
    public AnimationClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        clips = animator.runtimeAnimatorController.animationClips;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndEvent()
    {
        animator.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        animator.SetTrigger("IsHide");
    }
}
