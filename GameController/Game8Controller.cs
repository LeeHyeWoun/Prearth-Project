using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/**
* Date     : 2020.05.01
* Manager  : 여연진
* 
* The function of this script :
*  Scene'8_AIr_1의 오브젝트에 관한 다양한 이벤트를 다루는 스크립트
*  
*  Applied Location :
*  -> GameDirector
*/
public class Game8Controller : GameController {

    public Text temperature; //온도 표시 text
    public GameObject
    clue1, clue2, clue3,                                //단서
    blank, win_aircap, B_aircap;

    public GameObject[] Elec_true, Elec_false;          //size = 4
    public ParticleSystem[] PS_Elec;                    //size = 4

    //변수
    MotionFridgeController MF;
    int elec_count = 0;
    int tem_count = 30; //온도 숫자

    void Start() {
        MF = GetComponent<MotionFridgeController>();

        Disappear_Clue[0] = false;
        Disappear_Clue[1] = false;
    }


    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "fan":
                if(!Btns_clue[0].interactable)
                    Collect(1, "사용하지 않는 선풍기");
                break;
            case "temperature":
                if(!Btns_clue[1].interactable)
                    Collect(2, "난방 설정 온도");
                break;
            case "aircap":
                Collect(3, "에어캡");
                break;
            case "fridge_left":
                MF.Motion_Left();
                break;
            case "fridge_right":
                MF.Motion_Right();
                break;
            case "fan_plug":
                if (Btns_clue[0].interactable && Btns_clue[1].interactable && Btns_clue[2].interactable)
                    Motion_elec(0);
                break;
            case "ligth1_plug":
                if (Btns_clue[0].interactable && Btns_clue[1].interactable && Btns_clue[2].interactable)
                    Motion_elec(1);
                break;
            case "ligth2_plug":
                if (Btns_clue[0].interactable && Btns_clue[1].interactable && Btns_clue[2].interactable)
                    Motion_elec(2);
                break;
            case "tv_plug":
                if (Btns_clue[0].interactable && Btns_clue[1].interactable && Btns_clue[2].interactable)
                    Motion_elec(3);
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    //온도계 이벤트 관리...온도 낮추기
    public void Down_Btn()
    {
        if (tem_count <= 18)
            return;

        tem_count--;
        SoundManager.Instance.Play_effect(0); //버튼 클릭 시 효과음
        temperature.text = tem_count + "℃";
    }
    //온도계 이벤트 관리...온도 높이기
    public void Up_Btn()
    {
        if (tem_count >= 30)
            return;

        tem_count++;
        SoundManager.Instance.Play_effect(0); //버튼 클릭 시 효과음 
        temperature.text = tem_count + "℃";
    }
    //온도계 이벤트 관리...온도 설정하기
    public void Select_Btn()
    {
        if (tem_count <= 20 && tem_count >= 18)
        {
            SoundManager.Instance.Play_effect(1); //적정 온도 맞을 떄 효과음
            blank.SetActive(false);
            Clear_Clue(2);
            AC.Dialog_and_Advice("play2_2");
        }
        else
            SoundManager.Instance.Play_effect(2); //적정 온도 아닐 시 효과음
    }
    // 단서 버튼 클릭 시 이벤트 진행
    public void BE_Clue1() {
        SoundManager.Instance.Play_effect(0);
        if (elec_count == 0)
            AC.Advice("콜록이의 집에서 문제의 단서를 찾아보세요.");
        else
            AC.Advice("아직 " + (4 - elec_count) + "개가 남았습니다.");
    }
    public void BE_Clue2()
    {
        SoundManager.Instance.Play_effect(0);
        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[4]))
        {
            blank.SetActive(true);
            Btns_clue[1].GetComponent<Image>().sprite = Sprites_clue[0];
            Btns_clue[1].interactable = false;
        }
        else
            AC.Advice("차례대로 진행해주세요.");
    }
    public void BE_Clue3()
    {
        SoundManager.Instance.Play_effect(0);
        if (Btns_clue[1].GetComponent<Image>().sprite.Equals(Sprites_clue[4]))
        {
            B_aircap.SetActive(true);
            Btns_clue[2].GetComponent<Image>().sprite = Sprites_clue[0];
            Btns_clue[2].interactable = false;
        }
        else
            AC.Advice("차례대로 진행해주세요.");
    }
    //창문에 뽁뽁이 붙이기
    public void Patch_Aircap() {
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            if (target_name.Equals("window"))
            {
                win_aircap.SetActive(true);
                B_aircap.SetActive(false);
                Clear_Clue(3);
                AC.Dialog_and_Advice("play2_3");
            }
        }
    }

    //private 함수---------------------------------------------------------------------------------
    // 코드 뽑기 이벤트 
    void Motion_elec(int num)
    {
        Elec_false[num].SetActive(true);
        Elec_true[num].SetActive(false);
        PS_Elec[num].Play();
        SoundManager.Instance.Play_effect(1);
        elec_count++;

        if (elec_count == 4)
        {
            Btns_clue[0].interactable = false;
            Clear_Clue(1);
            AC.Dialog_and_Advice("play2_1");
        }
    }
 
}
