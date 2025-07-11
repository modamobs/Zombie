using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour {
    public string moveHorizontalAxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string moveVerticalAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string mouseXAxisName = "Mouse X"; // 마우스 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름

    // 값 할당은 내부에서만 가능
    public float moveHorizontal { get; private set; } // 감지된 좌우 움직임 입력값
    public float moveVertical { get; private set; } // 감지된 앞뒤 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값 (마우스)
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

    // 매프레임 사용자 입력을 감지
    private void Update() {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            moveHorizontal = 0;
            moveVertical = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        // 키보드 좌우 이동 입력 감지
        moveHorizontal = Input.GetAxis(moveHorizontalAxisName);
        // 키보드 앞뒤 이동 입력 감지
        moveVertical = Input.GetAxis(moveVerticalAxisName);
        // 마우스 좌우 회전 입력 감지
        rotate = Input.GetAxis(mouseXAxisName);
        // fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);
    }
}