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
    int stage_num;
    string str_origin = "";

    //미리 캐싱
    readonly WaitForSeconds ShortTerm = new WaitForSeconds(1f);

    //커스텀 클래스의 인스턴스
    SceneController scenenCtrl;
    SoundController SoundCtrl;


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
                str_origin = "페페행성";
                RI_map.texture = texture_map_soil;
                mp = texture_mapPoint_soil;
                break;

            case 2:
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
        if ((mapNum == 1 && stage_num >= 3) ||
            (mapNum == 2 && stage_num >= 6) ||
            (mapNum == 3 && stage_num >= 9))
        {
            //상태창 내용 설정
            str_origin += "의 연료를 모두 모았습니다.";

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
            str_origin += " " + (3 - stage_num).ToString() + "조각 남았습니다.";

            //보석 설정
            switch (mapNum) {
                case 1:
                    if (stage_num == 0)
                        RI_gem.texture = texture_gem1_0;
                    else if (stage_num == 1)
                        RI_gem.texture = texture_gem1_1;
                    else
                        RI_gem.texture = texture_gem1_2;
                    break;

                case 2:
                    if (stage_num == 0)
                        RI_gem.texture = texture_gem2_0;
                    else if (stage_num == 1)
                        RI_gem.texture = texture_gem2_1;
                    else
                        RI_gem.texture = texture_gem2_2;
                    break;

                default:
                    if (stage_num == 0)
                        RI_gem.texture = texture_gem3_0;
                    else if (stage_num == 1)
                        RI_gem.texture = texture_gem3_1;
                    else
                        RI_gem.texture = texture_gem3_2;
                    break;
            }

            //맵 활성화
            btn_1.image.sprite = mp;            //stage1 활성화
            if (stage_num >= 1)
            {
                btn_2.image.sprite = mp;        //stage2 활성화

                if (stage_num >= 2)
                    btn_3.image.sprite = mp;    //stage3 활성화
            }

        }

        T_Status.text = str_origin;

        scenenCtrl = GetComponent<SceneController>();
        SoundCtrl = GetComponent<SoundController>();
    }

    public void Go_game(int num)
    {
        switch (num)
        {
            case 1:
                SoundCtrl.Play_effect(0);
                scenenCtrl.Go(11);
                break;

            case 2:
                if (stage_num >= 1)
                {
                    SoundCtrl.Play_effect(0);
                    scenenCtrl.Go(12);
                }
                else {
                    Advice("'Stage1'부터 입장해주세요!");
                    SoundCtrl.Play_effect(2);
                }
                break;

            case 3:
                if (stage_num >= 2)
                {
                    SoundCtrl.Play_effect(0);
                    scenenCtrl.Go(13);
                }
                else
                {
                    if (stage_num < 1)
                        Advice("'Stage1'부터 입장해주세요!");
                    else
                        Advice("'Stage2'부터 입장해주세요!");

                    SoundCtrl.Play_effect(2);
                }
                break;
        }
    }

    void Advice(string advice) {
        StopAllCoroutines();    //버튼을 연속으로 누를때 충돌을 피하기 위해
        StartCoroutine(routine_advice(advice));
    }

    IEnumerator routine_advice(string advice) {
        T_Status.text = "<color=#C1C1C1>" + advice + "</color>";
        yield return ShortTerm;
        T_Status.text = str_origin;
    }


}
