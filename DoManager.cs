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
    public Image[]
        Imgs_ribon,
        Imgs_mskTarget;

    //변수
    int clear;

    void Start() {
        clear = PlayerPrefs.GetInt("tmp_Clear", 0);
        Appear_decos();
    }

    public void BE_Tap_1()
    {
        SoundManager.Instance.Play_effect(0);
        Go_Planet.SetActive(true);
        Go_History.SetActive(false);
        b_tap_1.interactable = false;
        b_tap_2.interactable = true;
        b_tap_1.transform.Translate(Vector3.left * 40);
        b_tap_2.transform.Translate(Vector3.right * 40);
        Appear_decos();
    }
    public void BE_Tap_2()
    {
        SoundManager.Instance.Play_effect(0);
        Go_Planet.SetActive(false);
        Go_History.SetActive(true);
        b_tap_1.interactable = true;
        b_tap_2.interactable = false;
        b_tap_1.transform.Translate(Vector3.right * 40);
        b_tap_2.transform.Translate(Vector3.left * 40);
    }

    void Appear_decos() {
        if (clear > 2)
            StartCoroutine(Appear_deco(0));
        if (clear > 5)
            StartCoroutine(Appear_deco(1));
        if (clear > 8)
            StartCoroutine(Appear_deco(2));
    }

    IEnumerator Appear_deco(int num)
    {
        Imgs_ribon[num].fillAmount = 0.2f;
        Imgs_mskTarget[num].transform.Translate(Vector3.down * 20);

        for(int i=0; i<40; i++)
        {
            if(Imgs_ribon[num].fillAmount < 1)
                Imgs_ribon[num].fillAmount += 0.04f; 
            Imgs_mskTarget[num].transform.Translate(Vector3.up * 0.5f);
            yield return null;
        }
    }

}
