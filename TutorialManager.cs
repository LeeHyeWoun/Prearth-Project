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

    //할당 받을 객체
    public Texture tutorial1, tutorial2, tutorial3, tutorial4, tutorial5, tutorial6;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next;

    //커스텀 클래스 인스턴스
    SoundManager SM;

    //변수
    int page = 1;
    
    void Start() {
        Time.timeScale = 0;
        SM = SoundManager.Instance;

        if (SceneController.Instance.GetActiveScene_num().Equals(1))
            ChageImage(page = 5);
    }

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

    void ChageImage(int page) {
        Texture texture;
        switch (page)
        {
            case 2:
                texture = tutorial2;
                break;

            case 3:
                texture = tutorial3;
                break;

            case 4:
                texture = tutorial4;
                break;

            case 5:
                texture = tutorial5;
                break;

            case 6:
                texture = tutorial6;
                break;

            default:
                texture = tutorial1;
                break;
        }
        RI_tutorial.texture = texture;
    }

}
