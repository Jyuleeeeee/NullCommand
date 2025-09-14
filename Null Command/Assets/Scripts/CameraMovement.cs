using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // ī�޶� ���� ��� ������Ʈ
    [SerializeField] private Vector3 offset = new Vector3(0f, 3.8f, -5.5f); // ī�޶�� ��� ������Ʈ ���� ������
    [SerializeField] private float smoothTime = 0.1f; // ī�޶� �̵��� �ε巯�� ���� (�������� ����)

    private Vector3 velocity = Vector3.zero; // ī�޶� �̵� �ӵ�

    private void Start()
    {
        transform.position = targetTransform.position + offset; // �ʱ� ��ġ ����   
    }

    private void LateUpdate()
    {
        Vector3 targetPos = targetTransform.position + offset; // ��ǥ ��ġ ���

        // ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); 
    }
}
