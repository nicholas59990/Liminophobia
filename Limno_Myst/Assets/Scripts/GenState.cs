using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GenState
{
    Idle,
    GeneratingRoom,
    GeneratingLight,
    GeneratingSpawnRoom,
    GeneratingExitRoom,
    GeneratingBarrier,
}