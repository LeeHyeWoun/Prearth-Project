using UnityEngine;

public class DefaultController : MonoBehaviour {

    public GameObject Go_diary, Go_setting, Go_back;

    int culling;

    public void BE_Diary() {
        Go_diary.SetActive(false);
        Go_setting.SetActive(false);
        Go_back.SetActive(true);
        culling = Camera.main.cullingMask;
        Camera.main.cullingMask = (1<<9);
    }

    public void BE_Back()
    {
        Go_diary.SetActive(true);
        Go_setting.SetActive(true);
        Go_back.SetActive(false);
        Camera.main.cullingMask = culling;
    }
}
