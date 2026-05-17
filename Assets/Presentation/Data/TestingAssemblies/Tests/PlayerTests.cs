using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerTests : InputTestFixture
{
    private GameObject playerObj;
    private Keyboard keyboard;
    private Mouse mouse;
    private bool ready = false;
    [SetUp]
    public override void Setup()
    {
        SceneManager.LoadScene(1);
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
        mouse = InputSystem.AddDevice<Mouse>();
        ready = true;
    }
    [UnityTest]
    public IEnumerator DoesPlayerExists()
    {
        if (!ready)
        {
            do
            {
                yield return null;
            } while (!ready);
        }
        yield return null;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerObj);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator WalkingTestWithEnumeratorPasses()
    {
        if (!ready)
        {
            do
            {
                yield return null;
            } while (!ready);
        }
        yield return null;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Vector3 startPos = playerObj.transform.position;
        yield return new WaitForSeconds(1);
        Press(keyboard.wKey);
        yield return new WaitForSeconds(1);
        Release(keyboard.wKey);
        yield return new WaitForSeconds(0.25f);
        Assert.AreNotEqual(startPos, playerObj.transform.position);
    }
    [TearDown]
    public void Teardown()
    {
        keyboard = null;
        mouse = null;
    }
}
