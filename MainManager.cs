using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 00_Main의 관리해주는 스크립트
 */
public class MainManager : MonoBehaviour
{
    //할당
    public GameObject halo_water, halo_air;
    public Text t_water, t_air;
    public Button btn_soil, btn_water, btn_air;
    public Sprite sprite_water, sprite_air;

    //변수
    int stage_num, planet;

    //런타임 상수
    readonly Vector2 bigSize = new Vector2(652f, 652f);

    //커스텀 클래스 인스턴스
    SoundManager SM;



    void Awake() {
        stage_num = PlayerPrefs.GetInt("tmp_Clear", -1);

        if (stage_num < 0)
        {
            PlayerPrefs.SetInt("tmp_tutorial", 1);
            SceneController.Instance.Load_Scene(14);
        }
    }

    void Start()
    {
        //싱글톤의 SoundManager을 불러오기...Main에서는 꼭 Start에서 할당받아야함
        SM = SoundManager.Instance;

        planet = stage_num/3;
        //토양행성은 기본적으로 활성화되어 있음
        //조건에 따라 나머지 행성들도 활성화
        if (planet >= 1)
        {
            // 수질행성 활성화
            halo_water.SetActive(true);
            btn_water.image.sprite = sprite_water;
            btn_water.GetComponent<RectTransform>().sizeDelta = bigSize;
            t_water.color = Color.white;

            if (planet >= 2)
            {
                // 대기행성 활성화
                halo_air.SetActive(true);
                btn_air.image.sprite = sprite_air;
                btn_air.GetComponent<RectTransform>().sizeDelta = bigSize;
                t_air.color = Color.white;
            }
        }


    }

    public void Go_map(int num) {
        switch (num)
        {
            case 1:
                SM.Play_effect(0);
                if (stage_num < 0)
                {
                    PlayerPrefs.SetInt("tutorial_page", 3);
                    SceneController.Instance.Load_Scene(14);
                }
                else {
                    SceneController.Instance.SetPlanetNum(0);
                    SceneController.Instance.Load_Scene(1);
                }

                break;

            case 2:
                if (planet >= 1) {
                    SM.Play_effect(0);
                    SceneController.Instance.SetPlanetNum(1);
                    SceneController.Instance.Load_Scene(1);
                }
                else
                    SM.Play_effect(2);
                break;

            case 3:
                if (planet >= 2) {
                    SM.Play_effect(0);
                    SceneController.Instance.SetPlanetNum(2);
                    SceneController.Instance.Load_Scene(1);
                }
                else
                    SM.Play_effect(2);
                break;
        }
    }

}