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

    void Awake()
    {
        current = SceneManager.GetActiveScene().name;
    }

    //씬 이동에 사용
    void Go(string scene) {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    //씬 이름 앞의 숫자를 통해 구분
    public void Go(int num) {
        string scene;
        switch (num) {
            case 1: // 01_Main
                scene = "01_Main";
                //메인용 배경음악으로 초기화
                if (GetActiveScene_num() > 10)
                    SoundManager.Instance.BGM_reset();
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
                Change_Scene(5);
                return;
            case 6: // 06_Do_item
                Change_Scene(6);
                return;
            case 7: // 07_Do_diary
                Change_Scene(7);
                return;

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

                    //아직 준비되지 않은 게임씬일 경우
                    if (!Application.CanStreamedLevelBeLoaded(scene)) {
                        string message = "'stage" + (num - 10).ToString() + "'는 아직 준비중입니다. (" + scene + ")";
                        GameObject GO = GameObject.Find("Default");
                        if (GO)
                            GO.GetComponent<ToastManager>().ShowToast(message);
                        return;
                    }

                    //게임용 BGM설정
                    SoundManager.Instance.BGM_chage(true);
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
    public void Add_Scene(int num) {
        //기본 Scene은 일시 정지
        Time.timeScale = 0;

        string scene = "";
        switch (num) {
            case 5:
                scene = "05_Do_planet";
                break;
            case 8:
                scene = "08_Setting";
                break;
            case 9:
                scene = "09_Dialog";
                break;
            case 10:
                scene = "10_Clear";
                break;
        }
        if (scene.Length > 0)
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    //나가기(뒤로가기) 버튼에 적용
    public void Destroy_Scene() {
        //기본 Scene 다시 작동
        Time.timeScale = 1;
        for (int i = 1; i < SceneManager.sceneCount; ++i)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
        }
    }

    public void Change_Scene(int num) {
        string scene;
        switch (num)
        {
            case 5:
                scene = "05_Do_planet";
                break;
            case 6:
                scene = "06_Do_item";
                break;
            case 7:
                scene = "07_Do_diary";
                break;
            case 10:
                scene = "10_Clear";
                break;
            default:
                scene = "";
                break;
        }
        if (scene.Length > 0) {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            for (int i = 1; i < SceneManager.sceneCount - 1; ++i)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
            print("change " + scene);
        }
    }

    public void BackMap()
    {
        StopAllCoroutines();
        int map = PlayerPrefs.GetInt("TheMapIs", 1) + 1;
        Go(map);
    }

    public string GetActiveScene_name() {
        return current;
    }

    public int GetActiveScene_num() {
        string current_num = current.Substring(0, 2); 
        return int.Parse(current_num);
    }


}
