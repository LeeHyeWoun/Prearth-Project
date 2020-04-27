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

    public Text t_date_soil, t_date_water, t_date_air;
    public RawImage ri_soil, ri_water, ri_air;
    public Texture
        soil_ing,   soil_complete,
        water_ing,  water_complete,
        air_ing,    air_complete;

    void Awake () {

        int clear = PlayerPrefs.GetInt("tmp_Clear",0);

        if (clear < 0)
            return;
        else if (clear < 3)
            ri_soil.texture = soil_ing;
        else
        {
            ri_soil.texture = soil_complete;
            t_date_soil.text = PlayerPrefs.GetString("tmp_date_soil","null") + " 조사완료";
        }

        if (clear < 3)
            return;
        else if (clear < 6)
            ri_water.texture = water_ing;
        else
        {
            ri_water.texture = water_complete;
            t_date_water.text = PlayerPrefs.GetString("tmp_date_water", "null") + " 조사완료";
        }

        if (clear < 6)
            return;
        else if (clear < 9)
            ri_air.texture = air_ing;
        else
        {
            ri_air.texture = air_complete;
            t_date_air.text = PlayerPrefs.GetString("tmp_date_air", "null") + " 조사완료";
        }
    }
}
