using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    private PlayerBatterySystem batterySystem; // 배터리 시스템

    [SerializeField] private float moveSpeed = 5.0f; // 플레이어 이동 속도 (걷기 기준)
    [SerializeField] private float runMultiplier = 1.5f; // 달리기 속도 배율
    [SerializeField] private float jumpHeight = 2.0f; // 플레이어 점프 높이
    [SerializeField] private float gravityValue = -9.81f; // 중력 값
    [SerializeField] private float rotationSpeed = 10.0f; // 회전 속도

    private Vector3 playerVelocity; // 플레이어 속도 벡터

    private bool isGrounded; // 플레이어가 지면에 닿아있는지 여부

    void Start()
    {
        // 각종 컴포넌트 초기화
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        batterySystem = GetComponent<PlayerBatterySystem>();
    }

    void Update()
    {
        handleMovement();
    }

    /// <summary>
    /// 이동과 관련된 모든 처리를 담당하는 메서드
    /// </summary>
    private void handleMovement()
    {
        // 지면 체크
        isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            // 지면에 닿아있을 경우 속도를 약간의 음수값으로 설정하여 캐릭터가 지면에 붙어있도록
            playerVelocity.y = -2f;
        }

        // 수평 이동 입력 및 계산
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

            // 캐릭터 회전 처리 (이동 방향을 바라보도록)
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else // 정지 상태   
        {
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;

            batterySystem.TryConsume(ActionType.Idle);
        }

        // 점프 처리
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            animator.SetTrigger("jump");

            batterySystem.TryConsume(ActionType.Jump);
        }

        // 중력 적용
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime); // 최종 이동 적용

        // Animator 값 업데이트
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetFloat("verticalSpeed", playerVelocity.y);
    }
}
