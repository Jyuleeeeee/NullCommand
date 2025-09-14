using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private Slider batterySlider; // ���͸� �����̴� UI
    [SerializeField] private PlayerBatterySystem batterySystem; // ���͸� �ý���

    // ��ũ��Ʈ�� Ȱ��ȭ�� �� �̺�Ʈ ����
    private void OnEnable()
    {
        batterySystem.OnBatteryChanged += UpdateUI;
    }

    // ��ũ��Ʈ�� ��Ȱ��ȭ�� �� �̺�Ʈ ���� ����
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
