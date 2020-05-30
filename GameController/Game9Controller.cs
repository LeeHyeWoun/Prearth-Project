using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game9Controller : Stage2Controller
{

    public GameObject car_red;

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
