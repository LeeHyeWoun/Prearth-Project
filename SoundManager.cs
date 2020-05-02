using UnityEngine;

/**
 * Date     : 2020.01.23
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  사운드의 기본 구성에 대한 스크립트
 *  
 *  Applied Location :
 *  -> SoundManager(빈 오브젝트)
 */
public class SoundManager : MonoBehaviour {

    // 싱글톤 인스턴스
    public static SoundManager Instance = null;

    // 할당 받을 오디오 관련 컴포넌트 및 리소스
    public AudioSource Player_BGM, Player_effect;
    public AudioClip
    bgm_game, bgm_main,
    effect_no, effect_ok, effect_select;


    //싱글톤 구현
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            SceneController.Instance = GetComponent<SceneController>();
        }
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    //소리값 초기화
    void Start()
    {
        Player_BGM.volume = PlayerPrefs.GetFloat("tmp_bgm", 1f);
        Player_effect.volume = PlayerPrefs.GetFloat("tmp_effect", 1f);
    }

    
    //Effect 선택 Play
    public void Play_effect(int num)
    {
        if (num.Equals(1))
            Player_effect.PlayOneShot(effect_ok);

        else if (num.Equals(2))
            Player_effect.PlayOneShot(effect_no);

        else
            Player_effect.PlayOneShot(effect_select);
    }

    //Game 끝내고 소리 재설정
    public void BGM_reset()
    {
        BGM_chage(false);
        Player_BGM.volume = PlayerPrefs.GetFloat("tmp_bgm", 1f);
    }

    public void BGM_chage(bool isGame)
    {
        if (isGame)
            Player_BGM.clip = bgm_game;
        else
            Player_BGM.clip = bgm_main;

        Player_BGM.Play();
    }

    //BGM 소리 크기 설정
    public void SetBGM_Volum(float v)
    {
        Player_BGM.volume = v;
    }
}
