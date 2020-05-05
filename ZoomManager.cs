using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2019.11.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  카메라에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> MainCamera
 */

public class ZoomManager : MonoBehaviour
{
    //상수
    const float MAX_CMR_MOV = 0.5f; // Zoom 시 y축으로 움지이는 최대값 
    const float MAX_ZOOM = 3.5f;    // Zoom의 최대 값
    const float MOV_SPEED = 0.005f; // Zoom 속도
    const float AUTO_SPEED = 8f;   // autoZoom 속도

    //변수
    Camera cmr;
    float deltaMagDiff = 0f;        // Zoom 값

    void Start()
    {
        cmr = Camera.main;
    }

    void Update()
    {
        if (Time.timeScale.Equals(1))
            PinchTouch();
    }


    //두 손가락 제스처를 통해 줌 인/줌 아웃
    void PinchTouch()
    {
#if UNITY_EDITOR
        //Zoom_in   : 방향키▲
        if (Input.GetKey(KeyCode.UpArrow)) {
            deltaMagDiff -= 4f;
            Zoom(deltaMagDiff);
        }
        //Zoom_out  : 방향키▼
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaMagDiff += 4f;
            Zoom(deltaMagDiff);
        }
#elif UNITY_ANDROID
        //Pinch Zoom : 두 손가락을 이용해 화면 확대/축소
        if (Input.touchCount == 2 && (
            Input.GetTouch(0).phase == TouchPhase.Moved ||
            Input.GetTouch(1).phase == TouchPhase.Moved))
        {
            //두 손가락 입력 받아오기
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            //두 손가락의 움직이기 전 위치 구하기
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            //움직이기 전과 후의 두 손가락 사이 거리 구하기
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            //Zoom값 설정
            deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

            //일정 이상의 움직임에만 반응하기
            if (deltaMagDiff > 3f || deltaMagDiff < -3f)
                Zoom(deltaMagDiff);
        }
#endif
    }

    public void Zoom(float degree)
    {
        //카메라 줌 : orthographicSize = 1.5 ~ 5
        cmr.orthographicSize += degree * MOV_SPEED;
        float zoomSize = cmr.orthographicSize;
        if (zoomSize > 5f)
            zoomSize = 5f;
        else if (zoomSize < 5f - MAX_ZOOM)
            zoomSize = 5f - MAX_ZOOM;
        cmr.orthographicSize = zoomSize;

        //카메라 높이 수정
        float cmrPosY = (zoomSize - 5f) * MAX_CMR_MOV / MAX_ZOOM;
        cmr.transform.position = new Vector3(0, cmrPosY, 10);
    }
}
