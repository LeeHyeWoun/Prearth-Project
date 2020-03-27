using UnityEngine;
/**
 * Date     : 2019.11.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  일시정지 버튼에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> GameDirector(빈 오브젝트)
 */

public class SettingController_game : MonoBehaviour {

    //객체
    /*앱 시작 시 activeSelf == false 상태여서 GameObject.Find로 찾을 수 없고 public으로 직접 연결해야함*/
    public GameObject setting, Blur, P_Blur, itemBox;

    //커스텀 클래스 인스턴스
    RaycastManager RM;
    ZoomManager ZM;

    private void Start()
    {
        RM = GetComponent<RaycastManager>();
        ZM = GetComponent<ZoomManager>();
    }

    private void SetPlays(bool b)
    {
        RM.SetPlay(b);
        ZM.SetPlay(b);
    }

    //일시정지
    //Apply to 'B_Pause'
    public void Pause()
    {
        //일시정지 창 띄우기
        Blur.SetActive(true);
        P_Blur.SetActive(true);
        setting.SetActive(true);

        //zoom 이벤트 막기
        SetPlays(false);
    }

    //계속하기
    //Apply to 'B_KeepPlay'
    public void KeepPlay()
    {
        //모든 창 닫기
        P_Blur.SetActive(false);
        setting.SetActive(false);
        Blur.SetActive(false);

        //zoom 이벤트 실행 허가
        SetPlays(true);
    }

    //아이템박스 열고 닫기
    //Apply to 'B_Item'
    public void ItemBox() {
        if (itemBox.activeSelf)
            itemBox.SetActive(false);
        else
            itemBox.SetActive(true);
    }
}
