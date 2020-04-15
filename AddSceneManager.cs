using System.Collections;
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
 *  -> 08,09,10 Scene의 최상위 객체(canvas)
 */
public class AddSceneManager : MonoBehaviour {

    public Image color_filter;

    GameObject GO_Sound;

    //커스텀 클래스 인스턴스
    SoundController SC;


    void Start () {
        GO_Sound = GameObject.Find("GameDirector");
        if (GO_Sound) {
            SC = GO_Sound.GetComponent<SoundController>();
        }

        if (color_filter) {
            StartCoroutine("Routine_FadeIn");
        }

    }

    public void Play_Effect_0() {
        if (GO_Sound)
            SC.Play_effect(0);
    }

    public void BGM_Reset() {
        if (GO_Sound)
            SC.BGM_reset();
    }

    IEnumerator Routine_FadeIn()
    {
        Color color = color_filter.color;
        float origin = color.a;
        int time = 0;
        while (time < 10)
        {
            time++;
            color.a = origin * time * 0.1f;
            color_filter.color = color;
            yield return null;
        }
    }

}
