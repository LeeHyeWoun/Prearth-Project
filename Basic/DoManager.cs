using UnityEngine.UI;
using UnityEngine;

/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Do_Base의 버튼 이벤트를 담당하는 스크립트
 */
public class DoManager : MonoBehaviour {

    //UI
    public Button[] b_taps;
    public GameObject[] Go_contents;

    public void BE_Tap(int num)
    {
        SoundManager.Instance.Play_effect(0);
        for (int i = 0; i < b_taps.Length; i++)
        {
            if (i != num)
            {
                if (!b_taps[i].interactable)
                    b_taps[i].transform.localPosition += Vector3.right * 40;

                b_taps[i].interactable = true;
                Go_contents[i].SetActive(false);
            }
            else
            {
                b_taps[i].interactable = false;
                b_taps[i].transform.localPosition += Vector3.left * 40;
                Go_contents[i].SetActive(true);
            }
        }
    }
}
