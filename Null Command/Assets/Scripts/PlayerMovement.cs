using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    private PlayerBatterySystem batterySystem; // ���͸� �ý���

    [SerializeField] private float moveSpeed = 5.0f; // �÷��̾� �̵� �ӵ� (�ȱ� ����)
    [SerializeField] private float runMultiplier = 1.5f; // �޸��� �ӵ� ����
    [SerializeField] private float jumpHeight = 2.0f; // �÷��̾� ���� ����
    [SerializeField] private float gravityValue = -9.81f; // �߷� ��
    [SerializeField] private float rotationSpeed = 10.0f; // ȸ�� �ӵ�

    private Vector3 playerVelocity; // �÷��̾� �ӵ� ����

    private bool isGrounded; // �÷��̾ ���鿡 ����ִ��� ����

    void Start()
    {
        // ���� ������Ʈ �ʱ�ȭ
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        batterySystem = GetComponent<PlayerBatterySystem>();
    }

    void Update()
    {
        handleMovement();
    }

    /// <summary>
    /// �̵��� ���õ� ��� ó���� ����ϴ� �޼���
    /// </summary>
    private void handleMovement()
    {
        // ���� üũ
        isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            // ���鿡 ������� ��� �ӵ��� �ణ�� ���������� �����Ͽ� ĳ���Ͱ� ���鿡 �پ��ֵ���
            playerVelocity.y = -2f;
        }

        // ���� �̵� �Է� �� ���
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        bool isMoving = inputDir.sqrMagnitude != 0f;
        bool isRunning = false;

        if (isMoving)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;

            playerVelocity.x = inputDir.x * currentSpeed;
            playerVelocity.z = inputDir.z * currentSpeed;

            batterySystem.TryConsume(isRunning ? ActionType.Running : ActionType.Walking);

            // ĳ���� ȸ�� ó�� (�̵� ������ �ٶ󺸵���)
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else // ���� ����   
        {
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;

            batterySystem.TryConsume(ActionType.Idle);
        }

        // ���� ó��
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            animator.SetTrigger("jump");

            batterySystem.TryConsume(ActionType.Jump);
        }

        // �߷� ����
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime); // ���� �̵� ����

        // Animator �� ������Ʈ
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetFloat("verticalSpeed", playerVelocity.y);
    }
}
