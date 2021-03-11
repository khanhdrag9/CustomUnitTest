using Khite.AutomationTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTr : TestScript
{
    protected override IEnumerator OnRun()
    {
        Check(1 == 1);
        yield return null;
    }
}
