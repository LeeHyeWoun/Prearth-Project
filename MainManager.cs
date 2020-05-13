using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 00_Main의 관리해주는 스크립트
 */
public class MainManager : MonoBehaviour
{
    //UI
    public Text t_water, t_air;
    public Button btn_water, btn_air;
    public GameObject[] Gameobjects_halo;   //size = 3

    //Resource
    public Sprite sprite_water, sprite_air;

    //변수
    SoundManager SM;
    int stage_num, planet;

    //상수
    readonly Vector2 bigSize = new Vector2(652f, 652f);
    readonly WaitForSeconds wait = new WaitForSeconds(0.02f);


    //초기화----------------------------------------------------------------------------------------------
    void Awake() {
        stage_num = PlayerPrefs.GetInt("tmp_Clear", -2);

        if (stage_num.Equals(-2))
            SceneController.Instance.Load_Scene(14);
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
            Gameobjects_halo[1].SetActive(true);
            btn_water.image.sprite = sprite_water;
            btn_water.GetComponent<RectTransform>().sizeDelta = bigSize;
            t_water.color = Color.white;

            if (planet >= 2)
            {
                // 대기행성 활성화
                Gameobjects_halo[2].SetActive(true);
                btn_air.image.sprite = sprite_air;
                btn_air.GetComponent<RectTransform>().sizeDelta = bigSize;
                t_air.color = Color.white;
            }
        }

        StartCoroutine("routine_halo_soil");
        if(Gameobjects_halo[1].activeSelf)
            StartCoroutine("routine_halo_water");
        if(Gameobjects_halo[2].activeSelf)
            StartCoroutine("routine_halo_air");
    }

    //public 함수----------------------------------------------------------------------------------------
    public void Go_map(int num) {
        switch (num)
        {
            case 1:
                SM.Play_effect(0);
                SceneController.Instance.Planet_num = 0;
                SceneController.Instance.Load_Scene(1);
                break;

            case 2:
                if (planet >= 1) {
                    SM.Play_effect(0);
                    SceneController.Instance.Planet_num = 1;
                    SceneController.Instance.Load_Scene(1);
                }
                else
                    SM.Play_effect(2);
                break;

            case 3:
                if (planet >= 2) {
                    SM.Play_effect(0);
                    SceneController.Instance.Planet_num = 2;
                    SceneController.Instance.Load_Scene(1);
                }
                else
                    SM.Play_effect(2);
                break;
        }
    }

    //코루틴----------------------------------------------------------------------------------------------
    IEnumerator routine_halo_soil()
    {
        while (true) {
            for (int i = 250; i > 200; i--)
            {
                Gameobjects_halo[0].transform.localScale = Vector3.one * (0.004f * i);//1~0.8
                yield return wait;
            }
            for (int i = 200; i < 250; i++)
            {
                Gameobjects_halo[0].transform.localScale = Vector3.one * (0.004f * i);
                yield return wait;
            }
        }
    }
    IEnumerator routine_halo_water()
    {
        while (true)
        {
            for (int i = 250; i > 200; i--)
            {
                Gameobjects_halo[1].transform.localScale = Vector3.one * (0.004f * i);
                yield return wait;
            }
            for (int i = 200; i < 250; i++)
            {
                Gameobjects_halo[1].transform.localScale = Vector3.one * (0.004f * i);
                yield return wait;
            }
        }
    }
    IEnumerator routine_halo_air()
    {
        while (true)
        {
            for (int i = 250; i > 200; i--)
            {
                Gameobjects_halo[2].transform.localScale = Vector3.one * (0.004f * i);
                yield return wait;
            }
            for (int i = 200; i < 250; i++)
            {
                Gameobjects_halo[2].transform.localScale = Vector3.one * (0.004f * i);
                yield return wait;
            }
        }
    }
}