using UnityEngine;

//Do씬에 임시적으로 적용할 스크립트
public class DoManager : MonoBehaviour {

    //싱글톤의 SoundManager을 불러오기
    readonly SoundManager SM = SoundManager.Instance;

    public void button() {
        SM.Play_effect(0);
    }

}
