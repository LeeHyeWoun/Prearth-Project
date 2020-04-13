using UnityEngine;
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

public class ClearManager : MonoBehaviour {

    public GameObject blank, gamedirector, clue1, clue2, clue3;
    public Sprite clue_clear;

    //임시 종료 처리
    public void Developer_Button() {

        GameObject[] clue = { clue1, clue2, clue3};
        for (int i = 0; i < 3; i++)
        {
            clue[i].GetComponent<Image>().sprite = clue_clear;
            clue[i].GetComponent<Button>().interactable = true;
        }
        blank.SetActive(false);
        Invoke("Ending", 3f);
    }

    public void Clear_Clue(int num) {
        GameObject clue;
        switch (num) {
            case 1:
                clue = clue1;
                break;
            case 2:
                clue = clue2;
                break;
            default:
                clue = clue3;
                break;
        }
        clue.GetComponent<Image>().sprite = clue_clear;
        clue.GetComponent<Button>().interactable = true;

    }

    void Ending() {
        //게임 마무리 대사
        gamedirector.GetComponent<DialogManager>().Dialog_Start("11_clear", "end");
    }

}
