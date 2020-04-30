using UnityEngine.UI;
using UnityEngine;

/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Do_Base의 버튼 이벤트를 담당하는 스크립트
 */
public class DoManager : MonoBehaviour {

    public Button b_tap_1, b_tap_2;

    Camera cmr;

    SoundManager SM;
    SceneController SC;

    void Start()
    {
        SM = SoundManager.Instance;
        SC = SceneController.Instance;
        cmr = Camera.main;
        cmr.enabled = false;
    }

    public void BE_Tap_1()
    {
        SM.Play_effect(0);
        SC.Load_Scene(12);
        b_tap_1.interactable = false;
        b_tap_2.interactable = true;
        b_tap_1.transform.Translate(Vector3.left * 40);
        b_tap_2.transform.Translate(Vector3.right * 40);
    }
    public void BE_Tap_2()
    {
        SM.Play_effect(0);
        SC.Load_Scene(13);
        b_tap_1.interactable = true;
        b_tap_2.interactable = false;
        b_tap_1.transform.Translate(Vector3.right * 40);
        b_tap_2.transform.Translate(Vector3.left * 40);
    }
    public void BE_Back()
    {
        cmr.enabled = true;
        SM.Play_effect(0);
        SC.Destroy_Scene();
    }
}
