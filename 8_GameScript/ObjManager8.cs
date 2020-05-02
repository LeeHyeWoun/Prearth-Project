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

public class ObjManager8 : RaycastManager//ObjManager는 무조건 RaycastManager을 상속할 것
{
    public GameObject
        blank, blank_RI,
        clue1, clue2, clue3,                                //단서
        fridge_L, fridge_R;                              //냉장고 문    
    public ParticleSystem eff_clue1, eff_clue2, eff_clue3;  //단서버튼의 이펙트 효과
    public Button clueBox1, clueBox2, clueBox3;             //clue 박스
    public Sprite clue_empty, clue_clear;

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    bool[] clear_clue = new bool[3] { false, false, false };//단서 클리어 정도
    Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);

    //커스텀 클래스 인스턴스
    AdviceController AC;
    GameButtonController GBC;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        AC = GetComponent<AdviceController>();
        GBC = GetComponent<GameButtonController>();
    }
    void Update()
    {
        if (GetPlay())
            ObjectClick();
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //오브젝트 클릭 이벤트
    void ObjectClick()
    {
        //누를때 오브젝트를 클릭 했다면
        if (Input.GetMouseButtonDown(0))
        {
            target = GetClickedObject();
            if (target != null)
            {
                switch (target.name)
                {
                    case "fan_plug":
                        Collect(1);
                        break;
                    case "temperature":
                        Collect(2);
                        break;
                    case "aircap":
                        Collect(3);
                        break;
                    case "fridge_left":
                        Motion_Fridge(fridge_L, 0);
                        break;
                    case "fridge_right":
                        Motion_Fridge(fridge_R, 1);
                        break;               
                }
            }
        }
    }

    //단서를 모두 찾았을 때의 이벤트 ... Collect에서 Invoke로 사용
    void Success()
    {
        AC.Dialog_and_Advice("play1");
    }

    //단서 발견 시 이벤트...클릭
    void Collect(int num)
    {
        Button clueBox;
        ParticleSystem eff_clue;
        string message;

        switch (num)
        {
            case 1:
                clueBox = clueBox1;
                eff_clue = eff_clue1;
                message = "선풍기 플러그";
                target.SetActive(false);
                break;

            case 2:
                clueBox = clueBox2;
                eff_clue = eff_clue2;
                message = "난방 설정 온도";
                break;

            default:
                clueBox = clueBox3;
                eff_clue = eff_clue3;
                message = "에어캡(뽁뽁이)";
                break;
        }

        SoundManager.Instance.Play_effect(0);
        //target.SetActive(false);

        //단서 박스에 채우기 및 이펙트 효과
        clueBox.interactable = true;
        eff_clue.transform.localScale = effectScale * Camera.main.orthographicSize / 5;
        eff_clue.Play();

        AC.Advice(message);

        //모든 단서를 찾았다면
        if (clueBox1.interactable && clueBox2.interactable && clueBox3.interactable)
        {
            //다음 명령 내리기
            Invoke("Success", 3f);
        }

    }

    //냉장고 문 여닫기 이벤트...클릭
    void Motion_Fridge(GameObject obj, int num)
    {
        if (angle_fridge[num] <= 0)         //열기
            StartCoroutine(Open_Fridge(obj, num));
        else if (angle_fridge[num] >= 120)  //닫기
            StartCoroutine(Close_Fridge(obj, num));
    }

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
                    clueBox1.GetComponent<Image>().sprite = clue_empty;
                    blank.SetActive(true);
                    AC.Dialog_and_Advice("Play2_1");
                }
                break;

            case "B_Clue_2":    //난방 적정 온도 
                //일반 쓰레기통
                if (trash_num == 1)
                {
                    success = true;
                    Clear_clue(2);
                    AC.Dialog_and_Advice("Play2_2");
                }

                break;

            case "B_Clue_3":    //에어캡
                //비닐 쓰레기통
                if (trash_num == 2)
                {
                    success = true;
                    clueBox3.GetComponent<Image>().sprite = clue_empty;
                    blank.SetActive(true);
                    AC.Dialog_and_Advice("Play2_3");
                }
                break;

        }
        return success;
    }

    public void Item_knife()
    {
        string name = GetClickUI();
        if (name != null && name.Equals("RI_Blank"))
        {
            blank.SetActive(false);
            GBC.ItemBox();
        }
    }

    public void Clear_clue(int num)
    {
        Button clueBox = null;
        switch (num)
        {
            case 1:
                clueBox = clueBox1;
                break;
            case 2:
                clueBox = clueBox2;
                break;
            case 3:
                clueBox = clueBox3;
                break;
        }
        if (clueBox != null)
        {
            clear_clue[num - 1] = true;
            clueBox.GetComponent<Image>().sprite = clue_clear;
        }

        if (clear_clue[0] && clear_clue[1] && clear_clue[2])
            Invoke("Ending", 3f);
    }

    void Ending()
    {
        //게임 마무리 대사
        AC.Dialog_and_Advice("clear");
    }


    //루틴+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //냉장고 열기
    IEnumerator Open_Fridge(GameObject obj, int num)
    {
        Vector3 vec = num == 0 ? Vector3.forward : Vector3.back;
        while (angle_fridge[num] < 120)
        {
            if (GetPlay())
            {
                obj.transform.Rotate(vec, 4);
                angle_fridge[num] += 4;
            }
            yield return null;
        }
    }
    //냉장고 닫기
    IEnumerator Close_Fridge(GameObject obj, int num)
    {
        Vector3 vec = num == 0 ? Vector3.forward : Vector3.back;
        while (angle_fridge[num] > 0)
        {
            if (GetPlay())
            {
                obj.transform.Rotate(vec, -4);
                angle_fridge[num] -= 4;
            }
            yield return null;
        }
    }

}
