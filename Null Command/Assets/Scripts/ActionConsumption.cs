using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    // 지속 소모 (초당)
    Idle,
    Walking,
    Running,

    // 순간 소모 (1회성)
    Jump,
    ShootLaser
}

public enum ConsumptionType
{
    // 지속 소모 (초당)
    Continuous,
    // 순간 소모 (1회성)
    Instant
}

[System.Serializable]
public class ActionConsumption
{
    public ActionType action;
    public ConsumptionType consumptionType;
    public float value; // 초당 소모량 또는 1회성 소모량
}
