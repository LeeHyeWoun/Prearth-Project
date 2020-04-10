using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    //할당 받을 객체
    public Texture tutorial1, tutorial2, tutorial3, tutorial4;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next, gameDirector;

    //커스텀 클래스 인스턴스
    SoundController SC;

    //변수
    int page = 1;
    int clear;
    
    void Awake() {
        clear = PlayerPrefs.GetInt("tmp_Stage", -1);
        if (clear > -1)
            Destroy(gameObject);
    }

    void Start() {
        SC = GameObject.Find("GameDirector").GetComponent<SoundController>();
    }

    public void Next() {
        SC.Play_effect(0);
        ChageImage(page++);
        arrow_pre.SetActive(true);
        arrow_next.SetActive(false);
    }

    public void Pre() {
        SC.Play_effect(0);
        ChageImage(page--);
        arrow_pre.SetActive(false);
        arrow_next.SetActive(true);
    }

    public void Skip()
    {
        SC.Play_effect(0);
        if (page == 1)
        {
            page = 3;
            gameObject.SetActive(false);
            ChageImage(page);
        }
        else {
            PlayerPrefs.SetInt("tmp_Stage", ++clear);
            gameDirector.GetComponent<SceneController>().Go(2);
        }
    }

    void ChageImage(int page) {
        switch (page)
        {
            case 1:
                RI_tutorial.texture = tutorial1;
                break;

            case 2:
                RI_tutorial.texture = tutorial2;
                break;

            case 3:
                RI_tutorial.texture = tutorial3;
                break;

            case 4:
                RI_tutorial.texture = tutorial4;
                break;
        }
    }

}
