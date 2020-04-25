using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    //할당 받을 객체
    public Texture tutorial1, tutorial2, tutorial3, tutorial4;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next;

    //커스텀 클래스 인스턴스
    SoundManager SM;

    //변수
    int page;
    
    void Start() {
        SM = SoundManager.Instance;
        page = PlayerPrefs.GetInt("tutorial_page");
        ChageImage(page);
    }

    public void Next() {
        SM.Play_effect(0);
        ChageImage(++page);
        arrow_pre.SetActive(true);
        arrow_next.SetActive(false);
    }

    public void Pre() {
        SM.Play_effect(0);
        ChageImage(--page);
        arrow_pre.SetActive(false);
        arrow_next.SetActive(true);
    }

    public void Skip()
    {
        SM.Play_effect(0);

        if (page < 3)
        {
            PlayerPrefs.SetInt("tutorial_page", 3);
            SceneController.Instance.Destroy_Scene();
        }
        else
        {
            PlayerPrefs.SetInt("tmp_Clear", 0);
            SceneController.Instance.SetPlanetNum(0);
            SceneController.Instance.Load_Scene(1);
        }
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
