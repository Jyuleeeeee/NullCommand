using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBatterySystem : MonoBehaviour
{
    [Header("���͸� ����")]
    [SerializeField] private float maxBattery = 100.0f; // �ִ� ���͸� ��
    private float currentBattery; // ���� ���͸� ��

    [Header("�Ҹ� ����")]
    [SerializeField] private List<ActionConsumption> consumptionRules; // �ൿ�� �Ҹ� ��Ģ
    private Dictionary<ActionType, ActionConsumption> consumptionDict; // ���� ��ȸ�� ���� ��ųʸ�

    // ���͸� ��ȭ �� ȣ��Ǵ� �̺�Ʈ
    // �Ķ����: (���� ���͸���, �ִ� ���͸���)
    public event Action<float, float> OnBatteryChanged;

    private void Start()
    {
        currentBattery = maxBattery; // ���� �� �ִ� ���͸��� ����

        consumptionDict = new Dictionary<ActionType, ActionConsumption>();
        foreach (var rule in consumptionRules) 
        {
            if (!consumptionDict.ContainsKey(rule.action)) 
            {
                consumptionDict.Add(rule.action, rule);
            }
        }

        // ���� ���� �� UI �ʱ�ȭ�� ���� �̺�Ʈ ȣ��
        OnBatteryChanged?.Invoke(currentBattery, maxBattery);
    }

    /// <summary>
    /// �ܺο��� �ൿ�� �� ȣ���ϴ� ���͸� �Ҹ� �õ� �޼���
    /// </summary>
    /// <returns> �Ҹ� �����ϸ� true, �����ϸ� false </returns>
    public bool TryConsume(ActionType action)
    {
        if (!consumptionDict.ContainsKey(action))
        {
            Debug.LogWarning($"{action}�� ���� �Ҹ� ��Ģ�� �����ϴ�.");
            return false;
        }

        ActionConsumption data = consumptionDict[action];
        float cost = 0;

        // �Ҹ� Ÿ�Կ� ���� �Ҹ� ���
        if (data.consumptionType == ConsumptionType.Instant)
        {
            cost = data.value;
        }
        else if (data.consumptionType == ConsumptionType.Continuous)
        {
            // ���� �Ҹ�� Time.deltaTime�� ���Ͽ� � ȯ�濡���� ������ �Ҹ� �ǵ��� ��
            cost = data.value * Time.deltaTime;
        }

        // ���͸� �ܷ� Ȯ�� �� �Ҹ� ó��
        if (currentBattery >= cost)
        {
            currentBattery -= cost;
            OnBatteryChanged?.Invoke(currentBattery, maxBattery);
            return true;
        }

        ////////////////////////////////////////////////////////////////////
        /// TODO: ���͸� ���� �� ���ó�� �� �߰� ���� �ʿ�
        ////////////////////////////////////////////////////////////////////
        return false; // ���͸� �������� �Ҹ� ����
    }

    /// <summary>
    /// ���� ���͸� �� Ȯ�ο� getter
    /// </summary>
    public float GetCurrentBattery()
    {
        return currentBattery;
    }

    /// <summary>
    /// �����ҿ��� ���͸� ���� �� ȣ���ϴ� �޼���
    /// </summary>
    public void Charge()
    {
        currentBattery = maxBattery;
        OnBatteryChanged?.Invoke(currentBattery, maxBattery);
    }
}
