using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.01.23
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  소리에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  2020.04.15 수정 사항 : 
 *  SoundManager와 같은 위치에 스크립트를 적용, MainManager, MapManager등 각 씬의 Manager가 씬이 로드될 때 최초로 한번만 SoundManager를 Find하여 사용
 *  
 *  Applied Location :
 *  -> SoundManager(빈 오브젝트)
 */
public class SoundController : MonoBehaviour
{
    //객체 할당
    public Slider s_bgm, s_effect;

    private AudioSource bgm, effect;
    private AudioClip
        effect_no, effect_ok, effect_select;
    
    //변수
    private float value_effect = 1f;
    private float value_bgm = 1f;

    //싱글톤의 SoundManager을 불러오기
    readonly SoundManager SM = SoundManager.Instance;


    //초기화
    void Start()
    {
        if (SM)
        {
            bgm = SM.Player_BGM;
            effect = SM.Player_effect;
            effect_no = SM.effect_no;
            effect_ok = SM.effect_ok;
            effect_select = SM.effect_select;

            //슬라이더 초기화
            if (s_bgm != null || s_effect != null) {
                s_bgm.value = bgm.volume;
                s_effect.value = effect.volume;
            }
        }
        else
            print("SoundManager 없음\n소리 설정을 원한다면 '00_Tutorial'로 가주세요.");
    }

    //배경음악 설정
    public void SetBGM()
    {
        if (SM)
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
    public void SetEffect()
    {
        if (SM)
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

    //Effect 선택 Play
    public void Play_effect(int num)
    {
        if (SM)
            SM.Play_effect(num);
    }

    //BGM 소리 크기 설정
    public void SetBGM_Volum(float v) {
        bgm.volume = v;
    }

}
