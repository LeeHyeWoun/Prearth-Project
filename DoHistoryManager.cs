﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClueArray {
    public Texture[] clues;
}

public class DoHistoryManager : MonoBehaviour {

    //UI
    public Button Btn_Back, Btn_Next;
    public RawImage RI_main;
    public Text T_info_name, T_info_summary;

    public Button[] Btns_main;  //size = 3
    public RawImage[] RIs_clue; //size = 3
    public Text[] Txts_clue;    //size = 3

    //Resource... 스테이지 / 단서 이미지들
    public Texture[] 
        Textures_stage,         //size = 6
        Textures_clue;          //size = 19

    //변수
    SoundManager SM = SoundManager.Instance;
    List<string> List_clue_txt = new List<string>();    //0~2 : 토양 stage1 단서, 3~5 : 토양  stage2 단서 ....
    StringReader stringReader;
    int clear, stage; //stage >> 1 : 토양 stage1 , 2 : 토양 stage2 , 3 : 수질 stage1 , 4 : 수질 stage2 , 5 : 대기 stage1 , 6 : 대기 stage2

    //상수
    const string LOCATION = "history/";
    readonly string[] PLANET_NAME = {"페페","라라","도도" };
    readonly string[] INFO_SUMMARY = {
        "낯선 집 안에서 정신을 차리게 된 줄리앙.\n꼬륵족은 페페행성을 살리기 위해 줄리앙에게 자신의 방 안 문제점을 찾아달라고 도움을 요청하는데...",                            //토양1
        "꼬륵이를 따라 페페행성의 공원에 도착한 줄리안.공원에서도 토양오염을 발생시키는 원인들이 보여 문제를 해결해주려 하는데...",                                           //토양2
        "페페행성을 도와주고 라라행성에 도착한 줄리안.심각하게 물이 오염 된 라라행성.\n뿌직이는 줄리안에게 자신의 방 안 문제점을 찾아달라고 도움을 요청하는데...",            //수질1
        "라라행성의 바다에 도착한 뿌직이와 줄리안.\n바다에서도 수질오염을 발생시키는 원인들이 보여 문제를 해결해주려 하는데...",                                                //수질2
        "마지막 행성인 도도행성에 도착한 줄리안.\n심각한 대기오염으로 숨 조차 쉬기 힘든 도도행성.콜록이는 줄리안에게 자신의 방 안 문제점을 찾아달라고 도움을 요청하는데..",   //대기1
        "콜록이의 집 밖으로 나온 줄리안.\n길거리에 대기오염을 발생시키는 원인들이 보인다. 줄리안은 도도행성의 문제를 해결해주고 지구로 돌아갈 수 있을까..."                     //대기2
    };
    readonly Color COLOR_PRIVATE = new Color(56 / 255f, 56 / 255f, 56 / 255f);


    //초기화
    void Start()
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
        stage = 1;
        SetStage();
        Btn_Back.gameObject.SetActive(false);
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
        return stage < clear ? true : false;
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
            RIs_clue[i].gameObject.SetActive(true);

        //스테이지 이미지 설정
        RI_main.texture = Textures_stage[stage - 1];
        RI_main.color = IsClearStage() ? Color.white : COLOR_PRIVATE;

        //단서 이미지 설정
        for (int i = 0; i < 3; i++)
            RIs_clue[i].texture = IsClearStage() ? Textures_clue[(stage - 1) * 3 + i] : Textures_clue[18];

    }

    void SetStage()
    {
        SetTxtClue(stage);
        SetRIClue(stage);
        SetInfo(stage);
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
        Btn_Back.gameObject.SetActive(false);
        Btn_Next.gameObject.SetActive(true);
    }

    public void BE_Back()
    {
        SM.Play_effect(0);

        stage--;

        SetStage();
        Btn_Back.gameObject.SetActive(false);
        Btn_Next.gameObject.SetActive(true);
    }
    public void BE_Next()
    {
        SM.Play_effect(0);

        stage++;

        SetStage();
        Btn_Back.gameObject.SetActive(true);
        Btn_Next.gameObject.SetActive(false);
    }

    public void BE_clue(int num)
    {

        SM.Play_effect(0);

        if (!IsClearStage())
            return;

        GameObject ri, txt;
        txt = Txts_clue[num-1].gameObject;
        ri = RIs_clue[num - 1].gameObject;

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

}