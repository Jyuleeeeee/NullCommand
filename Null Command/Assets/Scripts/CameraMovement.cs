using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // 카메라가 따라갈 대상 오브젝트
    [SerializeField] private Vector3 offset = new Vector3(0f, 3.8f, -5.5f); // 카메라와 대상 오브젝트 간의 오프셋
    [SerializeField] private float smoothTime = 0.1f; // 카메라 이동의 부드러움 정도 (작을수록 빠름)

    private Vector3 velocity = Vector3.zero; // 카메라 이동 속도

    private void Start()
    {
        transform.position = targetTransform.position + offset; // 초기 위치 설정   
    }

    private void LateUpdate()
    {
        Vector3 targetPos = targetTransform.position + offset; // 목표 위치 계산

        // 현재 위치에서 목표 위치로 부드럽게 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); 
    }
}
