using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test1
{
    // A Test behaves as an ordinary method
    [Test]
    public void Test1SimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual("ciao", "ciao");
    }

    [Test]
    public void Test2()
    {
        Assert.AreEqual("casa", "casa");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
}
