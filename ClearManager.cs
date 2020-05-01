using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearManager : MonoBehaviour {

    //UI
    public Image clearbase, I_gem_fill;
    public RawImage RI_gem_base;
    public Text advice;

    //asset
    public Sprite
        base2, base3,
        sprite_gem_fill2, sprite_gem_fill3;
    public Texture
        texture_gem_base_2, texture_gem_base_3;                             //보석 베이스 이미지

    //변수
    int game_num;


    void Start () {
        game_num = SceneController.Instance.GetActiveScene_num() - 1;

        //이미지 세팅
        SettingScene(game_num);

        //클리어 값 설정
        PlayerPrefs.SetInt("tmp_Clear", game_num);
        if (game_num % 3 == 0)
        {
            PlayerPrefs.SetString(
                game_num == 3 ? "tmp_date_soil" : game_num == 6 ? "tmp_date_water" : "tmp_date_air", 
                DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }


    void SettingScene(int sceneNum) {
        //배경 설정
        if((sceneNum - 1) / 3 == 1)
            clearbase.sprite = base2;   //14~16
        else 
        if ((sceneNum - 1) / 3 == 2)
            clearbase.sprite = base3;   //17~19

        //보석 이미지 설정
        if (sceneNum > 7)       //air
        {
            RI_gem_base.texture = texture_gem_base_3;
            I_gem_fill.sprite = sprite_gem_fill3;
        }
        else if (sceneNum > 4)  //water
        {
            RI_gem_base.texture = texture_gem_base_2;
            I_gem_fill.sprite = sprite_gem_fill2;
        }

        //보석 양 설정
        if (sceneNum % 3 == 1)
            I_gem_fill.fillAmount = 0.33f;
        else if(sceneNum % 3 == 0)
            I_gem_fill.fillAmount = 0.66f;

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

        //02_Map으로 돌아가기
        SceneController.Instance.Load_Scene(1);

    }
}
