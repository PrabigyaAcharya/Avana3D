using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GamePlayTest
{
    
    [UnityTest]
    public IEnumerator Movement()
    {
        var gameObject = new GameObject();
        var player = gameObject.AddComponent<EnemyStat>();

        player.Respawn();
        yield return new WaitForSeconds(2);

        Assert.AreEqual(EnemyStat.respawnPosition, player.transform.position);        
    }
}
