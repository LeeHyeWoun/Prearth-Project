using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.01.23
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  소리에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> main, map, game씬의 GameDirector(빈 오브젝트)
 */
public class SoundController : MonoBehaviour
{
    public Slider s_bgm, s_effect;
    public AudioClip
        bgm_game, bgm_main, 
        effect_no, effect_ok, effect_select;

    private AudioSource bgm, effect;
    private GameObject GO;
    private float value_effect = 1f;
    private float value_bgm = 1f;

    //초기화
    void Start()
    {
        GO = GameObject.Find("SoundManager");
        if (GO)
        {
            SoundManager soundManager = GO.GetComponent<SoundManager>();

            bgm = soundManager.Player_BGM;
            effect = soundManager.Player_effect;

            //저장된 값 불러오기
            value_bgm = PlayerPrefs.GetFloat("bgm", 1f);
            value_effect = PlayerPrefs.GetFloat("effect", 1f);

            //저장된 값으로 슬라이더 설정
            s_bgm.value = value_bgm;
            s_effect.value = value_effect;

            //환경 설정 초기화
            bgm.volume = value_bgm;
            effect.volume = value_effect;
        }
        else
            print("SoundManager 없음\n소리 설정을 원한다면 '00_Tutorial'로 가주세요.");
    }

    //배경음악 설정
    public void setBGM()
    {
        if (GO)
        {
            value_bgm = s_bgm.value;
            bgm.volume = value_bgm;
            PlayerPrefs.SetFloat("bgm", value_bgm);
        }
        else {
            s_bgm.value = 0;
            print("'SoundManager'가 없어 소리 설정이 불가능합니다. ");
        }
    }

    //효과음 설정
    public void setEffect()
    {
        if (GO)
        {
            value_effect = s_effect.value;
            effect.volume = value_effect;
            PlayerPrefs.SetFloat("effect", value_effect);
        }
        else {
            s_effect.value = 0;
            print("'SoundManager'가 없어 소리 설정이 불가능합니다. ");
        }
    }

    //BGM 변경해서 Play
    public void Play_BGM(bool gameBGM)
    {
        if (GO) {
            if (gameBGM)
                bgm.clip = bgm_game;
            else
                bgm.clip = bgm_main;
            bgm.Play();
        }
    }

    //Effect 선택 Play
    public void Play_effect(int num)
    {
        if (GO)
        {
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

    //BGM 소리 크기 설정
    public void SetBGM_Volum(float v) {
        bgm.volume = v;
    }

    public bool IsSoundManager() {
        return GO;
    }
}
