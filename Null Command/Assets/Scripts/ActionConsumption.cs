using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    // ���� �Ҹ� (�ʴ�)
    Idle,
    Walking,
    Running,

    // ���� �Ҹ� (1ȸ��)
    Jump,
    ShootLaser
}

public enum ConsumptionType
{
    // ���� �Ҹ� (�ʴ�)
    Continuous,
    // ���� �Ҹ� (1ȸ��)
    Instant
}

[System.Serializable]
public class ActionConsumption
{
    public ActionType action;
    public ConsumptionType consumptionType;
    public float value; // �ʴ� �Ҹ� �Ǵ� 1ȸ�� �Ҹ�
}
