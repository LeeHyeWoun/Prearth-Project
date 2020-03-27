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
        RI_blank,
        clue1, clue2, clue3,                                //단서
        fridge_L, fridge_R,                                 //냉장고 문
        trash1_body, trash2_body, trash3_body, trash4_body, //쓰레기 통
        trash1_lid, trash2_lid, trash3_lid, trash4_lid;     //쓰레기 뚜껑
    public ParticleSystem eff_clue1, eff_clue2, eff_clue3;  //단서버튼의 이펙트 효과
    public Button clueBox1, clueBox2, clueBox3;             //clue 박스
    public Sprite clue_full;

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    int[] angle_trash = new int[4] { 0, 0, 0, 0 };          //쓰레기통 뚜껑 문 열림 정도
    GameObject[] trash_lids = new GameObject[4];
    Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);
    string str_preTarget = "";


    //커스텀 클래스 인스턴스
    SoundController SC;
    DialogManager DM;
    ClearManager CM;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        trash_lids[0] = trash1_lid;
        trash_lids[1] = trash2_lid;
        trash_lids[2] = trash3_lid;
        trash_lids[3] = trash4_lid;

        SC = GetComponent<SoundController>();
        DM = GetComponent<DialogManager>();
        CM = GetComponent<ClearManager>();
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
            //쓰레기통 뚜껑 모두 열기
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(Open_Trash(i));
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
            StartCoroutine(Open_Trash(num));
        else if (angle_trash[num] >= 100)   //닫기
            StartCoroutine(Close_Trash(num));
    }

    //쓰레기통 선택 이벤트...드래그
    public void Select_Trash() {
        string str_target="";

        if(target != null)
            str_preTarget = target.name;

        target = GetClickedObject();
        if (target != null)
        {
            str_target = target.name;
            if (str_target != str_preTarget) {
                int num = -1;
                Button clueBox;
                switch (str_target)
                {
                    case "trash1_2":
                        if (angle_trash[0] > 0) {
                        }
                        num = 0;
                        break;
                    case "trash2_2":
                        if (angle_trash[1] > 0)
                        {
                        }
                        num = 1;
                        break;
                    case "trash3_2":
                        if (angle_trash[2] > 0)
                        {
                        }
                        num = 2;
                        break;
                    case "trash4_2":
                        if (angle_trash[3] > 0)
                        {
                        }
                        num = 3;
                        break;

                    default:
                        SC.Play_effect(2);  //적절하지 않다는 효과음 내기
                        break;
                }
                //이벤트 적용
                if (num > -1 && ) {
                    SC.Play_effect(1);  //적절하다는 효과음 내기
                    print(num + "성공");
                }
                //이벤트 미적용
                else
                    SC.Play_effect(2);  //적절하지 않다는 효과음 내기

            }
        }
    }

    //쓰레기통 선택 이벤트...드래그
    public void Pick_Trash() {
        if (target != null)
        {
            if (target.Equals(trash1_body) && gameObject.name.Equals("B_Clue_1"))
            {
                gameObject.GetComponent<Image>().sprite = clue_full;
                RI_blank.SetActive(true);
                //적절하다는 효과음 내기
                SC.Play_effect(1);
            }
            else
                //적절하지 않다는 효과음 내기
                SC.Play_effect(2);
        }
        else
            //적절하지 않다는 효과음 내기
            SC.Play_effect(2);
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
