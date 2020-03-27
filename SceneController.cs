using UnityEngine.SceneManagement;
using UnityEngine;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  씬의 이동을 담당하는 스크립트
 *  
 *  Applied Location :
 *  -> 모든 Scene의 GameDirector(빈 오브젝트)
 */

public class SceneController : MonoBehaviour {

    //상수
    const string MAP_NUMBER = "TheMapIs";
    const string PRE_SCENE = "PreScene";

    //변수
    string current;

    void Start()
    {
        current = SceneManager.GetActiveScene().name;
    }

    //씬 이동에 사용
    void Go(string scene) {
        print("****************************** [" + current + "] --> [" + scene + "] ******************************");
        SceneManager.LoadScene(scene);
    }

    //씬 이름 앞의 숫자를 통해 구분
    public void Go(int num) {
        string scene;
        switch (num) {
            case 1: // 01_Main
                scene = "01_Main";
                break;
            case 2: // 02_Map.............. soil 세팅
                scene = "02_Map";
                PlayerPrefs.SetInt(MAP_NUMBER, 1);
                break;
            case 3: // 02_Map.............. water 세팅
                scene = "02_Map";
                PlayerPrefs.SetInt(MAP_NUMBER, 2);
                break;
            case 4: // 02_Map.............. air 세팅 
                scene = "02_Map";
                PlayerPrefs.SetInt(MAP_NUMBER, 3);
                break;
            case 5: // 05_Do_planet
                scene = "05_Do_planet";
                break;
            case 6: // 06_Do_item
                scene = "06_Do_item";
                break;
            case 7: // 07_Do_diary
                scene = "07_Do_diary";
                break;
            default:
                // 게임 씬
                /*
                 * 02_Map의 버튼 'mapPoint1'에 num = 11
                 * 02_Map의 버튼 'mapPoint2'에 num = 12
                 * 02_Map의 버튼 'mapPoint3'에 num = 13
                 * 를 적용하고 TheMapIs 값에 따라 결과를 달리한다.
                 */
                if (num > 10 && num < 14)
                {
                    int mapNum = PlayerPrefs.GetInt(MAP_NUMBER, 1);
                    scene = (num + 3 * (mapNum - 1)).ToString();
                    if (mapNum == 1)
                        scene += "_Soil_";
                    else if (mapNum == 2)
                        scene += "_Water_";
                    else
                        scene += "_Air_";
                    scene += (num - 10).ToString();

                    if (!Application.CanStreamedLevelBeLoaded(scene)) {
                        string message = "'stage" + (num - 10).ToString() + "'는 아직 준비중입니다. (" + scene + ")";
                        GameObject.Find("SoundManager").GetComponent<ToastManager>().ShowToast(message);
                        return;
                    }
                }
                // 존재하지 않는 씬 에러처리
                else {
                    scene = "01_Main";
                    print("해당 씬<" + scene + ">은 존재하지 않습니다.");
                    return;
                }
                break;
        }
        Go(scene);
    }

    //조사일지 버튼에 적용
    public void Do_in() {
        /*
         * 조사일지에서 돌아올 때를 위해 
         * 현재의 씬 이름을 저장하고 '05_Do_planet'으로 이동
         */
        PlayerPrefs.SetString(PRE_SCENE, current);
        Go(5);
    }

    //나가기(뒤로가기) 버튼에 적용
    public void Do_out() {
        /*
         * Do씬에 들어오기 전의 씬으로 돌아가기 위해
         * 저장해둔 씬 이름을 불러와 그 이름의 씬으로 이동
         */
        string scene = PlayerPrefs.GetString(PRE_SCENE, "");
        if (scene.Length > 0)
            Go(scene);
        else
            Go(1);
    }

    public void BackMap() {
        int map = PlayerPrefs.GetInt("TheMapIs", 1) + 1;
        Go(map);
    }
}
