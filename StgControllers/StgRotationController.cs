using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Date     : 2020.05.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  스테이지 오브젝트의 회전을 담당하는 스크립트
 */
[RequireComponent(typeof(SphereCollider))]
public class StgRotationController : MonoBehaviour {

    //상수
    private const float ROT_SPEED = 80;

    //변수
    private float angle = 0f;                   //스테이지 회전 각도

    //접근자
    protected float Angle {
        get { return angle;  }
    }

    //가상함수
    protected virtual void Active() { }

#if UNITY_EDITOR
    //회전 이벤트....PC환경에서 회전 테스트 용
    protected void Update()
    {
        if (Time.timeScale.Equals(1))
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.up, 0.3f);
                angle += 0.3f;
                if (angle < 0) angle += 360;
                angle %= 360;

                Active();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.down, 0.3f);
                angle -= 0.3f;
                if (angle < 0) angle += 360;
                angle %= 360;

                Active();
            }
    }

#elif UNITY_ANDROID
    //회전 이벤트 ... stage오브젝트에 콜라이더 씌워야 함
    protected void OnMouseDrag()
    {
        if (Time.timeScale.Equals(1) && 
            Input.touchCount == 1 &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //좌우로 드래그한 만큼 회전
            float rotX = Input.GetAxis("Mouse X") * ROT_SPEED * Mathf.Deg2Rad * (-1);
            transform.Rotate(Vector3.up, rotX);

            //각도 계산
            angle += rotX;
            if (angle < 0) angle += 360;
            angle %= 360;

            Active();
        }
    }
#endif
}
