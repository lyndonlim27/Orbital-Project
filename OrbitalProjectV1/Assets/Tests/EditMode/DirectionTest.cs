using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionTest
{

    // A Test behaves as an ordinary method
    [Test]
    public void North()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(new Vector3(0, 1, 0), Controller.north);
    }
}
