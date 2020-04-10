using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public Texture tutorial1, tutorial2, tutorial3, tutorial4;
    public RawImage RI_tutorial;
    public GameObject arrow_pre, arrow_next;

    int count = 1;

    SceneController SC;

    void Awake() {
        SC = GetComponent<SceneController>();

        if (PlayerPrefs.GetInt("tmp_Stage", -1) > -1)
            SC.Go(1);
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
        PlayerPrefs.SetInt("tmp_Stage", 0);
        SC.Go(1);
    }

    void ChageImage(int count) {
        switch (count)
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
