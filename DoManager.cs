using UnityEngine.UI;
using UnityEngine;
using System.Collections;

/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Do_Base의 버튼 이벤트를 담당하는 스크립트
 */
public class DoManager : MonoBehaviour {

    //UI
    public Button b_tap_1, b_tap_2;
    public GameObject Go_Planet, Go_History;

    public void BE_Tap_1()
    {
        SoundManager.Instance.Play_effect(0);
        Go_Planet.SetActive(true);
        Go_History.SetActive(false);
        b_tap_1.interactable = false;
        b_tap_2.interactable = true;
        b_tap_1.transform.Translate(Vector3.left * 40f);
        b_tap_2.transform.Translate(Vector3.right * 40f);
    }
    public void BE_Tap_2()
    {
        SoundManager.Instance.Play_effect(0);
        Go_Planet.SetActive(false);
        Go_History.SetActive(true);
        b_tap_1.interactable = true;
        b_tap_2.interactable = false;
        b_tap_1.transform.Translate(Vector3.right * 40f);
        b_tap_2.transform.Translate(Vector3.left * 40f);
    }

}
