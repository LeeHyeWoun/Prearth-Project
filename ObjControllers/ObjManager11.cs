using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    //3D Object
    public GameObject
        blank, blank_RI, B_Pet, B_Vinyl, B_icepack_vinyl, B_icepack_water,
        fridge_L, fridge_R;                                 //냉장고 문
    public GameObject[]
        GameObjects_Trash_lid;                              //쓰레기 뚜껑... size = 4

    //Effect
    public ParticleSystem[]
        particles_clue_btn,                                 //단서버튼의 이펙트 효과... size = 3
        particles_clue_object;                              //단서 위치의 이펙트 효과... size = 3

    //UI
    public Button[] Btns_clue;                              //clue 박스... size = 3

    //Resource
    public Texture blank_pat, blank_icepack;
    public Sprite[] Sprites_clue;                           //size = 4

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    int[] angle_trash = new int[4] { 0, 0, 0, 0 };          //쓰레기통 뚜껑 문 열림 정도
    bool[] clear_clue = new bool[3] { false, false, false };//단서 클리어 정도
    Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);
    IEnumerator coroutine_trash1, coroutine_trash2, coroutine_trash3, coroutine_trash4;
    AdviceController AC;
    GameButtonController GBC;


    //초기화----------------------------------------------------------------------------------
    void Start()
    {
        AC = GetComponent<AdviceController>();
        GBC = GetComponent<GameButtonController>();
    }
    void Update()
    {
        if (Time.timeScale.Equals(1))
            ObjectClick();
    }


    //private 함수----------------------------------------------------------------------------
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

    //단서 발견 시 이벤트...클릭
    void Collect(int num) {
        Btns_clue[num-1].GetComponent<Image>().sprite = Sprites_clue[num];
        clear_clue[num-1] = true;
        particles_clue_object[num-1].Play();

        SoundManager.Instance.Play_effect(1);
        target.SetActive(false);
        AC.Advice(num==1? "페트병" : num==2? "닭뼈" : "아이스팩");

        //모든 단서를 찾았다면
        if (clear_clue[0] && clear_clue[1] && clear_clue[2])
        {
            //초기화
            for (int i = 0; i < 3; i++)
                clear_clue[i] = false;

            //쓰레기통 뚜껑 모두 닫기
            for (int i = 0; i < 4; i++)
                StartCoroutine(Close_Trash(i));

            //다음 명령 내리기
            AC.Dialog_and_Advice("play1");

            //드래그해야할 단서 표시하기
            StartCoroutine(Effect_clue(particles_clue_btn[0]));

            //잠금 해제
            Enable_drag(true);
        }

    }

    IEnumerator Effect_clue(ParticleSystem ps) {
        yield return wait;
        ps.transform.localScale = effectScale * cmr.orthographicSize / 5;
        ps.Play();
    }

    //단서 드래그 활성화여부 설정 겸 이펙트 효과
    void Enable_drag(bool b)
    {
        for (int i = 0; i < 3; i++)
            if (!clear_clue[i])
            {
                Btns_clue[i].interactable = b;
                if (b)
                    StartCoroutine(Effect_clue(particles_clue_btn[i]));
            }
    }

    //냉장고 문 여닫기 이벤트...클릭
    void Motion_Fridge(GameObject obj, int num)
    {
        SoundManager.Instance.Play_effect(0);

        if (angle_fridge[num] <= 0)         //열기
            StartCoroutine(Open_Fridge(obj, num));
        else if (angle_fridge[num] >= 120)  //닫기
            StartCoroutine(Close_Fridge(obj, num));
    }

    //쓰레기통 뚜껑 여닫기 이벤트...클릭
    void Motion_Trash(int num)
    {
        SoundManager.Instance.Play_effect(0);

        if (angle_trash[num] <= 0)          //열기
        {
            IEnumerator coroutine = Open_Trash(num);
            switch (num) {  //다른 뚜껑 닫기 이벤트 시 구별을 위한 개별 코루틴 사용
                case 0:
                    coroutine_trash1 = coroutine;
                    break;
                case 1:
                    coroutine_trash2 = coroutine;
                    break;
                case 2:
                    coroutine_trash3 = coroutine;
                    break;
                case 3:
                    coroutine_trash4 = coroutine;
                    break;
            }
            StartCoroutine(coroutine);
        }
        else if (angle_trash[num] >= 100)   //닫기
            StartCoroutine(Close_Trash(num));
    }


    //public 함수----------------------------------------------------------------------------
    //쓰레기통 선택 이벤트...드래그
    //I_DragManager.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name) {

        target = GetClickedObject();
        if (target != null)
        {
            string obj_name = target.name;
            //단서1...페트병
            if (clue_name.Equals("B_Clue_1") 
                && obj_name.Equals("trash1_2") && angle_trash[0] > 0)
            {
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                Btns_clue[0].GetComponent<Image>().sprite = Sprites_clue[0];
                blank_RI.GetComponent<RawImage>().texture = blank_pat;
                blank.SetActive(true);
                Enable_drag(false);  //잠금

                AC.Dialog_and_Advice("play2_1");
            }
            //단서2...닭뼈
            else if (clue_name.Equals("B_Clue_2"))
            {
                if (obj_name.Equals("trash2_2") && angle_trash[1] > 0)
                {
                    SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                    clear_clue[1] = true;
                    Btns_clue[1].interactable = false;
                    GBC.Clear_Clue(2);
                    Enable_drag(true);
                }
                else if (obj_name.Equals("trash4_2") && angle_trash[3] > 0)
                {
                    SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
                    AC.Dialog_and_Advice("play2_2");
                }
            }
            //단서3...아이스팩
            else if (clue_name.Equals("B_Clue_3") 
                && obj_name.Equals("trash3_2") && angle_trash[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                Btns_clue[2].GetComponent<Image>().sprite = Sprites_clue[0];
                blank_RI.GetComponent<RawImage>().texture = blank_icepack;
                blank.SetActive(true);
                Enable_drag(false);  //잠금

                AC.Dialog_and_Advice("play2_3");
            }
            else
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
        }
    }

    public void Drag_DisassembledClue1(string name) {
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            //페트병 - 재활용 쓰레기통
            if (name.Equals("B_Pet") && target_name.Equals("trash1_2") && angle_trash[0] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_Pet.SetActive(false);
            }
            else
            //비닐 - 비닐 쓰레기통
            if (name.Equals("B_Vinyl") && target_name.Equals("trash3_2") && angle_trash[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_Vinyl.SetActive(false);
            }
            else
                SoundManager.Instance.Play_effect(2);

            if (!B_Pet.activeSelf && !B_Vinyl.activeSelf)
            {
                clear_clue[0] = true;
                Enable_drag(true);  //잠금해제
                GBC.Clear_Clue(1);
            }
        }
        else
            SoundManager.Instance.Play_effect(2);
    }

    public void Drag_DisassembledClue2(string name)
    {
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            //비닐 - 비닐 쓰레기통
            if (name.Equals("B_icepack_vinyl") && target_name.Equals("trash3_2") && angle_trash[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_icepack_vinyl.SetActive(false);
            }
            else
            //아이스팩 내용물 - 일반 쓰레기통
            if (name.Equals("B_icepack_water") && target_name.Equals("trash2_2") && angle_trash[1] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_icepack_water.SetActive(false);
            }
            else
                SoundManager.Instance.Play_effect(2);

            if (!B_icepack_vinyl.activeSelf && !B_icepack_water.activeSelf)
            {
                clear_clue[2] = true;
                Enable_drag(true);  //잠금해제
                GBC.Clear_Clue(3);
            }
        }
        else
            SoundManager.Instance.Play_effect(2);
    }

    public void Disassembled() {
        blank.SetActive(false);
        GBC.ItemBox();
        SoundManager.Instance.Play_effect(1);

        //페트병 분리
        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[0]))
        {
            B_Vinyl.SetActive(true);
            B_Pet.SetActive(true);
        }
        //아이스팩 분리
        else
        {
            B_icepack_vinyl.SetActive(true);
            B_icepack_water.SetActive(true);
        }
    }

    //코루틴---------------------------------------------------------------------------------------
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
    //쓰레기통 열기
    IEnumerator Open_Trash(int num)
    {
        Vector3 vec = Vector3.left;
        while (angle_trash[num] < 100)
        {
            for (int i = 0; i < 4; i++)
            {
                //(num+1)번 쓰레기통은 뚜껑을 열고
                if (i == num)
                {
                    GameObjects_Trash_lid[i].transform.Rotate(vec, 4);
                    angle_trash[i] += 4;
                }
                //나머지 뚜껑은 닫기
                else if (angle_trash[i] > 0)
                {
                    switch (i)  //열리고 있는 뚜껑과 이벤트 충돌 방지
                    {
                        case 0:
                            StopCoroutine(coroutine_trash1);
                            break;
                        case 1:
                            StopCoroutine(coroutine_trash2);
                            break;
                        case 2:
                            StopCoroutine(coroutine_trash3);
                            break;
                        case 3:
                            StopCoroutine(coroutine_trash4);
                            break;
                    }

                    GameObjects_Trash_lid[i].transform.Rotate(vec, -4);
                    angle_trash[i] -= 4;
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
            GameObjects_Trash_lid[num].transform.Rotate(vec, -4);
            angle_trash[num] -= 4;
            yield return null;
        }
    }

}
