using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Date     : 2020.02.11
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  개발자 도구를 다루는 스크립트
 *  -> 숨겨놓은 툴을 꺼내 클리어 상황을 조절할 수 있음
 *  
 *  Applied Location :
 *  -> GameDirector(빈 오브젝트)
 */

public class DvpToolController : MonoBehaviour {

    public GameObject tool;
    public Text txt_clear;

    private int count=0;
    private const string tmp_Clear = "tmp_Clear";

    //커스텀 클래스 인스턴스
    SoundManager SM;


    private void Start()
    {
        SM = SoundManager.Instance;
    }


    //투명 버튼 3번 연속 터치시 tool 열기
    public void OpenTool() {
        SM.Play_effect(0);
        txt_clear.text = "Clear : " + PlayerPrefs.GetInt(tmp_Clear, 0).ToString();

        if (count > 2)
        {
            tool.SetActive(true);
            CancelInvoke("CountReset");
        }
        else
        {
            count++;
            if (!IsInvoking("CountReset"))
                Invoke("CountReset", 3.0f);
        }
    }

    //ClickCount 초기화
    void CountReset() { count = 0; }

    //적용하기
    public void Apply(int num)
    {
        SM.Play_effect(0);
        PlayerPrefs.SetInt(tmp_Clear, num);
        if (num > -1) {
            PlayerPrefs.SetString(
                num == 3 ? "tmp_date_soil" : num == 6 ? "tmp_date_water" : "tmp_date_air",
                DateTime.Now.ToString("yyyy-MM-dd"));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else {
            SceneController.Instance.Load_Scene(0);
        }

    }
}
