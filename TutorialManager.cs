using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public Sprite tutorial1, tutorial2, tutorial3, tutorial4;
    public Image P_tutorial;
    public GameObject arrow_pre, arrow_next;

    int count = 1;

    SceneController SC;

    void Start() {
        SC = GetComponent<SceneController>();

        if (PlayerPrefs.GetInt("tmp_Stage") > 0)
            Skip();

    }

    public void Next() {
        if (count < 4) {
            count++;
            ChageImage(count);

            if (count == 4)
                arrow_next.SetActive(false);
            else if (count == 2)
                arrow_pre.SetActive(true);

        }
    }

    public void Pre() {
        if (count > 1) {
            count--;
            ChageImage(count);

            if (count == 1)
                arrow_pre.SetActive(false);
            else if (count == 3)
                arrow_next.SetActive(true);
        }
    }

    public void Skip()
    {
        SC.Go(1);
    }

    void ChageImage(int count) {
        switch (count)
        {
            case 1:
                P_tutorial.sprite = tutorial1;
                break;

            case 2:
                P_tutorial.sprite = tutorial2;
                break;

            case 3:
                P_tutorial.sprite = tutorial3;
                break;

            case 4:
                P_tutorial.sprite = tutorial4;
                break;
        }
    }

}
