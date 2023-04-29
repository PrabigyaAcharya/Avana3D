using NUnit.Framework;
using UnityEngine;

public class SpawnTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void RespawnPoint()
    {
        Assert.AreEqual(new Vector3(6, 0, -9), EnemyStat.respawnPosition);
    }

}
