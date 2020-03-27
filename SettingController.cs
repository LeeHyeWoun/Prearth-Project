using UnityEngine;

/**
 * Date     : 2020.02.11
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  세팅창을 조절하는 스크립트
 *  
 *  Applied Location :
 *  -> Scene# 05 ~ 07을 제외한 GameDirector(빈 오브젝트)
 */
public class SettingController : MonoBehaviour {

    public GameObject blur, setting;

    //세팅창 띄우기
    //Apply to 'btn_setting'
    public void openSetting()
    {
        blur.SetActive(true);
        setting.SetActive(true);
    }

    //세팅창 닫기
    //Apply to 'B_KeepPlay'
    public void closeSetting()
    {
        blur.SetActive(false);
        setting.SetActive(false);
    }
}
