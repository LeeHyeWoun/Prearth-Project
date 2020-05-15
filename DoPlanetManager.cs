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
    public RawImage[] RIs_planet;       //size = 3
    public GameObject[] Gos_cause;      //size = 3
    public Text[] Txts_progress;        //size = 3

    //Resource
    public Texture[] Texturex_planet;   //size = 6

    //상수
    const string ING = "조사중인 행성";
    readonly Color color_ing = new Color(32 / 255f, 40 / 255f, 70 / 255f);
    readonly Color color_done = new Color(50 / 255f, 50 / 255f, 50 / 255f);

    //초기화
    void Awake () {

        int clear = PlayerPrefs.GetInt("tmp_Clear",0);

        //첫번째 이미지 설정-----------------------------------------------------------------------
        //미진행
        if (clear < 0)
            return;
        //진행중
        else if (clear < 3)
        {
            Txts_progress[0].text = ING;
            Txts_progress[0].color = color_ing;
            RIs_planet[0].texture = Texturex_planet[0];
        }
        //완료
        else
        {
            Gos_cause[0].SetActive(true);
            RIs_planet[0].texture = Texturex_planet[1];
            Txts_progress[0].text = PlayerPrefs.GetString("tmp_date_soil","null") + " 조사완료";
            Txts_progress[0].color = color_done;
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
            RIs_planet[1].texture = Texturex_planet[2];
        }
        //완료
        else
        {
            Gos_cause[1].SetActive(true);
            RIs_planet[1].texture = Texturex_planet[3];
            Txts_progress[1].text = PlayerPrefs.GetString("tmp_date_water", "null") + " 조사완료";
            Txts_progress[1].color = color_done;
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
            RIs_planet[2].texture = Texturex_planet[4];
        }
        //완료
        else
        {
            Gos_cause[2].SetActive(true);
            RIs_planet[2].texture = Texturex_planet[5];
            Txts_progress[2].text = PlayerPrefs.GetString("tmp_date_air", "null") + " 조사완료";
            Txts_progress[2].color = color_done;
        }
    }
}
