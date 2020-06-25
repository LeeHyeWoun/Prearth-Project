using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game6Controller : Stage2Controller
{

    //3D Object
    public GameObject
        blank, blank_R1;

    //Resource
    public Texture blank_turtle, blank_netturtle, blank_fish, blank_gfish, blank_plastic; //blank_fish = 물고기, blank_gfish = 째진 물고기

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "turtle":
                if (play) {
                    if (Btns_clue[0].interactable)
                        StartCoroutine(Game1(true));
                    //Collect(1, "그물에 묶인 거북이");
                    else
                        AC.Advice("다른 NPC입니다!");
                }
                break;
            case "Npc2_1":
                if (play)
                {
                    if (Btns_clue[1].interactable)
                        StartCoroutine(Game2());
                    //Collect(2, "배가 아픈 뿌직이");
                    else
                        AC.Advice("다른 NPC입니다!");
                }
                break;
            case "Npc2_2":
                if (play)
                {
                    if (Btns_clue[2].interactable)
                        StartCoroutine(Game3());
                    //Collect(3, "물놀이 중인 뿌직이");
                    else
                        AC.Advice("다른 NPC입니다!");
                }
                break;
            default:
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    //도구 칼 사용
    public bool DE_Item2()
    {
        Time.timeScale = 1;

        //거북이랑 그물 분리
        if (blank_R1.GetComponent<RawImage>().texture.name.Equals("clue4_0"))
        {
            ItemBox();
            SoundManager.Instance.Play_effect(1);
            blank_R1.GetComponent<RawImage>().texture = blank_turtle;
            StartCoroutine(Game1(false));
            return true;
        }
        else if (blank_R1.GetComponent<RawImage>().texture.name.Equals("clue4_1"))
        {
            ItemBox();
            SoundManager.Instance.Play_effect(1);
            blank_R1.GetComponent<RawImage>().texture = blank_gfish;
            AC.Dialog_and_Advice("play2_1");
            return true;
        }
        else
            return false;
    }

    //도구 돋보기 사용
    public bool DE_Item3()
    {
        Time.timeScale = 1;

        //물고기 확대
        if (blank_R1.GetComponent<RawImage>().texture.name.Equals("6_gfish"))
        {
            ItemBox();
            SoundManager.Instance.Play_effect(1);
            blank_R1.GetComponent<RawImage>().texture = blank_plastic;
            StartCoroutine(Game2_1());
            return true;
        }
        else
            return false;

    }
    //private 함수---------------------------------------------------------------------------------
    //거북이 이벤트
    IEnumerator Game1(bool start)
    {
        if (start)
        {
            blank_R1.GetComponent<RawImage>().texture = blank_netturtle;
            AC.Dialog_and_Advice("play1");
        }
        else
            AC.Dialog_and_Advice("play1_1");

        yield return wait;

        blank.SetActive(start);

        if (!start)
            GameEnd(0);
    }
    //물고기 이벤트 시작
    IEnumerator Game2()
    {
        blank_R1.GetComponent<RawImage>().texture = blank_fish;
        AC.Dialog_and_Advice("play2");

        yield return wait;
        blank.SetActive(true);
    }
    //물고기 돋보기 이벤트 후 진행
    IEnumerator Game2_1()
    {
        AC.Dialog_and_Advice("play2_2");

            yield return wait;

        blank.SetActive(false);
        GameEnd(1);
    }
    //선크림 이벤트 진행 (대화로만)
    IEnumerator Game3()
    {
        AC.Dialog_and_Advice("play3");

            yield return wait;

        GameEnd(2);
    }

}