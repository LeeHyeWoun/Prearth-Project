using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.04.25
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 12_Do_Planet의 관리해주는 스크립트
 */
public class DoPlanetManager : MonoBehaviour {

    //UI
    public Image[] Imgs_planet;     //size = 3
    public GameObject[] Gos_deco;   //size = 3
    public Text[]
        Txts_title,                 //size = 3
        Txts_progress;              //size = 3
    public Image[]
    Imgs_ribon,                     //size = 3
    Imgs_mskTarget;                 //size = 3


    //Resource
    public Sprite[] Sprites_planet;

    //변수
    int clear;


    //상수
    const string ING = "조사중인 행성";
    readonly Color color_ing = new Color(32 / 255f, 40 / 255f, 70 / 255f);
    readonly Color color_done = new Color(50 / 255f, 50 / 255f, 50 / 255f);

    //초기화
    void Awake () {

        clear = PlayerPrefs.GetInt("tmp_Clear",0);

        //첫번째 이미지 설정-----------------------------------------------------------------------
        //미진행
        if (clear < 0)
            return;
        //진행중
        else if (clear < 3)
        {
            Txts_progress[0].text = ING;
            Txts_progress[0].color = color_ing;
        }
        //완료
        else
        {
            Gos_deco[0].SetActive(true);
            Imgs_planet[0].sprite = Sprites_planet[0];//Atlas.GetSprite("do_planet1_2");
            Txts_progress[0].text = PlayerPrefs.GetString("tmp_date_soil","null") + " 조사완료";
            Txts_progress[0].color = color_done;
            Txts_title[0].color = Color.white;
        }

        //두번째 이미지 설정-----------------------------------------------------------------------
        //미진행
        if (clear < 3)
            return;
        //진행중
        else if (clear < 6)
        {
            Txts_progress[1].text = ING;
            Txts_progress[1].color = color_ing;
        }
        //완료
        else
        {
            Gos_deco[1].SetActive(true);
            Imgs_planet[1].sprite = Sprites_planet[1];//Atlas.GetSprite("do_planet2_2");
            Txts_progress[1].text = PlayerPrefs.GetString("tmp_date_water", "null") + " 조사완료";
            Txts_progress[1].color = color_done;
            Txts_title[1].color = Color.white;
        }

        //세번째 이미지 설정-----------------------------------------------------------------------
        //미진행
        if (clear < 6)
            return;
        //진행중
        else if (clear < 9)
        {
            Txts_progress[2].text = ING;
            Txts_progress[2].color = color_ing;
        }
        //완료
        else
        {
            Gos_deco[2].SetActive(true);
            Imgs_planet[2].sprite = Sprites_planet[2];//Atlas.GetSprite("do_planet3_2");
            Txts_progress[2].text = PlayerPrefs.GetString("tmp_date_air", "null") + " 조사완료";
            Txts_progress[2].color = color_done;
            Txts_title[2].color = Color.white;
        }
    }

    void OnEnable()
    {
        if (clear > 2)
            Appear_Decos(0);
        if (clear > 5)
            Appear_Decos(1);
        if (clear > 8)
            Appear_Decos(2);
    }

    void Appear_Decos(int num) {
        StartCoroutine(routine_ribon(num));
        StartCoroutine(routine_Appear(num));
    }

    //코루틴 -----------------------------------------------------------------------------------------------------------------
    IEnumerator routine_ribon(int num) {
        Imgs_ribon[num].fillAmount = 0.2f;
        for (int i = 0; i < 20; i++)
        {
            Imgs_ribon[num].fillAmount += 0.04f;
            yield return null;
        }
    }

    IEnumerator routine_Appear(int num)
    {
        Imgs_mskTarget[num].color = Color.white;
        Imgs_mskTarget[num].transform.localPosition = Vector3.zero;

        for (float i = 1; i <= 20; i += 0.5f)
        {
            Imgs_mskTarget[num].transform.localPosition = Vector3.up * i;
            yield return null;
        }
    }
}
