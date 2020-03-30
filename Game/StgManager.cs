﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/**
 * Date     : 2019.11.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  스테이지에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> cube
 */

public class StgManager : MonoBehaviour{

    public GameObject c1, c2, c3, c4;   //벽 오브젝트

    //상수
    const float ROT_SPEED = 80;

    //변수
    float angle = 0f;                   //스테이지 회전 각도
    bool play = true;                   //스테이지 회전 가능 여부

    //공개 메소드
    //스테이지 회전 가능 여부 설정자
    public void setPlay(bool play)
    {
        this.play = play;
    }

    //회전 이벤트 ... stage오브젝트에 콜라이더 씌워야 함
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnMouseDrag()
    {
        if (play && Input.touchCount == 1 &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //좌우로 드래그한 만큼 회전
            float rotX = Input.GetAxis("Mouse X") * ROT_SPEED * Mathf.Deg2Rad * (-1);
            transform.Rotate(Vector3.up, rotX);

            //각도 계산
            angle += rotX;
            if (angle < 0) angle += 360;
            angle %= 360;

            //안쪽 벽만 보이게 하기
            if (angle > 45 && angle < 135)
                Inside(c1, c2, c3, c4);
            else if (angle > 135 && angle < 225)
                Inside(c2, c3, c4, c1);
            else if (angle > 225 && angle < 315)
                Inside(c3, c4, c1, c2);
            else if (angle > 315 || angle < 45)
                Inside(c4, c1, c2, c3);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    void Inside(GameObject f1, GameObject f2, GameObject b1, GameObject b2) {
        if (!b1.activeSelf || !b2.activeSelf) {
            f1.SetActive(false);
            f2.SetActive(false);
            b1.SetActive(true);
            b2.SetActive(true);
        }
    }

    // 스테이지 자동회전.... PC환경에서 회전 테스트 용
    //void Update()
    //{
    //    float rotX = 0.3f;
    //    transform.Rotate(Vector3.up, rotX);
    //    angle += rotX;
    //    if (angle < 0) angle += 360;
    //    angle %= 360;

    //    if (angle > 45 && angle < 135)
    //        Inside(c1, c2, c3, c4);
    //    else if (angle > 135 && angle < 225)
    //        Inside(c2, c3, c4, c1);
    //    else if (angle > 225 && angle < 315)
    //        Inside(c3, c4, c1, c2);
    //    else if (angle > 315 || angle < 45)
    //        Inside(c4, c1, c2, c3);
    //}


}