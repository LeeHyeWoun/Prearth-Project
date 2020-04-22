using UnityEngine;

//Do씬에 임시적으로 적용할 스크립트
public class DoManager : MonoBehaviour {

    SceneController SC;

    void Start() {
        SC = GetComponent<SceneController>();
    }

    public void button(int num) {
        SoundManager.Instance.Play_effect(0);

        switch (num) {
            case 1:
                SC.Go(5);
                break;
            case 2:
                SC.Go(6);
                break;
            case 3:
                SC.Go(7);
                break;
            default:
                SC.Destroy_Scene();
                break;
        }
    }

}
