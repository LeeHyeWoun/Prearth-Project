using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {

    public GameObject 
        go_blur, go_result, go_commentary, go_correct, go_wrong, 
        go_btn_commentary, go_btn_close, 
        go_btn_back, go_btn_next;
    public Text t_question, t_option1, t_option2, t_option3, t_option4, t_count, t_page, t_q, t_a;
    public Button btn_option1, btn_option2, btn_option3, btn_option4;
    public Sprite img_option, img_option_correct, img_option_wrong;

    //변수
    char[] corrects = new char[6];
    bool wrong = false;
    int page, count = 0;
    StringReader stringReader;
    List<string> wrongList = new List<string>();


    //상수
    const string LOCATION = "quiz/";
    readonly char[] corrects1 = { '1', '4', '3', '2', '2', '3' };
    readonly char[] corrects2 = { '1', '2', '4', '1', '3', '2' };
    readonly char[] corrects3 = { '3', '1', '1', '1', '2', '3' };
    readonly Color color_t_option = new Color(92 / 255, 100 / 255, 102 / 255);
    readonly WaitForSeconds term = new WaitForSeconds(2f);

    SceneController SC;


    void Start () {
        SC = GetComponent<SceneController>();
        
        //대화창
        PlayerPrefs.SetString("DIALOG", SC.GetActiveScene_num() + "_start");
        SC.Add_Scene(9);

        string file_name = "quiz"+ (SC.GetActiveScene_num()-10)/3;

        //대사 파일 불러오기 및 파일 존재 여부 체크
        TextAsset file = Resources.Load(LOCATION + file_name) as TextAsset;
        if (file == null)
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/dialog/' 경로에서 파일<" + file_name + ".txt>를 찾을 수 없습니다.");
            return;
        }
        stringReader = new StringReader(file.text);
        SetQuiz();

        //정답 설정
        switch (file_name) {
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

    void Next_Question()
    {
        page++;
        Reset();
    }

    void Open_Result()
    {
        if (count.Equals(6)) {
            go_btn_commentary.SetActive(false);

            Vector3 v3 = go_btn_close.transform.position;
            v3.x = go_result.transform.position.x;
            go_btn_close.transform.position=v3;
        }

        t_count.text = count.ToString();
        go_blur.SetActive(true);
        go_result.SetActive(true);
    }

    void SetQuiz() {
        t_question.text = stringReader.ReadLine();
        t_option1.text = stringReader.ReadLine();
        t_option2.text = stringReader.ReadLine();
        t_option3.text = stringReader.ReadLine();
        t_option4.text = stringReader.ReadLine();
        t_page.text = (page + 1) + " / 6";
    }

    void Reset() {
        //내용 설정
        SetQuiz();

        //글자색 초기화
        t_option1.color = color_t_option;
        t_option2.color = color_t_option;
        t_option3.color = color_t_option;
        t_option4.color = color_t_option;

        //버튼 색 초기화
        btn_option1.GetComponent<Image>().sprite = img_option;
        btn_option2.GetComponent<Image>().sprite = img_option;
        btn_option3.GetComponent<Image>().sprite = img_option;
        btn_option4.GetComponent<Image>().sprite = img_option;

        //클릭 활성화
        btn_option1.interactable = true;
        btn_option2.interactable = true;
        btn_option3.interactable = true;
        btn_option4.interactable = true;
    }

    public void Select(int num) {
        //선택된 버튼 구분
        Button option;
        Text t_option;
        switch (num) {

            case 1:
                option = btn_option1;
                t_option = t_option1;
                break;

            case 2:
                option = btn_option2;
                t_option = t_option2;
                break;

            case 3:
                option = btn_option3;
                t_option = t_option3;
                break;

            default:
                option = btn_option4;
                t_option = t_option4;
                break;
        }

        //정답 이벤트
        if (num.Equals(corrects[page]-'0'))
        {
            btn_option1.interactable = false;
            btn_option2.interactable = false;
            btn_option3.interactable = false;
            btn_option4.interactable = false;

            if (wrong) {
                wrong = false;
                wrongList.Add(t_option.text);
            }
            else
                count++;

            SoundManager.Instance.Play_effect(1);
            option.GetComponent<Image>().sprite = img_option_correct;
            StartCoroutine(Routine_check(true));

            if (page.Equals(5))
                Invoke("Open_Result", 2f);
            else
                Invoke("Next_Question", 2f);
        }
        //오답 이벤트
        else
        {
            option.interactable = false;

            if (!wrong) {
                wrongList.Add("Q" + (page + 1) + ". " + t_question.text);
                wrong = true;
            }

            SoundManager.Instance.Play_effect(2);
            option.GetComponent<Image>().sprite = img_option_wrong;
            StartCoroutine(Routine_check(false));
        }
        t_option.color = Color.white;
    }

    public void Open_Commentary(bool open) {
        SoundManager.Instance.Play_effect(0);
        go_commentary.SetActive(open);
        go_result.SetActive(!open);
        if (open) {
            page = 0;
            t_q.text = wrongList[0];
            t_a.text = wrongList[1];
            if (wrongList.Count.Equals(2))
                go_btn_next.SetActive(false);
        }
    }

    public void Close_Quiz() {
        go_blur.SetActive(false);
        go_result.SetActive(false);
        PlayerPrefs.SetString("DIALOG", SC.GetActiveScene_num()+"_clear");
        SC.Add_Scene(9);
    }

    public void Back()
    {
        SoundManager.Instance.Play_effect(0);
        page--;
        t_q.text = wrongList[page * 2];
        t_a.text = wrongList[page * 2 + 1];

        if(!go_btn_next.activeSelf)
            go_btn_next.SetActive(true);
        if (page.Equals(0))
            go_btn_back.SetActive(false);
    }

    public void Next() {
        SoundManager.Instance.Play_effect(0);
        page++;
        t_q.text = wrongList[page * 2];
        t_a.text = wrongList[page * 2 + 1];

        if(!go_btn_back.activeSelf)
            go_btn_back.SetActive(true);
        if ((page+1).Equals(wrongList.Count/2))
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
