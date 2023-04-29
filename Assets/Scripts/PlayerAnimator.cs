using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : CharacterAnimator
{
    public WeaponAnimations[] weaponAnimations;

    protected override void Start()
    {
        base.Start();
        currentAttackAnimationSet = defaultAttackAnimationSet;
    }

    [System.Serializable]
    public struct WeaponAnimations
    {
        public AnimationClip[] clips;
    }
}
