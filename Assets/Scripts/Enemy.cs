using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStat))]
public class Enemy : InteractableObject
{
    PlayerManager playerManager;
    CharacterStat myStat;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        myStat = GetComponent<CharacterStat>();
    }
    public override void Interact()
    {
        base.Interact();
        Combat playerCombat = playerManager.Player.GetComponent<Combat>();
        if (playerCombat != null)
        {
            playerCombat.Attack(myStat);
        }
    }
}
