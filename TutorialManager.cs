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
    public Texture tutorial1, tutorial2, tutorial3, tutorial4;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next;

    //커스텀 클래스 인스턴스
    SoundManager SM;

    //변수
    int page = 1;
    
    void Start() {
        Time.timeScale = 0;
        SM = SoundManager.Instance;
    }

    public void Next() {
        SM.Play_effect(0);
        ChageImage(++page);

        if (page.Equals(2))
            arrow_pre.SetActive(true);
        else if (page.Equals(4))
            arrow_next.SetActive(false);
    }

    public void Pre() {
        SM.Play_effect(0);
        ChageImage(--page);
        if (page.Equals(1))
            arrow_pre.SetActive(false);
        else if (page.Equals(3))
            arrow_next.SetActive(true);
    }

    public void Skip()
    {
        SM.Play_effect(0);
        PlayerPrefs.SetInt("tmp_Clear", 0);
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

            default:
                texture = tutorial1;
                break;
        }
        RI_tutorial.texture = texture;
    }

}
