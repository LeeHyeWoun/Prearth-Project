using UnityEngine;
using UnityEngine.UI;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  Scene# 02_Map의 관리해주는 스크립트
 *  
 *  Applied Location :
 *  -> Scene# 02_Map의 GameDirector(빈 오브젝트)
 */

public class MapManager : MonoBehaviour
{

    //할당
    public Button btn_1, btn_2, btn_3;                                      //stage 포인트
    public Text T_Status;                                                   //상태 알림
    public RawImage RI_gem, RI_map;                                         //보석과 지도
    public Texture 
        texture_gem1_0, texture_gem1_1, texture_gem1_2, texture_gem1_3,     //대기 행성의 보석
        texture_gem2_0, texture_gem2_1, texture_gem2_2, texture_gem2_3,     //수질 행성의 보석
        texture_gem3_0, texture_gem3_1, texture_gem3_2, texture_gem3_3,     //대기 행성의 보석
        texture_map_soil, texture_map_water, texture_map_air;               //지도 이미지
    public Sprite
        texture_mapPoint_soil, texture_mapPoint_water, texture_mapPoint_air;//지도 스테이지 이미지

    //변수
    int stage_num;
    Texture[,] gems= new Texture[3,3];

    //초기화
    void Start()
    {
        //데이터 불러오기
        stage_num = PlayerPrefs.GetInt("tmp_Stage", 0);
        int mapNum = PlayerPrefs.GetInt("TheMapIs", 1);

        //행성 디자인 설정
        Sprite mp;
        switch (mapNum)
        {
            case 1:
                T_Status.text = "페페행성";
                RI_map.texture = texture_map_soil;
                mp = texture_mapPoint_soil;
                break;

            case 2:
                T_Status.text = "도도행성";
                RI_map.texture = texture_map_water;
                mp = texture_mapPoint_water;
                break;

            default:
                T_Status.text = "라라행성";
                RI_map.texture = texture_map_air;
                mp = texture_mapPoint_air;
                break;
        }
        btn_1.image.sprite = mp;
        btn_2.image.sprite = mp;
        btn_3.image.sprite = mp;

        // 행성 진행 상태
        // 이미 클리어한 행성일 경우.................................................................................
        if ((mapNum == 1 && stage_num >= 3) ||
            (mapNum == 2 && stage_num >= 6) ||
            (mapNum == 3 && stage_num >= 9))
        {
            //상태창 내용 설정
            T_Status.text += "의 연료를 모두 모았습니다.";

            //보석 설정
            switch (mapNum)
            {
                case 1:
                    RI_gem.texture = texture_gem1_3;
                    break;
                case 2:
                    RI_gem.texture = texture_gem2_3;
                    break;
                default:
                    RI_gem.texture = texture_gem3_3;
                    break;
            }

            //맵 활성화
            btn_2.interactable = true;
            btn_3.interactable = true;
        }
        // 진행중인 행성일 경우.............................................................................
        else
        {
            //상태창 내용 설정
            stage_num %= 3;
            T_Status.text += " " + (3 - stage_num).ToString() + "조각 남았습니다.";

            //보석 설정
            gems[0, 0] = texture_gem1_0;
            gems[0, 1] = texture_gem1_1;
            gems[0, 2] = texture_gem1_2;
            gems[1, 0] = texture_gem2_0;
            gems[1, 1] = texture_gem2_1;
            gems[1, 2] = texture_gem2_2;
            gems[2, 0] = texture_gem3_0;
            gems[2, 1] = texture_gem3_1;
            gems[2, 2] = texture_gem3_2;
            RI_gem.texture = gems[mapNum - 1, stage_num];

            //맵 활성화
            if (stage_num >= 1)
            {
                btn_2.interactable = true;      //stage2 활성화

                if (stage_num >= 2)
                    btn_3.interactable = true; //stage3 활성화
            }

        }
    }


}
