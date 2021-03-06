﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DoHistoryManager : MonoBehaviour {

    //UI
    public GameObject Go_Back, Go_Next;
    public Image I_main;
    public Text T_info_name, T_info_summary;

    public Button[] Btns_main;  //size = 3
    public Image[] 
        imgs_clue_base,         //size = 3
        Imgs_clue;              //size = 3
    public Text[] Txts_clue;    //size = 3

    //Resource... 스테이지 / 단서 이미지들
    public SpriteAtlas Atlas;

    //변수
    SoundManager SM = SoundManager.Instance;
    List<string> List_clue_txt = new List<string>();    //0~2 : 토양 stage1 단서, 3~5 : 토양  stage2 단서 ....
    StringReader stringReader;
    int clear, stage; //stage >> 1 : 토양 stage1 , 2 : 토양 stage2 , 3 : 수질 stage1 , 4 : 수질 stage2 , 5 : 대기 stage1 , 6 : 대기 stage2

    //상수
    const string LOCATION = "history/";
    readonly string[] PLANET_NAME = {"페페","라라","도도" };
    readonly string[] INFO_SUMMARY = {
        //토양1
        "꼬륵이 집에서 토양오염 원인 찾기\n" +
            "올바른 분리수거를 배워보자",

        //토양2
        "공원에서 토양오염 원인 찾기\n" +
            "일회용품의 대체제를 배워보자",

        //수질1
        "뿌직이 집에서 수질오염 원인 찾기\n" +
            "올바른 생활폐수 처리방법을 배워보자",

        //수질2
        "바다에서 수질오염 원인 찾기\n" +
            "바다를 지키기 위한 방법을 배워보자",

        //대기1
        "콜록이 집에서 대기오염 원인 찾기\n" +
            "전기/난방에너지 절약 방법을 배워보자",

        //대기2
        "도시에서 대기오염 원인 찾기\n" +
            "미세먼지를 줄일 방법에 대해 배워보자"
    };
    readonly Color COLOR_PRIVATE = new Color(56 / 255f, 56 / 255f, 56 / 255f);
    readonly Vector3[] CLUE_POS = { new Vector3(390, 315, 0), new Vector3(390, 0, 0), new Vector3(390, -315, 0) };


    //초기화
    void Awake()
    {
        clear = PlayerPrefs.GetInt("tmp_Clear", 0);
        if (clear > 0)
            Road_file("02_clues");
        if (clear > 1)
            Road_file("03_clues");
        if (clear > 3)
            Road_file("05_clues");
        if (clear > 4)
            Road_file("06_clues");
        if (clear > 6)
            Road_file("08_clues");
        if (clear > 7)
            Road_file("09_clues");

        Go_Back.GetComponent<Image>().sprite = Atlas.GetSprite("icon_arrow_left");
        Go_Next.GetComponent<Image>().sprite = Atlas.GetSprite("icon_arrow_right");
    }

    void OnEnable()
    {
        stage = 1;
        Go_Back.SetActive(false);
        Go_Next.SetActive(true);
        Btns_main[0].interactable = false;
        Btns_main[1].interactable = true;
        Btns_main[2].interactable = true;
        SetStage();
    }

    //privaste 함수들---------------------------------------------------------------------------------------------------------------
    void Road_file(string filename)
    {
        if (!IsClearStage())
            return;

        TextAsset file = Resources.Load(LOCATION + filename) as TextAsset;

#if DEV_TEST
        // 파일 존재 여부 체크
        if (file == null)
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/history/' 경로에서 파일<" + filename + ".txt>를 찾을 수 없습니다.");
            return;
        }
#endif
        //리스트에 넣기
        stringReader = new StringReader(file.text);
        for (int i = 0; i < 3; i++)
            List_clue_txt.Add(stringReader.ReadLine());
    }

    bool IsClearStage()
    {
        return (stage - 1) / 2 * 3 + (stage - 1) % 2 + 1 <= clear ? true : false;
    }

    void SetInfo(int stage)
    {
        //스테이지 이름 설정
        T_info_name.text = PLANET_NAME[(stage - 1) / 2] + "행성 stage" + (stage % 2 == 1 ? 1 : 2);

        //스테이지 줄거리 설정
        T_info_summary.text = IsClearStage() ? INFO_SUMMARY[stage - 1] : "이곳은 어떤 행성일까?";
    }

    void SetTxtClue(int stage)
    {
        //텍스트 비활성화
        for (int i = 0; i < 3; i++)
            Txts_clue[i].gameObject.SetActive(false);

        if (!IsClearStage())
            return;

        for (int i = 0; i < 3; i++)
            Txts_clue[i].text = List_clue_txt[(stage-1)*3 + i];
    }

    void SetRIClue(int stage)
    {
        //이미지 활성화
        for (int i = 0; i < 3; i++)
            Imgs_clue[i].gameObject.SetActive(true);

        //스테이지 이미지 설정
        I_main.sprite = Atlas.GetSprite("stage_" + stage);
        I_main.color = IsClearStage() ? Color.white : COLOR_PRIVATE;

        //단서 이미지 설정
        for (int i = 0; i < 3; i++)
            Imgs_clue[i].sprite = IsClearStage() ? 
                Atlas.GetSprite("clue"+ stage +"_" + i) : Atlas.GetSprite("clue_private");

    }

    void SetStage()
    {
        SetTxtClue(stage);
        SetRIClue(stage);
        SetInfo(stage);
        StopCoroutine("Appear_btn_clue");
        StartCoroutine("Appear_btn_clue");
    }


    //public 함수들-----------------------------------------------------------------------------------------------------------------

    // planet >> 1 : 토양 ,   2 : 수질,   3 : 대기 
    public void BE_Planet(int planet)
    {
        SM.Play_effect(0);

        //활성화 설정
        for(int i = 0; i<3; i++)
            Btns_main[i].interactable = planet.Equals(i+1) ? false : true;

        stage = planet * 2 - 1;

        SetStage();
        Go_Back.SetActive(false);
        Go_Next.SetActive(true);
    }

    public void BE_Back()
    {
        SM.Play_effect(0);

        stage--;

        SetStage();
        Go_Back.SetActive(false);
        Go_Next.SetActive(true);
    }
    public void BE_Next()
    {
        SM.Play_effect(0);

        stage++;

        SetStage();
        Go_Back.SetActive(true);
        Go_Next.SetActive(false);
    }

    public void BE_clue(int num)
    {

        SM.Play_effect(0);

        if (!IsClearStage())
            return;

        GameObject ri, txt;
        txt = Txts_clue[num-1].gameObject;
        ri = Imgs_clue[num - 1].gameObject;

        if (ri.activeSelf)
        {
            ri.SetActive(false);
            txt.SetActive(true);
        }
        else
        {
            ri.SetActive(true);
            txt.SetActive(false);
        }
    }

    //코루틴 -----------------------------------------------------------------------------------------------------------------
    System.Collections.IEnumerator Appear_btn_clue() {
        //초기화
        Color color = Color.white;
        color.a = 0.2f;
        for (int i = 0; i < 3; i++)
            imgs_clue_base[i].transform.localPosition = CLUE_POS[i];

        //시작 위치로 이동
        for (int i=0; i<3; i++)
            imgs_clue_base[i].transform.Translate(Vector3.down * 20f);

        //움직임
        for (int i = 0; i < 40; i++)
        {
            color.a += 0.02f;
            for (int j = 0; j < 3; j++)
            {
                Imgs_clue[j].color = color;
                imgs_clue_base[j].transform.Translate(Vector3.up * 0.5f);
            }
            yield return null;
        }
    }
}
