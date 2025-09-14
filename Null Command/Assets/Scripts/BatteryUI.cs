using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private Slider batterySlider; // 배터리 슬라이더 UI
    [SerializeField] private PlayerBatterySystem batterySystem; // 배터리 시스템

    // 스크립트가 활성화될 때 이벤트 구독
    private void OnEnable()
    {
        batterySystem.OnBatteryChanged += UpdateUI;
    }

    // 스크립트가 비활성화될 때 이벤트 구독 해제
    void OnDisable()
    {
        batterySystem.OnBatteryChanged -= UpdateUI;
    }

    private void UpdateUI(float currentBattery, float maxBattery)
    {
        if (batterySlider == null) return;

        batterySlider.value = currentBattery / maxBattery;
    }
}
