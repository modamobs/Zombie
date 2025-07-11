using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Camera playerCamera; // 플레이어 카메라 참조

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        
        // 메인 카메라 참조 가져오기
        playerCamera = Camera.main;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 회전 실행
        Rotate();
        // 움직임 실행
        Move();

        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경 (이동 속도의 크기 사용)
        float moveAmount = new Vector2(playerInput.moveHorizontal, playerInput.moveVertical).magnitude;
        playerAnimator.SetFloat("Move", moveAmount);
    }

    // 입력값에 따라 캐릭터를 상하좌우로 움직임 (카메라 방향 기준)
    private void Move() {
        // 카메라가 없으면 이동하지 않음
        if (playerCamera == null) return;
        
        // 카메라의 forward와 right 벡터를 Y축 제거하여 평면상의 방향만 사용
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        
        // Y축 성분 제거 (평면 이동만)
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        // 정규화
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // 카메라 방향 기준으로 이동 거리 계산
        Vector3 moveVertical = playerInput.moveVertical * cameraForward * moveSpeed * Time.deltaTime;
        Vector3 moveHorizontal = playerInput.moveHorizontal * cameraRight * moveSpeed * Time.deltaTime;
        
        // 최종 이동 거리 = 앞뒤 이동 + 좌우 이동
        Vector3 moveDistance = moveVertical + moveHorizontal;
        
        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 마우스 위치를 기준으로 캐릭터 회전
    private void Rotate() {
        // 카메라가 없으면 회전하지 않음
        if (playerCamera == null) return;
        
        // 플레이어 위치를 스크린 좌표로 변환
        Vector3 playerScreenPosition = playerCamera.WorldToScreenPoint(transform.position);
        
        // 마우스 스크린 좌표 가져오기
        Vector3 mouseScreenPosition = Input.mousePosition;
        
        // 스크린 좌표에서 방향 벡터 계산
        Vector2 direction2D = new Vector2(
            mouseScreenPosition.x - playerScreenPosition.x,
            mouseScreenPosition.y - playerScreenPosition.y
        );
        
        // 방향 벡터가 유효한 경우에만 회전
        if (direction2D.magnitude > 0.001f)
        {
            // 스크린 좌표의 방향을 월드 좌표 방향으로 변환
            Vector3 worldDirection = new Vector3(direction2D.x, 0, direction2D.y);
            
            // 카메라 방향 고려하여 월드 방향으로 변환
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            
            // Y축 성분 제거
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            // 최종 방향 계산
            Vector3 finalDirection = cameraRight * direction2D.x + cameraForward * direction2D.y;
            finalDirection.y = 0;
            
            // 마우스 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
            playerRigidbody.rotation = targetRotation;
        }
    }
}