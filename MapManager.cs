using System.Collections;
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
    int clear_num;
    string str_origin = "";

    //커스텀 클래스의 인스턴스    
    SceneController SC;


    //초기화
    void Start()
    {
        SC = SceneController.Instance;

        //데이터 불러오기
        clear_num = PlayerPrefs.GetInt("tmp_Clear", 0);
        int planet_num = SC.GetPlanetNum();

        //행성 디자인 설정
        Sprite mp;
        switch (planet_num)
        {
            case 0:
                str_origin = "페페행성";
                RI_map.texture = texture_map_soil;
                mp = texture_mapPoint_soil;
                break;

            case 1:
                str_origin = "도도행성";
                RI_map.texture = texture_map_water;
                mp = texture_mapPoint_water;
                break;

            default:
                str_origin = "라라행성";
                RI_map.texture = texture_map_air;
                mp = texture_mapPoint_air;
                break;
        }

        // 행성 진행 상태
        // 이미 클리어한 행성일 경우.................................................................................
        if ((planet_num == 0 && clear_num >= 3) ||
            (planet_num == 1 && clear_num >= 6) ||
            (planet_num == 2 && clear_num >= 9))
        {
            //상태창 내용 설정
            str_origin += "의 연료를 모두 모았습니다.";

            //보석 설정
            switch (planet_num)
            {
                case 0:
                    RI_gem.texture = texture_gem1_3;
                    break;
                case 1:
                    RI_gem.texture = texture_gem2_3;
                    break;
                default:
                    RI_gem.texture = texture_gem3_3;
                    break;
            }

            //맵 활성화
            btn_1.image.sprite = mp;
            btn_2.image.sprite = mp;
            btn_3.image.sprite = mp;

            //맵 활성화
            btn_2.interactable = true;
            btn_3.interactable = true;
        }
        // 진행중인 행성일 경우.............................................................................
        else
        {
            //상태창 내용 설정
            clear_num %= 3;
            str_origin += " " + (3 - clear_num).ToString() + "조각 남았습니다.";

            //보석 설정
            switch (planet_num) {
                case 0:
                    if (clear_num == 0)
                        RI_gem.texture = texture_gem1_0;
                    else if (clear_num == 1)
                        RI_gem.texture = texture_gem1_1;
                    else
                        RI_gem.texture = texture_gem1_2;
                    break;

                case 1:
                    if (clear_num == 0)
                        RI_gem.texture = texture_gem2_0;
                    else if (clear_num == 1)
                        RI_gem.texture = texture_gem2_1;
                    else
                        RI_gem.texture = texture_gem2_2;
                    break;

                default:
                    if (clear_num == 0)
                        RI_gem.texture = texture_gem3_0;
                    else if (clear_num == 1)
                        RI_gem.texture = texture_gem3_1;
                    else
                        RI_gem.texture = texture_gem3_2;
                    break;
            }

            //맵 활성화
            btn_1.image.sprite = mp;            //stage1 활성화
            if (clear_num >= 1)
            {
                btn_2.image.sprite = mp;        //stage2 활성화

                if (clear_num >= 2)
                    btn_3.image.sprite = mp;    //stage3 활성화
            }

        }

        T_Status.text = str_origin;
    }

    public void Go_game(int num)
    {
        //순서를 지키지 않았을 때 안내
        if (num > clear_num)
        {
            SoundManager.Instance.Play_effect(2);
            SC.Prevent(clear_num + 1);
            return;
        }


        int scene_index = 2;                    //게임 인덱스는 2부터 시작
        scene_index += num;                     //stage 설정: 0~2
        scene_index += 3 * SC.GetPlanetNum();   //행성 설정 : 토양 : 0, 수질 : 1, 대기 : 2
        
        SoundManager.Instance.Play_effect(0);
        SC.Load_Scene(scene_index);             //Scene 이동

    }

}
