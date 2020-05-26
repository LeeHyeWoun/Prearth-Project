using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game9Controller : GameController {

    public GameObject car_red;
    public GameObject[]
        mask;

    //변수
    bool play = true;

    //상수
    WaitForSeconds wait = new WaitForSeconds(0.1f);

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "Npc3_1":
                if (play)
                {
                    if (Btns_clue[0].interactable)
                        StartCoroutine(Game1(true));
                    else
                        AC.Advice("다른 콜록이입니다!");
                }
                break;
            case "Npc3_2":
                if (play)
                {
                    if (Btns_clue[1].interactable)
                        StartCoroutine(Game2());
                    else
                        AC.Advice("다른 콜록이입니다!");
                }
                break;
            case "Npc3_3":
                if (play)
                {
                    if (Btns_clue[2].interactable)
                        StartCoroutine(Game3());
                    else
                        AC.Advice("다른 콜록이입니다!");
                }
                break;
            case "Car_red":
                if (!play && Btns_clue[0].interactable)
                {
                    car_red.SetActive(false);
                    particles_clue_object[0].Play();
                    StartCoroutine(Game1(false));
                }
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    public void BE_clue(int num)
    {
        string advice = "";
        switch (num)
        {
            case 1:
                advice = "경찰 콜록이를 찾아가세요.";
                break;
            case 2:
                advice = "안경 콜록이를 찾아가세요.";
                break;
            case 3:
                advice = "마스크 콜록이를 찾아가세요.";
                break;
        }
        AC.Advice(advice);
    }

    //private 함수-----------------------------------------------------------------------------------
    void GameEnd(int num)
    {
        play = true;
        Clear_Clue(num + 1);
        mask[num].SetActive(false);
        Btns_clue[num].interactable = false;
        if (num < 2)
            Btns_clue[num + 1].interactable = true;
    }


    //코루틴 -----------------------------------------------------------------------------------
    IEnumerator Game1(bool start)
    {
        if (start)
            AC.Dialog_and_Advice("play1_1");
        else
        {
            yield return new WaitForSeconds(2f);
            AC.Dialog_and_Advice("play1_2");
        }

        yield return wait;

        if (start)
            play = false;
        else
            GameEnd(0);
    }
    IEnumerator Game2()
    {
        AC.Dialog_and_Advice("play2");
        yield return wait;

        GameEnd(1);
    }
    IEnumerator Game3()
    {
        AC.Dialog_and_Advice("play3");
        yield return wait;

        GameEnd(2);
    }

}
