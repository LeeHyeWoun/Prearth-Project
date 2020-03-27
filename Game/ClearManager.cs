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

    int count = 0;

    //임시 종료 처리
    public void Developer_Button() {
        if(clue1.GetComponent<Button>().interactable &&
            clue2.GetComponent<Button>().interactable &&
            clue3.GetComponent<Button>().interactable)
            switch (count)
            {
                case 0:
                    clue1.GetComponent<Image>().sprite = clue_clear;
                    blank.SetActive(false);
                    count++;
                    break;

                case 1:
                    clue2.GetComponent<Image>().sprite = clue_clear;
                    clue3.GetComponent<Image>().sprite = clue_clear;
                    Invoke("ending", 3f);
                    break;

            }
    }

    void ending() {
        //게임 마무리 대사
        gamedirector.GetComponent<DialogManager>().Dialog_Start("11_clear", null);
    }
}
