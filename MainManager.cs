using UnityEngine;
using UnityEngine.UI;

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

    //상수
    private const string tmp_Stage = "tmp_Stage";

    //변수
    private int stage_num;
    Vector2 bigSize = new Vector2(960f, 960f);


    void Start()
    {
        stage_num = PlayerPrefs.GetInt(tmp_Stage, 0);

        stage_num /= 3;
        if (stage_num >= 0)
        {
            //토양행성은 기본적으로 활성화되어 있음
            
            //조건에 따라 나머지 행성들도 활성화
            if (stage_num >= 1)
            {
                // 수질행성 활성화
                btn_water.interactable = true;      
                btn_water.GetComponent<RectTransform>().sizeDelta = bigSize;
                t_water.color = Color.white;

                if (stage_num >= 2) {
                    // 대기행성 활성화
                    btn_air.interactable = true;  
                    btn_air.GetComponent<RectTransform>().sizeDelta = bigSize;
                    t_air.color = Color.white;
                }
            }

        }


    }


}