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
    public Text[] Txts_name;            //size = 3

    //Resource
    public Texture[] Texturex_planet;   //size = 6


    //초기화
    void Awake () {

        int clear = PlayerPrefs.GetInt("tmp_Clear",0);

        if (clear < 0)
            return;
        else if (clear < 3)
            RIs_planet[0].texture = Texturex_planet[0];
        else
        {
            RIs_planet[0].texture = Texturex_planet[1];
            Txts_name[0].text = PlayerPrefs.GetString("tmp_date_soil","null") + " 조사완료";
        }

        if (clear < 3)
            return;
        else if (clear < 6)
            RIs_planet[1].texture = Texturex_planet[2];
        else
        {
            RIs_planet[1].texture = Texturex_planet[3];
            Txts_name[1].text = PlayerPrefs.GetString("tmp_date_water", "null") + " 조사완료";
        }

        if (clear < 6)
            return;
        else if (clear < 9)
            RIs_planet[2].texture = Texturex_planet[4];
        else
        {
            RIs_planet[2].texture = Texturex_planet[5];
            Txts_name[2].text = PlayerPrefs.GetString("tmp_date_air", "null") + " 조사완료";
        }
    }
}
