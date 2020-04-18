using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour {

    public Image clearbase;
    public RawImage Gem;
    public Text advice;
    public Sprite
        base1, base2, base3;
    public Texture
        gem1_1, gem1_2, gem1_3,
        gem2_1, gem2_2, gem2_3,
        gem3_1, gem3_2, gem3_3;

    int game_num;

    void Start () {
        game_num = GetComponent<SceneController>().GetActiveScene_num() - 10;

        //이미지 세팅
        SettingScene(game_num);
    }

    void SettingScene(int sceneNum) {
        //씬 넘버에 따라 배경 세팅
        switch ((sceneNum - 1) / 3)
        {
            case 1:
                clearbase.sprite = base2;   //14~16
                break;
            case 2:
                clearbase.sprite = base3;   //17~19
                break;
            default:
                clearbase.sprite = base1;   //11~13
                break;
        }

        //씬 넘버에 따라 연료이미지 세팅
        Texture texture_gem;
        switch (sceneNum)
        {
            case 2:
                texture_gem = gem1_2;       //12
                break;
            case 3:
                texture_gem = gem1_3;       //13
                break;
            case 4:
                texture_gem = gem2_1;       //14
                break;
            case 5:
                texture_gem = gem2_2;       //15
                break;
            case 6:
                texture_gem = gem2_3;       //16
                break;
            case 7:
                texture_gem = gem3_1;       //17
                break;
            case 8:
                texture_gem = gem3_2;       //18
                break;
            case 9:
                texture_gem = gem3_3;       //19
                break;
            default:
                texture_gem = gem1_1;       //11
                break;
        }
        Gem.texture = texture_gem;

        //씬 넘버에 안내문 내용 세팅
        int quotient = (sceneNum) % 3;
        switch (quotient)
        {
            case 0:
                advice.text = "짝짝짝! 우주선을 출발할 수 있어요!";
                break;
            default:
                advice.text = "우주선 출발까지 " + (3 - quotient) + "조각 남았습니다.";
                break;
        }
    }

    public void Close() {
        //메인용 배경음악으로 초기화
        SoundManager.Instance.BGM_reset();
        
        //클리어 값 설정
        if (PlayerPrefs.GetInt("tmp_stage") < (game_num))
            PlayerPrefs.SetInt("tmp_stage", game_num);

        //02_Map으로 돌아가기
        GetComponent<SceneController>().BackMap();

    }
}
