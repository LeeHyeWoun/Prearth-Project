using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game3Controller : GameController {

    //3D Object
    public GameObject
        blank, balance_lever, balance_A, balance_B, tissueTree;
    public Image cup_A, cup_B;

    public GameObject[] 
        mask, plains; //size = 3
    public Image[] results; //size = 3
    public Button[] cups;   //size = 3

    //변수
    bool[] clear = new bool[3] { false, false, false };
    bool play = true;
    int weight_A, weight_B = -1;
    int count = -1;

    //상수
    WaitForSeconds wait = new WaitForSeconds(0.1f);

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "Npc1_1":
                if (play)
                {
                    if(Btns_clue[0].interactable)
                        StartCoroutine(Game1(true));
                    else
                        AC.Advice("다른 꼬륵이입니다!");
                }
                break;
            case "Npc1_2":
                if (play)
                {
                    if (Btns_clue[1].interactable)
                        StartCoroutine(Game2());
                    else
                        AC.Advice("다른 꼬륵이입니다!");
                }
                break;
            case "Npc1_3":
                if (play)
                {
                    if (Btns_clue[2].interactable)
                        StartCoroutine(Game3(true));
                    else
                        AC.Advice("다른 꼬륵이입니다!");
                }
                break;
            case "Plane_2":
                Find_Tissue(0);
                break;
            case "Plane_3":
                Find_Tissue(1);
                break;
            case "Plane_4":
                Find_Tissue(2);
                break;
            case "TissueTree":
                if (count == 3)
                {
                    tissueTree.SetActive(false);
                    particles_clue_object[3].Play();
                    StartCoroutine(Game3(false));
                }
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    #region Balance : 컵을 저울에 올렸을 때의 이벤트
    public void Balance(string target, int weight)
    {
        if (target.Equals("I_cupA"))
        {
            cup_A.sprite = cups[weight].GetComponent<Image>().sprite;
            cup_A.color = Color.white;
            weight_A = weight;
        }
        else
        {
            cup_B.sprite = cups[weight].GetComponent<Image>().sprite;
            cup_B.color = Color.white;
            weight_B = weight;
        }

        SoundManager.Instance.Play_effect(0);

        if (weight_A > weight_B)
            Balancing(15);
        else if (weight_A < weight_B)
            Balancing(-15);
        else
            Balancing(0);
    }
    #endregion

    #region Cup_Position : 컵의 순서를 정할 때의 이벤트
    public void Cup_Position(string target, Sprite sprite_cup)
    {
        switch (target)
        {
            case "I_cup_1":
                results[0].sprite = sprite_cup;
                results[0].color = Color.white;
                clear[0] = sprite_cup.name.Equals("cup_tumbler") ? true : false;
                break;
            case "I_cup_2":
                results[1].sprite = sprite_cup;
                results[1].color = Color.white;
                clear[1] = sprite_cup.name.Equals("cup_paper") ? true : false;
                break;
            case "I_cup_3":
                results[2].sprite = sprite_cup;
                results[2].color = Color.white;
                clear[2] = sprite_cup.name.Equals("cup_plastic") ? true : false;
                break;
        }
        SoundManager.Instance.Play_effect(0);

        if (clear[0] && clear[1] && clear[2])
            StartCoroutine(Game1(false));
    }
    #endregion

    public void BE_clue(int num) {
        string advice="";
        switch (num) {
            case 1:
                advice = "카페 사장 꼬륵이를 찾아가세요.";
                break;
            case 2:
                advice = "마트에서 나온 꼬륵이를 찾아가세요.";
                break;
            case 3:
                advice = "소풍 나온 꼬륵이를 찾아가세요.";
                break;
        }
        AC.Advice(advice);
    }

    //private 함수-----------------------------------------------------------------------------------
    void Balancing(int rot) {
        balance_lever.transform.localRotation = Quaternion.Euler(Vector3.forward * rot);
        balance_A.transform.localRotation = Quaternion.Euler(Vector3.back * rot);
        balance_B.transform.localRotation = Quaternion.Euler(Vector3.back * rot);
    }

    void Find_Tissue(int num) {
        if (count < 0)
            return;

        plains[num].SetActive(false);
        particles_clue_object[num].Play();

        count++;
        if (count == 1)
            AC.Dialog_and_Advice("play3_2");
        else
            AC.Advice("더 찾아주세요!");
    }

    void GameEnd(int num)
    {
        play = true;
        Clear_Clue(num + 1);
        mask[num].SetActive(false);
        Btns_clue[num].interactable = false;
        if(num < 2)
            Btns_clue[num + 1].interactable = true;
    }

    //코루틴 -----------------------------------------------------------------------------------
    IEnumerator Game1(bool start)
    {
        if (start)
            AC.Dialog_and_Advice("play1_1");
        else
            AC.Dialog_and_Advice("play1_2");

        yield return wait;

        blank.SetActive(start);

        if (start)
        {
            cmr.cullingMask = 1536;
            play = false;
        }
        else
        {
            cmr.cullingMask = -1;
            GameEnd(0);
        }
    }
    IEnumerator Game2()
    {
        AC.Dialog_and_Advice("play2");
        yield return wait;

        GameEnd(1);
    }
    IEnumerator Game3(bool start)
    {
        if (start)
        {
            AC.Dialog_and_Advice("play3_1");
            count = 0;
            play = false;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            AC.Dialog_and_Advice("play3_3");
        }

        yield return wait;

        if (!start)
            GameEnd(2);
    }

}
