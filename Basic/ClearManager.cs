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

        Camera.main.GetComponent<SuperBlur.SuperBlur>().enabled = true;

        game_num = SceneController.Instance.GetActiveScene_num() - 1;

        //이미지 세팅
        SettingScene(game_num);

        //클리어 값 설정
        PlayerPrefs.SetInt("tmp_Clear", game_num);
    }


    void SettingScene(int game_num) {

        //행성별 이미지 설정
        if (game_num > 6)       //air
        {
            //배경 설정
            clearbase.sprite = base3;
            //보석 이미지 설정
            RI_gem_base.texture = texture_gem_base_3;
            I_gem_fill.sprite = sprite_gem_fill3;
        }
        else if (game_num > 3)  //water
        {
            //배경 설정
            clearbase.sprite = base2;
            //보석 이미지 설정
            RI_gem_base.texture = texture_gem_base_2;
            I_gem_fill.sprite = sprite_gem_fill2;
        }

        //행성 진행 정도 설정
        int q = game_num % 3;
        if (q.Equals(0))
        {
            advice.text = "짝짝짝! 우주선을 출발할 수 있어요!";

            //클리어 날짜 저장
            PlayerPrefs.SetString(
                game_num.Equals(3) ? "tmp_date_soil" : game_num.Equals(6) ? "tmp_date_water" : "tmp_date_air",
                DateTime.Now.ToString("yyyy-MM-dd"));
        }
        else
        {
            advice.text = "우주선 출발까지 " + (3 - q) + "조각 남았습니다.";
            
            //보석 양 설정
            if (q == 1)
                I_gem_fill.fillAmount = 0.33f;
            else
                I_gem_fill.fillAmount = 0.66f;

        }
    }

    public void Close() {
        //메인용 배경음악으로 초기화
        SoundManager.Instance.BGM_reset();

        //02_Map으로 돌아가기
        SceneController.Instance.Load_Scene(1);

    }
}
