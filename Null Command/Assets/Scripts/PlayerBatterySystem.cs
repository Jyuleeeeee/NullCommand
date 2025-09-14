using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBatterySystem : MonoBehaviour
{
    [Header("배터리 설정")]
    [SerializeField] private float maxBattery = 100.0f; // 최대 배터리 양
    private float currentBattery; // 현재 배터리 양

    [Header("소모 설정")]
    [SerializeField] private List<ActionConsumption> consumptionRules; // 행동별 소모 규칙
    private Dictionary<ActionType, ActionConsumption> consumptionDict; // 빠른 조회를 위한 딕셔너리

    // 배터리 변화 시 호출되는 이벤트
    // 파라미터: (현재 배터리량, 최대 배터리량)
    public event Action<float, float> OnBatteryChanged;

    private void Start()
    {
        currentBattery = maxBattery; // 시작 시 최대 배터리로 설정

        consumptionDict = new Dictionary<ActionType, ActionConsumption>();
        foreach (var rule in consumptionRules) 
        {
            if (!consumptionDict.ContainsKey(rule.action)) 
            {
                consumptionDict.Add(rule.action, rule);
            }
        }

        // 게임 시작 시 UI 초기화를 위해 이벤트 호출
        OnBatteryChanged?.Invoke(currentBattery, maxBattery);
    }

    /// <summary>
    /// 외부에서 행동할 때 호출하는 배터리 소모 시도 메서드
    /// </summary>
    /// <returns> 소모에 성공하면 true, 실패하면 false </returns>
    public bool TryConsume(ActionType action)
    {
        if (!consumptionDict.ContainsKey(action))
        {
            Debug.LogWarning($"{action}에 대한 소모 규칙이 없습니다.");
            return false;
        }

        ActionConsumption data = consumptionDict[action];
        float cost = 0;

        // 소모 타입에 따른 소모량 계산
        if (data.consumptionType == ConsumptionType.Instant)
        {
            cost = data.value;
        }
        else if (data.consumptionType == ConsumptionType.Continuous)
        {
            // 지속 소모는 Time.deltaTime을 곱하여 어떤 환경에서도 일정한 소모가 되도록 함
            cost = data.value * Time.deltaTime;
        }

        // 배터리 잔량 확인 및 소모 처리
        if (currentBattery >= cost)
        {
            currentBattery -= cost;
            OnBatteryChanged?.Invoke(currentBattery, maxBattery);
            return true;
        }

        ////////////////////////////////////////////////////////////////////
        /// TODO: 배터리 부족 시 사망처리 등 추가 로직 필요
        ////////////////////////////////////////////////////////////////////
        return false; // 배터리 부족으로 소모 실패
    }

    /// <summary>
    /// 현재 배터리 양 확인용 getter
    /// </summary>
    public float GetCurrentBattery()
    {
        return currentBattery;
    }

    /// <summary>
    /// 충전소에서 배터리 충전 시 호출하는 메서드
    /// </summary>
    public void Charge()
    {
        currentBattery = maxBattery;
        OnBatteryChanged?.Invoke(currentBattery, maxBattery);
    }
}
