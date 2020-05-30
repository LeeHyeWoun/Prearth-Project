using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Controller : GameController
{

    public GameObject[]
    mask;

    //변수
    protected bool play = true;
    protected WaitForSeconds wait = new WaitForSeconds(0.1f);

    //상수
    readonly string[] npc = new string[9] {
        "카페 사장 꼬륵이", "마트에서 나온 꼬륵이", "소풍 나온 꼬륵이",
        "그물에 묶인 거북이", "배가 아픈 뿌직이", "물놀이 하고 있는 뿌직이",
        "경찰 콜록이", "안경 콜록이", "마스크 콜록이" };

    protected void GameEnd(int num)
    {
        play = true;
        Clear_Clue(num + 1);
        mask[num].SetActive(false);
        Btns_clue[num].interactable = false;
        if (num < 2)
            Btns_clue[num + 1].interactable = true;
    }

    public void BE_clue(int num)
    {
        string advice = npc[num] + "를 찾아가세요.";
        AC.Advice(advice);
    }

}
