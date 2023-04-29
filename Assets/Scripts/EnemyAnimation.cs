using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : CharacterAnimator
{
    protected override void Start()
    {
        base.Start();
        currentAttackAnimationSet = defaultAttackAnimationSet;
    }
}
