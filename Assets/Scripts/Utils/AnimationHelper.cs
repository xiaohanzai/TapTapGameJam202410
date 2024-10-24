using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationHelper
{
    public static float GetAnimationLength(Animator animator, string animationName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator or RuntimeAnimatorController is missing.");
            return 0f;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation not found: " + animationName);
        return 0f; // Return 0 if the animation is not found
    }
}
