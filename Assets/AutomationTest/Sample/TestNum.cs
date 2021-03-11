using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khite.AutomationTest;
using NUnit.Framework;

public class TestNum : TestScript
{
    protected override IEnumerator OnRun()
    {
        Check(1 == 1);
        yield return null;
    }
}
