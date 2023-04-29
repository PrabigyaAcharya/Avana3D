using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public override void Die()
    {
        base.Die();
        PlayerManager.instance.KillPlayer();
    }
}
