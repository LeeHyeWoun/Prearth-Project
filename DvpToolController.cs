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
    public InputField input1;

    private int count=0;
    private const string tmp_Stage = "tmp_Stage";

    private void Start()
    {
        input1.text = PlayerPrefs.GetInt(tmp_Stage, 0).ToString();
    }

    //투명 버튼 3번 연속 터치시 tool 열기
    public void OpenTool() {
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
    public void Apply()
    {
        PlayerPrefs.SetInt(tmp_Stage, int.Parse(input1.text));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
