using UnityEngine;

/**
 * Date     : 2020.01.23
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  소리에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> do 씬의 GameDirector(빈 오브젝트)
 */
public class SoundController_onlyEffect : MonoBehaviour {

    public AudioClip effect_no, effect_ok, effect_select;

    private GameObject GO;
    private AudioSource effect;

    private void Start()
    {
        GO = GameObject.Find("SoundManager");
        if (GO)
            effect = GO.GetComponent<SoundManager>().Player_effect;
        else
            print("SoundManager 없음\n소리 설정을 원한다면 '01_Main'으로 가주세요.");

    }

    //Effect 선택 Play
    public void Play_effect(int num)
    {
        if (GO) {
            AudioClip clip;
            switch (num)
            {
                case 1:
                    clip = effect_ok;
                    break;

                case 2:
                    clip = effect_no;
                    break;

                default:
                    clip = effect_select;
                    break;
            }
            effect.PlayOneShot(clip);
        }
    }
}
