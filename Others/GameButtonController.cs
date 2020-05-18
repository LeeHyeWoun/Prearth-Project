﻿using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.03.19
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  게임 Scene 클리어에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> 
 */

public class GameButtonController : MonoBehaviour
{

    public Image clue1, clue2, clue3;
    public GameObject itemBox;
    public Sprite clue_clear;

    //임시 종료 처리
    public void Developer_Button()
    {
        clue1.sprite = clue_clear;
        clue2.sprite = clue_clear;
        clue3.sprite = clue_clear;

        Invoke("Ending", 1f);
    }

    //단서 해결 이벤트
    public void Clear_Clue(int num)
    {
        switch (num)
        {
            case 1:
                clue1.sprite = clue_clear;
                if (clue2.sprite.Equals(clue_clear) && clue3.sprite.Equals(clue_clear))
                    Invoke("Ending", 1f);
                break;
            case 2:
                clue2.sprite = clue_clear;
                if (clue1.sprite.Equals(clue_clear) && clue3.sprite.Equals(clue_clear))
                    Invoke("Ending", 1f);
                break;
            case 3:
                clue3.sprite = clue_clear;
                if (clue1.sprite.Equals(clue_clear) && clue2.sprite.Equals(clue_clear))
                    Invoke("Ending", 1f);
                break;
        }
    }

    //아이템박스 열고 닫기
    //Apply to 'B_Item'
    public void ItemBox()
    {
        SoundManager.Instance.Play_effect(0);
        if (itemBox.activeSelf)
            itemBox.SetActive(false);
        else
            itemBox.SetActive(true);
    }

    //게임 마무리 대사
    void Ending()
    {
        GetComponent<AdviceController>().Dialog_and_Advice("clear");
    }
}