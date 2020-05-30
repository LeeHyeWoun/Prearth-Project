using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdviceController : MonoBehaviour
{
    //할당받을 객체
    public GameObject order_I, order_T;

    //변수
    Dictionary<string, string> orderList = new Dictionary<string, string>();
    int sceneNum;
    IEnumerator routine;
    Image oderI;
    Text oderT;

    //상수
    readonly WaitForSeconds wait1 = new WaitForSeconds(0.3f);
    readonly WaitForSeconds wait2 = new WaitForSeconds(3f);


    void Start()
    {
        sceneNum = SceneController.Instance.GetActiveScene_num();
        oderI = order_I.GetComponent<Image>();
        oderT = order_T.GetComponent<Text>();

        //현재 씬에 따라 다른 start 대사 가져오기
        switch (sceneNum)
        {
            case 2: //02_Soil_1
                orderList.Add("2_start", "꼬륵이의 집에서 문제의 단서를 찾아보세요.");
                orderList.Add("2_play1", "PET 병을 올바르게 분리수거하세요.");
                orderList.Add("2_play2_1", "비닐과 플라스틱을 분리해주세요.");
                orderList.Add("2_play2_2", null);
                orderList.Add("2_play2_3", "내용물과 비닐을 분리해주세요.");
                orderList.Add("2_clear", null);
                break;
            case 3: //02_Soil_2
                orderList.Add("3_start", "공원에서 꼬륵이의 이웃주민을 찾아 문제를 해결하세요.");
                orderList.Add("3_play1_1", "3개의 컵을 저울을 이용하여 환경 부담이 적은 순으로 나열하세요.");
                orderList.Add("3_play1_2", null);
                orderList.Add("3_play2", null);
                orderList.Add("3_play3_1", "돗자리에 엎질러진 물을 닦기위해 휴지나무에서 얻은 휴지를 전달해주세요.");
                orderList.Add("3_play3_2", "더 찾아주세요.");
                orderList.Add("3_play3_3", null);
                orderList.Add("3_clear", null);
                break;
            case 5: //05_Water_1
                orderList.Add("5_start", "뿌직이의 집에서 문제의 단서를 찾아보세요.");
                orderList.Add("5_play1", "폐기름을 올바르게 분리수거해주세요.");
                orderList.Add("5_play2_1", "남은 약품들을 올바르게 분리수거해주세요.");
                orderList.Add("5_play2_2", "컨택트 렌즈를 올바르게 분리수거해주세요.");
                orderList.Add("5_play2_3", null);
                orderList.Add("5_clear", null);
                break;
            case 6: //06_Water_2
                orderList.Add("6_start", "해변에서 도움이 필요한 곳을 찾아 문제를 해결해주세요.");
                orderList.Add("6_play1", "거북이를 그물에서 분리해주세요.");
                orderList.Add("6_play1_1", null);
                orderList.Add("6_play2", "뿌직족이 먹은 물고기를 조사하여 문제점을 찾아주세요");
                orderList.Add("6_play2_1", "도구를 이용해 물고기를 조사해주세요.");
                orderList.Add("6_play2_2", null);
                orderList.Add("6_play3", null);
                orderList.Add("6_clear", null);
                break;
            case 8: //08_Air_1
                orderList.Add("8_start", "콜록이의 집에서 문제의 단서를 찾아보세요.");
                orderList.Add("8_play1", "사용하지 않는 가전제품의 코드를 뽑아주세요.");
                orderList.Add("8_play2_1", "겨울철 적정온도를 맞춰주세요.");
                orderList.Add("8_play2_2", "에어캡을 이용해 집 온도를 높여주세요.");
                orderList.Add("8_play2_3", null);
                orderList.Add("8_clear", null);
                break;
            case 9: //09_Air_2
                orderList.Add("9_start", "도시에서 도움이 필요한 곳을 찾아 문제를 해결하세요.");
                orderList.Add("9_play1_1", "도시에 있는 5등급 차량을 단속하세요.");
                orderList.Add("9_play1_2", null);
                orderList.Add("9_play2", null);
                orderList.Add("9_play3", "배출가스 저감장치를 설치하여 연기를 없애주세요.");
                orderList.Add("9_clear", null);
                break;
            default:
                print("orderList 미설정");
                break;
        }

        Dialog_and_Advice("start");
    }

    public void Advice(string order)
    {
        if(routine!=null)
            StopCoroutine(routine);

        //조언 띄우기
        routine = Routine_Advice(order);
        StartCoroutine(routine);
    }

    public void Dialog_and_Advice(string file)
    {
        file = sceneNum + "_" + file;
        //Dialog
        StartCoroutine(Routine_Dialog(file));
    }

    IEnumerator Routine_Dialog(string file) {
        PlayerPrefs.SetString("DIALOG", file);
        while (routine != null) {
            yield return wait1;
        }
        SceneController.Instance.Load_Scene(14);

        //Advice
        if (orderList[file] != null)
            Advice(orderList[file]);
    }

    IEnumerator Routine_Advice(string txt)
    {
        yield return wait1;

        //초기화
        Color fade_RI = oderI.color;
        Color fade_T = oderT.color;

        //투명화
        float fade_A = 0f;
        fade_RI.a = fade_A;
        fade_T.a = fade_A;
        oderI.color = fade_RI;
        oderT.color = fade_T;
        
        //내용 설정
        oderT.text = txt;

        //글자 수에 따라 이미지 가로 사이즈 변경
        float width = 512 + ((txt.Length > 3) ? (txt.Length - 3) * 35 : 0);
        order_I.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(width, 256);

        //열기
        order_I.SetActive(true);
        while (fade_RI.a < 1)
        {
            fade_A += 0.075f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;
            oderI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }

        //대기
        yield return wait2;

        //닫기
        while (fade_RI.a > 0)
        {
            fade_A *= 0.94f;
            if (fade_A < 0.2)
                fade_A -= 0.05f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;

            oderI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }
        order_I.SetActive(false);
        routine = null;
    }

}
