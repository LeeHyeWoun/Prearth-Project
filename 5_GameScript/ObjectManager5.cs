using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.03.27
 * Manager  : 여연진
 * 
 * Date     : 2020.05.14
 * Modify   : 이혜원
 * 
 * The function of this script :
 *  Scene'05_Water_1의 오브젝트에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> GameDirector
 */

public class ObjectManager5 : RaycastManager//ObjManager는 무조건 RaycastManager을 상속할 것
{
    //UI
    public Button[] Btns_clue;  //size = 3

    //Effect
    public ParticleSystem[]
        Particles_clue_btn,     //size = 3
        Particles_clue_object;  //size = 3

    //Resource
    public Sprite clue_full, clue_clear;

    //변수
    AdviceController AC;
    Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);
    bool[] clear_clue = new bool[3] { false, false, false };//단서 클리어 정도


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        AC = GetComponent<AdviceController>();
    }
    void Update()
    {
        if (Time.timeScale.Equals(1))
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
                    case "pan":
                        Collect(1);
                        break;
                    case "drug":
                        Collect(2);
                        break;
                    case "lens":
                        Collect(3);
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
    void Collect(int i)
    {
        SoundManager.Instance.Play_effect(0);
        target.SetActive(false);


        //단서 박스애 채우기 및 이펙트 효과
        Btns_clue[i - 1].interactable = true;
        Particles_clue_btn[i - 1].transform.localScale = effectScale * Camera.main.orthographicSize / 5;
        Particles_clue_btn[i - 1].Play();
        Particles_clue_object[i - 1].Play();

        AC.Advice(i == 1 ? "기름" : i == 2 ? "물약" : "렌즈");

        //모든 단서를 찾았다면
        if (Btns_clue[0].interactable && Btns_clue[1].interactable && Btns_clue[2].interactable)
        {
            Invoke("Success", 3f);
        }

    }

    //기름 선택 이벤트...드래그
    //DragController14.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name)
    {

        target = GetClickedObject();
        if (target != null)
        {
            int num = -1;
            SetTrash_Number(target.name, ref num);

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
            case "oil_can":
                num = 0;
                break;
            case "drug_can":
                num = 1;
                break;
            case "lens_trashcan":
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

            case "B_Clue_1":    //기름
                //폐기름통
                if (trash_num == 0)
                {
                    success = true;
                    Btns_clue[0].GetComponent<Image>().sprite = clue_full;
                    Clear_clue(1);
                    AC.Dialog_and_Advice("Play2_1");
                }
                break;

            case "B_Clue_2":    //물약
                //폐약품함
                if (trash_num == 1)
                {
                    success = true;
                    Btns_clue[1].GetComponent<Image>().sprite = clue_full;
                    Clear_clue(2);
                    AC.Dialog_and_Advice("Play2_2");
                }

                break;

            case "B_Clue_3":    //렌즈
                //비닐 쓰레기통
                if (trash_num == 2)
                {
                    success = true;
                    Btns_clue[2].GetComponent<Image>().sprite = clue_full;
                    Clear_clue(3);
                    AC.Dialog_and_Advice("Play2_3");
                }
                break;

        }
        return success;
    }

    public void Clear_clue(int num)
    {
        clear_clue[num - 1] = true;
        Btns_clue[num - 1].GetComponent<Image>().sprite = clue_clear;

        if (clear_clue[0] && clear_clue[1] && clear_clue[2])
            Invoke("Ending", 3f);
    }

    void Ending()
    {
        //게임 마무리 대사
        AC.Dialog_and_Advice("clear");
    }

}

