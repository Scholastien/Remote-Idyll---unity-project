using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;

public class PlayMode_SingletonMonobehaviour
{
    [UnityTest]
    // [MethodUnderTest]_[Scenario]_[ExpectedResult]
    // ShouldThrowIfAnUnknownTypeIsProvided()
    public IEnumerator Instance_OnePlayer_InstanceNotNull()
    {
        GameObject go = new GameObject("mock");
        var mock = go.AddComponent<Player>();
        mock.gameObject.AddComponent<Rigidbody2D>();
        yield return null;
        Assert.IsNotNull(Player.Instance, "Player isn't accessible through singleton Instance");
    }

    [UnityTest]
    public IEnumerator Instance_TwoPlayers_OnlyOneInstance()
    {
        bool isInstantiated = Player.Instance != null;

        GameObject go1 = new GameObject("p1");
        go1.name = "p1";
        Player player1 = go1.AddComponent<Player>();

        GameObject go2 = null;
        Player player2 = null;
        if (Player.Instance == player1)
        {
            go2 = new GameObject("p2");
            go2.name = "p2";
            player2 = go2.AddComponent<Player>();
        }

        yield return null;

        bool p1 = (player1 != Player.Instance && Player.Instance != null && player1 == null);
        bool p2 = (player2 != Player.Instance && player1 != null && player2 == null && Player.Instance == player1);
        bool assert = (p1 && !p2) ^ (!p1 && p2);

        Assert.IsTrue(assert, "Player gameObject cannot be instantiated \ngo1 = " + go1 + "\ngo2 = " + go2 + "\nPlayer.Instance is " + Player.Instance);





    }
}
