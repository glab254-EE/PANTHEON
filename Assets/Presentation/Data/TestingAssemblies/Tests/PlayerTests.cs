using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class PlayerTests : InputTestFixture
{
    private GameObject playerObj;
    private Keyboard keyboard;
    private Mouse mouse;
    [SetUp]
    public void SetUp()
    {
        keyboard = InputSystem.AddDevice<Keyboard>();
        mouse = InputSystem.AddDevice<Mouse>();
    }
    [Test]
    public void DoesPlayerExists()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerObj);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator WalkingTestWithEnumeratorPasses()
    {
        if (playerObj == null) DoesPlayerExists();
        Vector3 startPos = playerObj.transform.position;
        Assert.IsNotNull(playerObj);
        yield return new WaitForSeconds(1);
        Press(keyboard.wKey);
        yield return new WaitForSeconds(1);
        Release(keyboard.wKey);
        Assert.AreNotEqual(startPos, playerObj.transform.position);
    }
    [TearDown]
    public void Teardown()
    {
        keyboard = null;
        mouse = null;
    }
}
