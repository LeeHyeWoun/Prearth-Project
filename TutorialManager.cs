using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    //할당 받을 객체
    public Texture tutorial1, tutorial2, tutorial3, tutorial4;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next;

    //커스텀 클래스 인스턴스
    SoundController SC;

    //변수
    int page;
    
    void Start() {
        GameObject GO = GameObject.Find("GameDirector");
        if (GO)
        {
            SC = GO.GetComponent<SoundController>();
            page = PlayerPrefs.GetInt("tutorial_page", 1);
            ChageImage(page);
            print("튜토리얼 모드 ************************************************************");
        }
        else
            SceneManager.LoadScene(0);
    }

    public void Next() {
        SC.Play_effect(0);
        ChageImage(++page);
        arrow_pre.SetActive(true);
        arrow_next.SetActive(false);
    }

    public void Pre() {
        SC.Play_effect(0);
        ChageImage(--page);
        arrow_pre.SetActive(false);
        arrow_next.SetActive(true);
    }

    public void Skip()
    {
        SC.Play_effect(0);

        if (page < 3)
        {
            PlayerPrefs.SetInt("tutorial_page", 3);
            SceneManager.UnloadSceneAsync("00_Tutorial");
        }
        else
        {
            PlayerPrefs.SetInt("tmp_Stage", 0);
            PlayerPrefs.SetInt("TheMapIs", 1);

            print("튜토리얼 종료 ************************************************************");
            print("****************************** [" + SceneManager.GetActiveScene().name + "] --> [" + "02_Map" + "] ******************************");
            SceneManager.LoadScene("02_Map");
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
