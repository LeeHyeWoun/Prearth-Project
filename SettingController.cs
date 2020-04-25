/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene "14_Setting"의 모든 이벤트를 담당하는 스크립트
 */
public class SettingController : SoundController {

    public void BE_GoMain() {
        SoundManager.Instance.Play_effect(0);
        SceneController.Instance.Load_Scene(0);
    }
    public void BE_Play() {
        SoundManager.Instance.Play_effect(0);
        SceneController.Instance.Destroy_Scene();
    }

}
