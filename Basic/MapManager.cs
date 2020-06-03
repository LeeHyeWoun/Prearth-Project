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
    //UI
    public Text T_Status;                                                   //상태 알림
    public Image I_gem_fill;                                                //보석
    public RawImage RI_gem_base, RI_map;                                    //보석 베이스와 지도
    public Button btn_1, btn_2, btn_3;                                      //stage 포인트

    //Resource
    public Texture
        texture_gem_base_2, texture_gem_base_3,                             //보석 베이스 이미지
        texture_map_water, texture_map_air;                                 //지도 이미지
    public Sprite
        sprite_gem_fill2, sprite_gem_fill3;                                 //보석 이미지
    public Sprite[] Sprites_MapPoin;                                        //지도 스테이지 이미지...size = 3

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
        clear_num = PlayerPrefs.GetInt("tmp_Clear");

        if (clear_num == 9)
            SceneController.Instance.Load_Scene(16);

        int planet_num = SC.Planet_num;

        //행성 디자인 설정
        Sprite mp = Sprites_MapPoin[planet_num];
        switch (planet_num)
        {
            case 0:
                str_origin = "페페행성";
                break;

            case 1:
                str_origin = "도도행성";
                RI_map.texture = texture_map_water;
                break;

            default:
                str_origin = "라라행성";
                RI_map.texture = texture_map_air;
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

            //보석 이미지 설정
            if (planet_num.Equals(1))
            {
                RI_gem_base.texture = texture_gem_base_2;
                I_gem_fill.sprite = sprite_gem_fill2;
            }
            else if (planet_num.Equals(2))
            {
                RI_gem_base.texture = texture_gem_base_3;
                I_gem_fill.sprite = sprite_gem_fill3;
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
            if (clear_num > 0)
                str_origin += " " + (3 - clear_num).ToString() + "조각 남았습니다.";
            else
                str_origin += " 3조각 남았습니다.";

            //보석 이미지 설정
            if (planet_num.Equals(1))
            {
                RI_gem_base.texture = texture_gem_base_2;
                I_gem_fill.sprite = sprite_gem_fill2;
            }
            else if (planet_num.Equals(2))
            {
                RI_gem_base.texture = texture_gem_base_3;
                I_gem_fill.sprite = sprite_gem_fill3;
            }

            //보석 양 설정
            switch (clear_num) {
                case 1:
                    I_gem_fill.fillAmount = 0.33f;
                    break;
                case 2:
                    I_gem_fill.fillAmount = 0.66f;
                    break;
                default:
                    I_gem_fill.fillAmount = 0f;
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
        //게임 튜토리얼
        if (clear_num.Equals(-1))
        {
            if (num == 0)
            {
                SoundManager.Instance.Play_effect(0);
                SC.Load_Scene(12);
                clear_num = 0;
            }
            else {
                SoundManager.Instance.Play_effect(2);
                SC.Prevent(clear_num + 2);
            }
        }
        //순서를 지키지 않았을 때 안내
        else if (num > clear_num)
        {
            SoundManager.Instance.Play_effect(2);
            SC.Prevent(clear_num + 1);
        }
        else
        {
            int scene_index = 2;                    //게임 인덱스는 2부터 시작
            scene_index += num;                     //stage 설정: 0~2
            scene_index += 3 * SC.Planet_num;   //행성 설정 : 토양 : 0, 수질 : 1, 대기 : 2

            SoundManager.Instance.Play_effect(0);
            SC.Load_Scene(scene_index);             //Scene 이동
        }
    }

}
