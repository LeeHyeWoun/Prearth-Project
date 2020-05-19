using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Obj2Controller : ObjController
{
    //3D Object
    public GameObject
        blank, blank_RI, B_Pet, B_Vinyl, B_icepack_vinyl, B_icepack_water,
        fridge_L, fridge_R;                                 //냉장고 문
    public GameObject[]
        GameObjects_Trash_lid;                              //쓰레기 뚜껑... size = 4

    //변수
    int[] angle_fridge = new int[2] { 0, 0 };               //냉장고 문 열림 정도
    int[] angle_trash = new int[4] { 0, 0, 0, 0 };          //쓰레기통 뚜껑 문 열림 정도
    bool[] clear_clue = new bool[3] { false, false, false };//단서 클리어 정도
    IEnumerator coroutine_trash1, coroutine_trash2, coroutine_trash3, coroutine_trash4;


    //Overriding
    protected override void SetObjectEvent(string name) {
        switch (name)
        {
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
    public override void KnifeEvent() {
        Time.timeScale = 1;
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

    //단서 발견 시 이벤트...클릭
    void Collect(int num)
    {
        Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[num];
        clear_clue[num - 1] = true;
        particles_clue_object[num - 1].Play();

        SoundManager.Instance.Play_effect(1);
        target.SetActive(false);
        AC.Advice(num == 1 ? "페트병" : num == 2 ? "닭뼈" : "아이스팩");

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
            Effect_clue(particles_clue_btn[0]);

            //잠금 해제
            Enable_drag(true);
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
            switch (num)
            {  //다른 뚜껑 닫기 이벤트 시 구별을 위한 개별 코루틴 사용
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
