using UnityEngine;
using UnityEngine.UI;

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

    // Audio players components.
    public AudioSource Player_BGM;
    public AudioSource Player_effect;

    // Singleton instance.
    public static SoundManager Instance = null;

    //싱글톤 구현
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
