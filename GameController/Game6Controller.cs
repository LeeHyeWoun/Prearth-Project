using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game6Controller : GameController
{

    //3D Object
    public GameObject
        blank, blank_R1;  

    //Resource
    public Texture blank_turtle, blank_netturtle, blank_fish, blank_gfish, blank_plastic; //blank_fish = 물고기, blank_gfish = 째진 물고기

    //변수
   // bool[] clear = new bool[3] { false, false, false };
    bool play = true;

    //상수
    WaitForSeconds wait = new WaitForSeconds(5f);

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        print(name);
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
                        StartCoroutine(Game2(true));
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

    //protected override void Collect(int num, string name)
    //{
    //    Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[num];
    //    Movable_Clue[num - 1] = true;
    //    particles_clue_object[num - 1].Play();

    //    SoundManager.Instance.Play_effect(1);
    //    AC.Advice(name);
       
    //    // 거북이 단서를 찾았을 때
    //    if (Movable_Clue[0])
    //    {
    //        target.SetActive(false);

    //        // 코루틴 Game1 호출
    //        StartCoroutine(Game1(true));

    //        //드래그해야할 단서 표시하기
    //        Effect_clue(particles_clue_btn[0]);

    //    }
    //    // 미세플라스틱
    //    else if (Movable_Clue[1])
    //    {
    //        blank_R1.GetComponent<RawImage>().texture = blank_fish;
    //        //다음 명령 내리기
    //        StartCoroutine(Game2(true));

    //        //드래그해야할 단서 표시하기
    //        Effect_clue(particles_clue_btn[1]);

    //    }
    //    // 선크림 
    //    else if (Movable_Clue[2])
    //    {
    //        StartCoroutine(Game3());

    //        //드래그해야할 단서 표시하기
    //        Effect_clue(particles_clue_btn[2]);

    //    }
        

    //}

    //public 함수-----------------------------------------------------------------------------------
    public void BE_clue(int num)
    {
        string advice = "";
        switch (num)
        {
            case 1:
                advice = "그물에 묶인 거북이를 찾아가세요.";
                break;
            case 2:
                advice = "배가 아픈 뿌직이를 찾아가세요.";
                break;
            case 3:
                advice = "물놀이 하고 있는 뿌직이를 찾아가세요.";
                break;
        }
        AC.Advice(advice);
    }

    //도구 칼 사용
    public void KnifeEvent()
    {
        Time.timeScale = 1;
        ItemBox();
        SoundManager.Instance.Play_effect(1);

        //거북이랑 그물 분리
        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[1]))
        {
            blank_R1.GetComponent<RawImage>().texture = blank_turtle;
            StartCoroutine(Game1_1(true));
        }
        else if (Btns_clue[1].GetComponent<Image>().sprite.Equals(Sprites_clue[2]) && blank_R1.GetComponent<RawImage>().texture.name.Equals("6_fish"))
        {
            blank_R1.GetComponent<RawImage>().texture = blank_gfish;
            AC.Dialog_and_Advice("play2_1");
        }
    }

    //도구 돋보기 사용
    public void DodbogiEvent()
    {
        Time.timeScale = 1;
        ItemBox();
        SoundManager.Instance.Play_effect(1);

        //물고기 확대
        if (Btns_clue[1].GetComponent<Image>().sprite.Equals(Sprites_clue[2]) && blank_R1.GetComponent<RawImage>().texture.name.Equals("6_gfish"))
        {
                blank_R1.GetComponent<RawImage>().texture = blank_plastic;
                StartCoroutine(Game2_1(true));
    
        }


    }
    //private 함수---------------------------------------------------------------------------------
    //거북이 이벤트 시작 진행
    IEnumerator Game1(bool start)
    {
        if(start)
            AC.Dialog_and_Advice("play1");

        yield return wait;

        blank_R1.GetComponent<RawImage>().texture = blank_netturtle;
        blank.SetActive(start);
    }
    //거북이와 금루 분리 후 진행 내용
    IEnumerator Game1_1(bool start)
    {
        AC.Dialog_and_Advice("play1_1");

        yield return wait;

        blank.SetActive(false);

        Btns_clue[0].interactable = false;
        Clear_Clue(1);

    }
    //물고기 이벤트 시작
    IEnumerator Game2(bool start)
    {
            AC.Dialog_and_Advice("play2");

            yield return wait;

            blank_R1.GetComponent<RawImage>().texture = blank_fish;
            blank.SetActive(true);

        
    }
    //물고기 돋보기 이벤트 후 진행
    IEnumerator Game2_1(bool start)
    {
        AC.Dialog_and_Advice("play2_2");

            yield return wait;

        blank.SetActive(false);
        Clear_Clue(2);

    }
    //선크림 이벤트 진행 (대화로만)
    IEnumerator Game3()
    {
        AC.Dialog_and_Advice("play3");

            yield return wait;

        blank.SetActive(false);
        Clear_Clue(3);

    }

}