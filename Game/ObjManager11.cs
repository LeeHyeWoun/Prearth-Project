using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/**
 * Date     : 2020.03.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene'11_Soil_1의 오브젝트에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> GameDirector
 */

public class ObjManager11 : RaycastManager//ObjManager는 무조건 RaycastManager을 상속할 것
{
    public GameObject
        blank, blank_RI, B_Pet, B_Vinyl,
        clue1, clue2, clue3,                                //단서
        fridge_L, fridge_R,                                 //냉장고 문
        trash1_body, trash2_body, trash3_body, trash4_body, //쓰레기 통
        trash1_lid, trash2_lid, trash3_lid, trash4_lid;     //쓰레기 뚜껑
    public ParticleSystem eff_clue1, eff_clue2, eff_clue3;  //단서버튼의 이펙트 효과
    public Button clueBox1, clueBox2, clueBox3;             //clue 박스
    public Sprite clue_full, clue_clear;
    public Texture blank_pat, blank_icepack;

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    int[] angle_trash = new int[4] { 0, 0, 0, 0 };          //쓰레기통 뚜껑 문 열림 정도
    bool[] clear_clue = new bool[3] { false, false, false };//단서 클리어 정도
    GameObject[] trash_lids = new GameObject[4];
    Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);
    IEnumerator trash_corotine;

    //커스텀 클래스 인스턴스
    SoundController SC;
    DialogManager DM;
    //ClearManager CM;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        trash_lids[0] = trash1_lid;
        trash_lids[1] = trash2_lid;
        trash_lids[2] = trash3_lid;
        trash_lids[3] = trash4_lid;

        SC = GetComponent<SoundController>();
        DM = GetComponent<DialogManager>();
        //CM = GetComponent<ClearManager>();
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
                switch (target.name) {
                    case "Bottle":
                        Collect(1);
                        break;
                    case "bone1":
                        Collect(2);
                        break;
                    case "icepack":
                        Collect(3);
                        break;
                    case "fridge_left":
                        Motion_Fridge(fridge_L, 0);
                        break;
                    case "fridge_right":
                        Motion_Fridge(fridge_R, 1);
                        break;
                    case "trash1_2":
                        Motion_Trash(0);
                        break;
                    case "trash2_2":
                        Motion_Trash(1);
                        break;
                    case "trash3_2":
                        Motion_Trash(2);
                        break;
                    case "trash4_2":
                        Motion_Trash(3);
                        break;
                }
            }
        }
    }

    //단서를 모두 찾았을 때의 이벤트 ... Collect에서 Invoke로 사용
    void Success()
    {
        DM.Dialog_Start("11_play1", "PET 병을 올바르게 분리수거하세요.");
    }

    //단서 발견 시 이벤트...클릭
    void Collect(int num) {
        Button clueBox;
        ParticleSystem eff_clue;
        string message;

        switch (num) {
            case 1:
                clueBox = clueBox1;
                eff_clue = eff_clue1;
                message = "페트병";
                break;

            case 2:
                clueBox = clueBox2;
                eff_clue = eff_clue2;
                message = "닭뼈";
                break;

            default:
                clueBox = clueBox3;
                eff_clue = eff_clue3;
                message = "아이스팩";
                break;
        }
        
        SC.Play_effect(0);
        target.SetActive(false);

        //단서 박스애 채우기 및 이펙트 효과
        clueBox.interactable = true;
        eff_clue.transform.localScale = effectScale * Camera.main.orthographicSize / 5;
        eff_clue.Play();

        DM.Message(message);

        //모든 단서를 찾았다면
        if (clueBox1.interactable && clueBox2.interactable && clueBox3.interactable)
        {
            //쓰레기통 뚜껑 모두 닫기
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(Close_Trash(i));
            }
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

    //쓰레기통 뚜껑 여닫기 이벤트...클릭
    void Motion_Trash(int num)
    {
        if (angle_trash[num] <= 0)          //열기
        {
            trash_corotine = Open_Trash(num);
            StartCoroutine(trash_corotine);
        }
        else if (angle_trash[num] >= 100)   //닫기
            StartCoroutine(Close_Trash(num));
    }

    //쓰레기통 선택 이벤트...드래그
    //I_DragManager.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name) {

        target = GetClickedObject();
        if (target != null)
        {
            int num = -1;
            SetTrash_Number(target.name, ref num);
            //print(target.name + " / " + num);

            if (Match_Clue_to_Trash(clue_name, num))
            {
                SC.Play_effect(1);  //적절하다는 효과음 내기
            }
            else
                SC.Play_effect(2);  //적절하지 않다는 효과음 내기
        }
    }

    //쓰레기통 번호 매기기
    void SetTrash_Number(string target_name, ref int num) {
        switch (target_name)
        {
            case "trash1_2":
                if (angle_trash[0] > 0)
                    num = 0;
                break;
            case "trash2_2":
                if (angle_trash[1] > 0)
                    num = 1;
                break;
            case "trash3_2":
                if (angle_trash[2] > 0)
                    num = 2;
                break;
            case "trash4_2":
                if (angle_trash[3] > 0)
                    num = 3;
                break;

            default:
                SC.Play_effect(2);  //적절하지 않다는 효과음 내기
                break;
        }
    }

    //적절한 단서와 쓰레기통이 매치되었는지 확인
    bool Match_Clue_to_Trash(string clue_name, int trash_num)
    {
        bool success = false;
        switch (clue_name)
        {

            case "B_Clue_1":    //페트병
                //재활용 쓰레기통
                if (trash_num == 0)
                {   
                    success = true;
                    clueBox1.GetComponent<Image>().sprite = clue_full;

                    blank_RI.GetComponent<RawImage>().texture = blank_pat;
                    blank.SetActive(true);
                    DM.Dialog_Start("11_Play2_1", "비닐과 플라스틱을 분리해주세요.");
                }
                break;

            case "B_Clue_2":    //닭뼈
                //일반 쓰레기통
                if (trash_num == 1)
                {
                    success = true;
                    Clear_clue(2);
                }
                //음식물 쓰레기통
                else if (trash_num == 3)
                    DM.Dialog_Start("11_Play2_2", null);

                break;

            case "B_Clue_3":    //아이스팩
                //비닐 쓰레기통
                if (trash_num == 2)
                {
                    success = true;
                    clueBox3.GetComponent<Image>().sprite = clue_full;
                    blank_RI.GetComponent<RawImage>().texture = blank_icepack;
                    blank.SetActive(true);
                    DM.Dialog_Start("11_Play2_3", "내용물과 비닐을 분리해주세요.");
                }
                break;

        }
        return success;
    }

    public void DragDrop_Clue(string name) {
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            bool success = false;
            switch (name) {
                case "B_Pet":       //페트병 - 재활용 쓰레기통
                    if (target_name.Equals("trash1_2")) {
                        success = true;
                        B_Pet.SetActive(false);
                    }
                    break;

                case "B_Vinyl":     //비닐 - 비닐 쓰레기통
                    if (target_name.Equals("trash3_2")){
                        success = true;
                        B_Vinyl.SetActive(false);
                    }
                    break;
            }
            if(success)
                SC.Play_effect(1);
            else
                SC.Play_effect(2);
        }
        else
            SC.Play_effect(2);
    }
    public void Item_knife() {
        string name = GetClickUI();
        if (name != null && name.Equals("RI_Blank"))
        {
            blank.SetActive(false);
            B_Vinyl.SetActive(true);
            B_Pet.SetActive(true);
            GetComponent<SettingController_game>().ItemBox();
        }
    }

    public void Clear_clue(int num) {
        Button clueBox = null;
        switch (num) {
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
        if (clueBox != null) {
            clear_clue[num - 1] = true;
            clueBox.GetComponent<Image>().sprite = clue_clear;
        }

        if (clear_clue[0] && clear_clue[1] && clear_clue[2])
            Invoke("Ending", 3f);
    }

    void Ending()
    {
        //게임 마무리 대사
        DM.Dialog_Start("11_clear", "end");
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
    //쓰레기통 열기
    IEnumerator Open_Trash(int num)
    {
        Vector3 vec = Vector3.left;
        while (angle_trash[num] < 100)
        {
            if (GetPlay())
            {
                for (int i = 0; i < 4; i++) {
                    //(num+1)번 쓰레기통은 뚜껑을 열고
                    if (i == num) {
                        trash_lids[i].transform.Rotate(vec, 4);
                        angle_trash[i] += 4;
                    }
                    //나머지 뚜껑은 닫기
                    else if (angle_trash[i] > 0) {
                        StopCoroutine(trash_corotine);
                        trash_lids[i].transform.Rotate(vec, -4);
                        angle_trash[i] -= 4;
                    }
                }
            }
            yield return null;
        }
    }

    //쓰레기통 닫기
    IEnumerator Close_Trash(int num)
    {
        Vector3 vec = Vector3.left;
        while (angle_trash[num] > 0)
        {
            if (GetPlay())
            {
                trash_lids[num].transform.Rotate(vec, -4);
                angle_trash[num] -= 4;
            }
            yield return null;
        }
    }

}
