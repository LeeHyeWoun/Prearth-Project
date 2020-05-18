using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 14_Tutorial의 관리해주는 스크립트
 */
public class TutorialManager : MonoBehaviour {

    //UI
    public GameObject arrow_pre, arrow_next;
    public RawImage RI_tutorial;

    //Resource
    public Texture[] Textures_tutorial; //size = 6

    //변수
    SoundManager SM;
    int page = 1;


    //초기화------------------------------------------------------------------------
    void Start() {
        Time.timeScale = 0;
        SM = SoundManager.Instance;

        if (SceneController.Instance.GetActiveScene_num().Equals(1))
            ChageImage(page = 5);
    }


    //private 함수------------------------------------------------------------------------
    void ChageImage(int page)
    {
        RI_tutorial.texture = Textures_tutorial[page - 1];
    }

    //public 함수-------------------------------------------------------------------------
    public void Next() {
        SM.Play_effect(0);

        if (page.Equals(1))
            arrow_pre.SetActive(true);
        else if (page.Equals(3))
            arrow_next.SetActive(false);
        else if (page.Equals(5))
        {
            arrow_pre.SetActive(true);
            arrow_next.SetActive(false);
        }

        ChageImage(++page);
    }

    public void Pre() {
        SM.Play_effect(0);

        if (page.Equals(2))
            arrow_pre.SetActive(false);
        else if (page.Equals(4))
            arrow_next.SetActive(true);
        else if (page.Equals(6))
        {
            arrow_pre.SetActive(false);
            arrow_next.SetActive(true);
        }

        ChageImage(--page);
    }

    public void Skip()
    {
        SM.Play_effect(0);

        PlayerPrefs.SetInt("tmp_Clear", page < 5 ? -1 : 0);
        Time.timeScale = 1;
        SceneController.Instance.Destroy_Scene();
    }
}
