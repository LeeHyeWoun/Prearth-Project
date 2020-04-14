using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 01_Main의 관리해주는 스크립트
 *  
 *  Applied Location :
 *  -> Scene# 01_Main의 GameDirector(빈 오브젝트)
 */
public class MainManager : MonoBehaviour
{
    //할당
    public Button btn_soil, btn_water, btn_air;
    public Text t_water, t_air;
    public Sprite sprite_water, sprite_air;

    //커스텀 클래스 인스턴스
    SceneController scenenCtrl;
    SoundController SoundCtrl;

    //변수
    Vector2 bigSize = new Vector2(960f, 960f);
    private int stage_num, planet;

    //상수
    private const string tmp_Stage = "tmp_Stage";

    void Awake() {
        stage_num = PlayerPrefs.GetInt(tmp_Stage, -1);

        if (stage_num < 0)
        {
            PlayerPrefs.SetInt("tutorial_page", 1);
            SceneManager.LoadSceneAsync("00_Tutorial", LoadSceneMode.Additive);
        }
    }

    void Start()
    {
        planet = stage_num/3;
        //토양행성은 기본적으로 활성화되어 있음
        //조건에 따라 나머지 행성들도 활성화
        if (planet >= 1)
        {
            // 수질행성 활성화
            btn_water.image.sprite = sprite_water;
            btn_water.GetComponent<RectTransform>().sizeDelta = bigSize;
            t_water.color = Color.white;

            if (planet >= 2)
            {
                // 대기행성 활성화
                btn_air.image.sprite = sprite_air;
                btn_air.GetComponent<RectTransform>().sizeDelta = bigSize;
                t_air.color = Color.white;
            }
        }

        scenenCtrl = GetComponent<SceneController>();
        SoundCtrl = GetComponent<SoundController>();

    }

    public void Go_map(int num) {
        switch (num)
        {
            case 1:
                SoundCtrl.Play_effect(0);
                if (stage_num < 0) {
                    PlayerPrefs.SetInt("tutorial_page", 3);
                    SceneManager.LoadSceneAsync("00_Tutorial", LoadSceneMode.Additive);
                }
                else
                    scenenCtrl.Go(2);

                break;

            case 2:
                if (planet >= 1) {
                    SoundCtrl.Play_effect(0);
                    scenenCtrl.Go(3);
                }
                else
                    SoundCtrl.Play_effect(2);
                break;

            case 3:
                if (planet >= 2) {
                    SoundCtrl.Play_effect(0);
                    scenenCtrl.Go(4);
                }
                else
                    SoundCtrl.Play_effect(2);
                break;
        }
    }

}