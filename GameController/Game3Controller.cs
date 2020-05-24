using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game3Controller : GameController {

    //3D Object
    public GameObject
        blank, balance_lever, balance_A, balance_B;
    public Image cup_A, cup_B;

    public GameObject[] plains;//size = 4
    public Image[] results; //size = 3
    public Button[] cups;   //size = 3

    //변수
    bool[] clear = new bool[3] { false, false, false };
    int weight_A, weight_B = -1;

    //상수
    WaitForSeconds wait = new WaitForSeconds(0.1f);

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "npc1":
                if (Btns_clue[0].interactable)
                    StartCoroutine(Game1(true));
                break;
            case "npc2":
                if (Btns_clue[1].interactable)
                    StartCoroutine(Game2());
                break;
            case "npc3":
                StartCoroutine(Game3(true));
                break;
            //case "Plane_1":
            //    plains[0].SetActive(false);
            //    break;
            case "Plane_2":
                plains[1].SetActive(false);
                break;
            case "Plane_3":
                plains[2].SetActive(false);
                break;
            case "Plane_4":
                plains[3].SetActive(false);
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
                clear[0] = sprite_cup.name.Equals("item3") ? true : false;
                break;
            case "I_cup_2":
                results[1].sprite = sprite_cup;
                results[1].color = Color.white;
                clear[1] = sprite_cup.name.Equals("item1") ? true : false;
                break;
            case "I_cup_3":
                results[2].sprite = sprite_cup;
                results[2].color = Color.white;
                clear[2] = sprite_cup.name.Equals("item2") ? true : false;
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
                StartCoroutine(Game3(false)); //임시
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

    IEnumerator Game1(bool start)
    {
        int cullingMask;
        if (start)
        {
            AC.Dialog_and_Advice("play1_1");
            cullingMask = 1536;
        }
        else
        {
            AC.Dialog_and_Advice("play1_2");
            cullingMask = -1;
        }
        yield return wait;

        cmr.cullingMask = cullingMask;
        blank.SetActive(start);

        if (!start)
        {
            Btns_clue[0].interactable = false;
            Clear_Clue(1);
        }
    }
    IEnumerator Game2()
    {
        AC.Dialog_and_Advice("3_play2");
        yield return wait;

        Btns_clue[1].interactable = false;
        Clear_Clue(2);
    }
    IEnumerator Game3(bool start)
    {
        if (start)
            AC.Dialog_and_Advice("play3_1");
        else
            AC.Dialog_and_Advice("play3_2");
        yield return wait;

        if (!start)
        {
            Btns_clue[2].interactable = false;
            Clear_Clue(3);
        }
    }



}
