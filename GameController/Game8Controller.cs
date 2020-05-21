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

    public GameObject
    blank,
    clue1, clue2, clue3,                                //단서
    fridge_L, fridge_R;                              //냉장고 문

    public GameObject[] Elec_true, Elec_false;

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    int elec_count = 0;

    void Start() {
        Disappear_Clue[0] = false;
        Disappear_Clue[1] = false;
    }

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "fan":
                Collect(1, "사용하지 않는 선풍기");
                break;
            case "temperature":
                Collect(2, "난방 설정 온도");
                break;
            case "aircap":
                Collect(3, "에어캡");
                break;
            case "fridge_left":
                Motion_Fridge(fridge_L, 0);
                break;
            case "fridge_right":
                Motion_Fridge(fridge_R, 1);
                break;
            case "fan_plug":
                if (Movable_Clue[0]&& Movable_Clue[1]&& Movable_Clue[2])
                { Motion_elec(0); }
                break;
            case "ligth1_plug":
                if (Movable_Clue[0] && Movable_Clue[1] && Movable_Clue[2])
                { Motion_elec(1); }
                break;
            case "ligth2_plug":
                if (Movable_Clue[0] && Movable_Clue[1] && Movable_Clue[2])
                { Motion_elec(2); }
                break;
            case "tv_plug":
                if (Movable_Clue[0]&& Movable_Clue[1]&& Movable_Clue[2])
                { Motion_elec(3); }
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------

    //쓰레기통 선택 이벤트...드래그
    //I_DragManager.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name)
    {

        target = GetClickedObject();
        if (target != null)
        {
            int num = -1;
            SetTrash_Number(target.name, ref num);
            //print(target.name + " / " + num);

            if (Match_Clue_to_Trash(clue_name, num))
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
            else
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
        }
    }

    public void KnifeEvent()
    {
        Time.timeScale = 1;
        blank.SetActive(false);
        ItemBox();
        SoundManager.Instance.Play_effect(1);

        blank.SetActive(false);
        ItemBox();
    }

    //private 함수---------------------------------------------------------------------------------
    //냉장고 문 여닫기 이벤트...클릭
    void Motion_Fridge(GameObject obj, int num)
    {
        if (angle_fridge[num] <= 0)         //열기
            StartCoroutine(Open_Fridge(obj, num));
        else if (angle_fridge[num] >= 120)  //닫기
            StartCoroutine(Close_Fridge(obj, num));
    }


    void SetTrash_Number(string target_name, ref int num)
    {
        switch (target_name)
        {
            case "trash1_2":
                num = 0;
                break;
            case "trash2_2":
                num = 1;
                break;
            case "window":
                num = 2;
                break;

            default:
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
                break;
        }
    }

    //적절한 단서와 쓰레기통이 매치되었는지 확인
    bool Match_Clue_to_Trash(string clue_name, int trash_num)
    {
        bool success = false;
        switch (clue_name)
        {

            case "B_Clue_1":    // 선풍기 코드 
                //재활용 쓰레기통
                if (trash_num == 0)
                {
                    success = true;
                    Btns_clue[0].GetComponent<Image>().sprite = Sprites_clue[0];
                    blank.SetActive(true);
                    AC.Dialog_and_Advice("play2_1");
                }
                break;

            case "B_Clue_2":    //난방 적정 온도 
                //일반 쓰레기통
                if (trash_num == 1)
                {
                    success = true;
                    Clear_Clue(2);
                    AC.Dialog_and_Advice("play2_2");
                }

                break;

            case "B_Clue_3":    //에어캡
                //비닐 쓰레기통
                if (trash_num == 2)
                {
                    success = true;
                    Btns_clue[2].GetComponent<Image>().sprite = Sprites_clue[0];
                    blank.SetActive(true);
                    AC.Dialog_and_Advice("play2_3");
                }
                break;

        }
        return success;
    }

    // 코드 뽑기 이벤트 
    void Motion_elec(int num)
    {
        Elec_false[num].SetActive(true);
        Elec_true[num].SetActive(false);
        switch (num)
        {
            case 0:
                SoundManager.Instance.Play_effect(1);
                elec_count++;
                break;
            case 1:
                SoundManager.Instance.Play_effect(1);
                elec_count++;
                break;
            case 2:
                SoundManager.Instance.Play_effect(1);
                elec_count++;
                break;
            case 3:
                SoundManager.Instance.Play_effect(1);
                elec_count++;
                break;

        }

        if (elec_count == 4)
        {
            Clear_Clue(1);
            AC.Dialog_and_Advice("play2_1");
        }
    }

    //루틴+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //냉장고 열기
    IEnumerator Open_Fridge(GameObject obj, int num)
    {
        Vector3 vec = num == 0 ? Vector3.forward : Vector3.back;
        while (angle_fridge[num] < 120)
        {
            obj.transform.Rotate(vec, 4);
            angle_fridge[num] += 4;
            yield return null;
        }
    }
    //냉장고 닫기
    IEnumerator Close_Fridge(GameObject obj, int num)
    {
        Vector3 vec = num == 0 ? Vector3.forward : Vector3.back;
        while (angle_fridge[num] > 0)
        {
            obj.transform.Rotate(vec, -4);
            angle_fridge[num] -= 4;
            yield return null;
        }
    }

}
