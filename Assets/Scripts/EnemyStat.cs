using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStat
{

    public static Vector3 respawnPosition = new Vector3(6, 0, -9);
    public override void Die()
    {
        base.Die();
        Respawn();
        
    }

    public override void Respawn()
    {
        base.Respawn();
        transform.position = respawnPosition;
    }
}
