﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {

    //UI
    public SuperBlur.SuperBlur blur_mainCmr;
    public GameObject 
        go_clear, go_commentary, go_correct, go_wrong, 
        go_btn_back, go_btn_next;
    public Text t_question, t_count, t_page, t_q, t_a;
    public Text[] Txts_option;      //size = 4
    public Button[] Btns_option;    //size = 4

    //변수
    char[] corrects = new char[6];
    bool wrong = false;
    int page, count = 0;
    StringReader q_stringReader, c_stringReader;
    SceneController SC;
    string c_tmp;
    List<string> commentationList = new List<string>();

    //상수
    const string LOCATION = "quiz/";
    readonly char[] corrects1 = { '1', '4', '3', '2', '2', '3' };
    readonly char[] corrects2 = { '1', '2', '4', '1', '3', '2' };
    readonly char[] corrects3 = { '3', '1', '1', '1', '2', '3' };
    readonly Color[] colors_option = {
        new Color(230 / 255f, 231 / 255f, 232 / 255f),
        new Color(55 / 255f, 183 / 255f, 107 / 255f),
        new Color(239 / 255f, 60 / 255f, 60 / 255f),
    };
    readonly Color color_t_option = new Color(92 / 255f, 100 / 255f, 102 / 255f);
    readonly WaitForSeconds term = new WaitForSeconds(2f);


    //초기화------------------------------------------------------------------------------------------
    void Start () {
        SC = GetComponent<SceneController>();
        
        //대화창
        PlayerPrefs.SetString("DIALOG", SC.GetActiveScene_num() + "_start");
        SC.Load_Scene(14);

        string q_file_name = "quiz"+ (SC.GetActiveScene_num()-1)/3;
        string c_file_name = "commentation" + (SC.GetActiveScene_num() - 1) / 3;

        //대사 파일 불러오기
        TextAsset q_file = Resources.Load(LOCATION + q_file_name) as TextAsset;
        TextAsset c_file = Resources.Load(LOCATION + c_file_name) as TextAsset;

#if DEV_TEST
        //파일 존재 여부 체크
        if (q_file.Equals(null) || c_file.Equals(null))
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/dialog/' 경로에서 파일<" + q_file_name + ".txt>를 찾을 수 없습니다.");
            return;
        }
#endif
        q_stringReader = new StringReader(q_file.text);
        c_stringReader = new StringReader(c_file.text);
        SetQuiz();

        //정답 설정
        switch (q_file_name) {
            case "quiz1":
                corrects = corrects1;
                break;

            case "quiz2":
                corrects = corrects2;
                break;

            case "quiz3":
                corrects = corrects3;
                break;
        }
    }


    //privaste 함수들---------------------------------------------------------------------------------------------------------------
    void Next_Question()
    {
        page++;
        Reset();
    }

    void Open_Result()
    {
        t_count.text = count.ToString();
        blur_mainCmr.enabled = true;
        go_clear.SetActive(true);
    }

    void SetQuiz() {
        t_question.text = q_stringReader.ReadLine();
        for(int i = 0; i<4; i++)
            Txts_option[i].text = q_stringReader.ReadLine();
        t_page.text = (page + 1) + " / 6";
    }

    void Reset() {
        //내용 설정
        SetQuiz();

        //글자색 초기화
        for (int i = 0; i < 4; i++)
            Txts_option[i].color = color_t_option;
        
        for (int i = 0; i < 4; i++)
        {
            //버튼 색 초기화
            Btns_option[i].GetComponent<Image>().color = colors_option[0];
            //클릭 활성화
            Btns_option[i].interactable = true;
        }
    }


    //public 함수들-----------------------------------------------------------------------------------------------------------------
    public void Select(int num) {
        //선택된 버튼 구분
        Button option = Btns_option[num-1];
        Text t_option = Txts_option[num-1];

        //정답 이벤트
        if (num.Equals(corrects[page]-'0'))
        {
            for(int i=0; i<4; i++)
                Btns_option[i].interactable = false;

            if (wrong)
                wrong = false;
            else
                count++;

            SoundManager.Instance.Play_effect(1);
            option.GetComponent<Image>().color = colors_option[1];
            StartCoroutine(Routine_check(true));

            //해설 저장
            commentationList.Add("Q" + (page + 1) + ". " + t_question.text);
            commentationList.Add(c_stringReader.ReadLine());

            if (page.Equals(5))
                Invoke("Open_Result", 2f);
            else
                Invoke("Next_Question", 2f);
        }
        //오답 이벤트
        else
        {
            option.interactable = false;

            wrong = true;

            SoundManager.Instance.Play_effect(2);
            option.GetComponent<Image>().color = colors_option[2];
            StartCoroutine(Routine_check(false));
        }
        t_option.color = Color.white;
    }

    public void Open_Commentary(bool open) {
        SoundManager.Instance.Play_effect(0);
        go_commentary.SetActive(open);
        if (open) {
            page = 0;
            t_q.text = commentationList[0];
            t_a.text = commentationList[1];
            go_btn_next.SetActive(true);
            go_btn_back.SetActive(false);
        }
    }

    public void Close_Quiz() {
        SoundManager.Instance.Play_effect(0);
        blur_mainCmr.enabled = false;
        go_clear.SetActive(false);
        PlayerPrefs.SetString("DIALOG", SC.GetActiveScene_num()+"_clear");
        SC.Load_Scene(14);
    }

    public void Back()
    {
        SoundManager.Instance.Play_effect(0);
        page--;
        t_q.text = commentationList[page * 2];
        t_a.text = commentationList[page * 2 + 1];

        if(!go_btn_next.activeSelf)
            go_btn_next.SetActive(true);
        if (page.Equals(0))
            go_btn_back.SetActive(false);
    }

    public void Next() {
        SoundManager.Instance.Play_effect(0);
        page++;
        t_q.text = commentationList[page * 2];
        t_a.text = commentationList[page * 2 + 1];

        if(!go_btn_back.activeSelf)
            go_btn_back.SetActive(true);
        if ((page+1).Equals(commentationList.Count/2))
            go_btn_next.SetActive(false);
    }

    IEnumerator Routine_check(bool correct) {
        GameObject check;
        if (correct)
        {
            go_wrong.SetActive(false);
            check = go_correct;
        }
        else
            check = go_wrong;
        check.SetActive(true);

        yield return term;

        check.SetActive(false);
    }

}
