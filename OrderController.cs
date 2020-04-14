using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderController : MonoBehaviour {
    //할당받을 객체
    public ParticleSystem order_effect;
    public GameObject order_RI, order_T;

    //변수
    int sceneNum;
    Dictionary<string, string> orderList = new Dictionary<string, string>();

    //커스텀 클래스 인스턴스
    SoundController SoundC;
    SceneController SceneC;
    DialogController DC;

    //미리 캐싱
    readonly WaitForSeconds orderTerm = new WaitForSeconds(3f);


    void Start()
    {
        SoundC = GetComponent<SoundController>();
        SceneC = GetComponent<SceneController>();
        DC = GetComponent<DialogController>();

        //현재 씬에 따라 다른 start 대사 가져오기
        sceneNum = SceneC.GetActiveScene_num();

        switch (SceneC.GetActiveScene_num())
        {
            case 11:
                orderList.Add("11_start",   "꼬륵이의 집에서 문제의 단서를 찾아보세요.");
                orderList.Add("11_play1",   "PET 병을 올바르게 분리수거하세요.");
                orderList.Add("11_play2_1", "비닐과 플라스틱을 분리해주세요.");
                orderList.Add("11_play2_2", null);
                orderList.Add("11_play2_3", "내용물과 비닐을 분리해주세요.");
                orderList.Add("11_clear",   null);
                break;
            default:
                print("orderList 미설정");
                break;
        }
    }


    public void Dialog_Start(string file)
    {
        DC.Dialog(file);

        if (orderList.ContainsValue(file))
            Order(orderList[file]);
    }

    public void Order(string order) {
        StartCoroutine(Routine_Order(order));
    }

    IEnumerator Routine_Order(string txt)
    {
        //초기화
        RawImage oderRI = order_RI.GetComponent<RawImage>();
        Text oderT = order_T.GetComponent<Text>();
        Color fade_RI = oderRI.color;
        Color fade_T = oderT.color;
        float fade_A = 0f;

        fade_RI.a = fade_A;
        fade_T.a = fade_A;
        oderRI.color = fade_RI;
        oderT.color = fade_T;
        oderT.text = txt;

        //열기
        order_RI.SetActive(true);
        while (fade_RI.a < 1)
        {
            fade_A += 0.075f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;
            oderRI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }

        //대기
        yield return orderTerm;

        //닫기
        while (fade_RI.a > 0)
        {
            fade_A *= 0.94f;
            if (fade_A < 0.2)
                fade_A -= 0.01f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;

            oderRI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }
        order_RI.SetActive(false);

    }

    IEnumerator Routine_ending()
    {
        //소리 조절
        if (SoundC.IsSoundManager())
        {
            float bgm_value = PlayerPrefs.GetFloat("bgm", 1f);
            while (bgm_value > 0)
            {
                SoundC.SetBGM_Volum(bgm_value);
                bgm_value -= 0.01f;
                yield return null;
            }
        }

        //클리어하지 않은 스테이지일 경우 
        int tmp = PlayerPrefs.GetInt("tmp_Stage", 0);
        if (tmp <= (sceneNum - 10))
        {
            tmp = sceneNum - 10;
            PlayerPrefs.SetInt("tmp_Stage", tmp);

            //클리어 창 팝업 + fade in effedt
            GetComponent<SceneController>().Change_Scene(10);
        }
        //이미 클리어한 스테이지일 경우 그냥 맵으로 돌아가기
        else
        {
            GetComponent<SceneController>().BackMap();
        }
    }

}
