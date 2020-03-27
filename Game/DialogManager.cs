using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  대화창과 명령창을 관리하는 스크립트
 *  
 *  Applied Location :
 *  -> Scene# 11이상의 게임 씬 GameDirector(빈 오브젝트)
 */

public class DialogManager : MonoBehaviour {

    //할당받을 객체
    public ParticleSystem order_effect;
    public GameObject order_RI, order_T, Blur, Blur_P, Dialog, clearPop;
    public Text ChatTxt;
    public RawImage character_img, character_name;
    public Texture img_jullian, img_alien, name_j, name_a;

    //상수
    const string LOCATION = "dialog/";
    const string CODE_JULLIAN = "0";

    //변수
    string txt;
    string currentCharacter;
    StringReader sr;
    bool fast = false;
    bool isEnd = false;
    int sceneNum;
    IEnumerator current, next;

    //커스텀 클래스 인스턴스
    RaycastManager RM;
    ZoomManager ZM;
    SoundController SC;

    //미리 캐싱
    readonly WaitForSeconds ShortTerm = new WaitForSeconds(0.5f);
    readonly WaitForSeconds LongTerm = new WaitForSeconds(0.8f);
    readonly WaitForSeconds speakingTerm = new WaitForSeconds(0.1f);
    readonly WaitForSeconds orderTerm = new WaitForSeconds(3f);
    readonly WaitForSeconds fadeTerm = new WaitForSeconds(0.02f);

    void Start()
    {
        RM = GetComponent<RaycastManager>();
        ZM = GetComponent<ZoomManager>();
        SC = GetComponent<SoundController>();

        //현재 씬에 따라 다른 start 대사 가져오기
        sceneNum = int.Parse(SceneManager.GetActiveScene().name.Substring(0,2));
        Dialog_Start(sceneNum + "_start", "꼬륵이의 집에서 문제의 단서를 찾아보세요.");
    }

    private void SetPlays(bool b)
    {
        RM.SetPlay(b);
        ZM.SetPlay(b);
    }

    public void Dialog_Start(string file, string order) {
        print("Dialog <" + file + ">");
        current = routine_Dialog(file);
        if (order != null)
            next = routine_Order(order);
        else {
            next = routine_ending();
            isEnd = true;
        }
        StartCoroutine(current);
    }

    public void Dialog_Skip() {
        StopCoroutine(current);
        StartCoroutine(routine_Close());
    }

    public void Message(string txt) {
        StartCoroutine(routine_Order(txt));
    }

    public void fastSpeaking() {
        fast = true;
    }

    IEnumerator routine_Dialog(string filename) {

        //스테이지 크기 초기화
        while (Camera.main.orthographicSize < 5) {
            ZM.Zoom(5f);
            yield return null;
        }

        //대사 파일 불러오기
        TextAsset textFile = Resources.Load(LOCATION + filename) as TextAsset;
        if (textFile == null)
        {
            print("Error : 파일명을 다시 체크해주세요.");
            yield break;
        }
        else
        {
            sr = new StringReader(textFile.text);
            txt = sr.ReadLine();
        }

        //대화창 띄우기
        yield return StartCoroutine(routine_Pop());

        //한 줄씩 대사 읽기
        while (txt != null)
        {
            string speak = txt.Substring(1);

            setCharacter(txt);
            ChatTxt.text = "";

            yield return ShortTerm;
            yield return StartCoroutine(routine_Speaking(speak));
            yield return ShortTerm;

            txt = sr.ReadLine();
        }

        yield return LongTerm;

        //대화창 닫기
        StartCoroutine(routine_Close());
    }

    IEnumerator routine_Speaking(string narration) {
        txt = "";
        fast = false;

        for (int i = 0; i < narration.Length; i++) {
            if (fast) {
                fast = false;
                i = narration.Length-1;
                txt = narration;
            }
            else
                txt += narration[i];

            ChatTxt.text = txt;
            yield return speakingTerm;
        }
    }

    IEnumerator routine_Order(string txt)
    {
        //초기화
        RawImage oderRI = order_RI.GetComponent<RawImage>();
        Text oderT = order_T.GetComponent<Text>();
        Color fade_RI = oderRI.color;
        Color fade_T = oderT.color;
        float fade_A = 0f;

        fade_RI.a = fade_A;
        fade_T.a = fade_A;
        oderRI.color = fade_RI;
        oderT.color = fade_T;
        oderT.text = txt;

        //열기
        order_RI.SetActive(true);
        while (fade_RI.a < 1)
        {
            fade_A += 0.075f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;
            oderRI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }

        //대기
        yield return orderTerm;

        //닫기
        while (fade_RI.a > 0) {
            fade_A *= 0.94f;
            if (fade_A < 0.2)
                fade_A -= 0.01f;
            fade_RI.a = fade_A;
            fade_T.a = fade_A;

            oderRI.color = fade_RI;
            oderT.color = fade_T;
            yield return null;
        }
        order_RI.SetActive(false);

    }

    IEnumerator routine_Pop() {
        SetPlays(false);

        Dialog.transform.Translate(new Vector3(0, -500f, 0));
        Blur.SetActive(true);
        Dialog.SetActive(true);
        setCharacter(txt);
        ChatTxt.text = "";
        for (int i = 0; i < 50; i++)
        {
            Dialog.transform.Translate(new Vector3(0, 10f, 0));
            yield return null;
        }
        Blur_P.SetActive(true);
    }

    IEnumerator routine_Close() {
        Blur_P.SetActive(false);
        for (int i = 0; i < 50; i++) {
            Dialog.transform.Translate(new Vector3(0, -10f, 0));
            yield return null;
        }
        if (!isEnd)
            Blur.SetActive(false);


        Dialog.SetActive(false);
        Dialog.transform.Translate(new Vector3(0, 500f, 0));

        SetPlays(true);

        current = next;
        StartCoroutine(current);
        order_effect.Play();
    }

    IEnumerator routine_ending() {
        //소리 조절
        if (SC.IsSoundManager())
        {
            float bgm_value = PlayerPrefs.GetFloat("bgm", 1f);
            while (bgm_value > 0)
            {
                SC.SetBGM_Volum(bgm_value);
                bgm_value -= 0.01f;
                yield return null;
            }
        }

        //클리어하지 않은 스테이지일 경우 
        int tmp = PlayerPrefs.GetInt("tmp_Stage", 0);
        if (tmp <= (sceneNum - 10))
        {
            tmp = sceneNum - 10;
            PlayerPrefs.SetInt("tmp_Stage", tmp);

            //클리어 창 팝업 + fade in effedt
            clearPop.SetActive(true);
            Image clearImage = clearPop.GetComponent<Image>();
            Color color = clearImage.material.color;
            int time = 4;
            while (time < 10) {
                time++;
                color.a = time * 0.1f;
                clearImage.material.color = color;
                yield return null;
            }
            Blur_P.SetActive(true);
        }
        //이미 클리어한 스테이지일 경우 그냥 맵으로 돌아가기
        else
        {
            GetComponent<SceneController>().BackMap();
        }
    }

    void setCharacter(string text) {
        string txt_FstLetter = text.Substring(0, 1);
        if (currentCharacter==null || !currentCharacter.Equals(txt_FstLetter)) {
            currentCharacter = txt_FstLetter;
            if (currentCharacter.Equals(CODE_JULLIAN))
            {
                character_name.texture = name_j;
                character_img.texture = img_jullian;
            }
            else
            {
                character_name.texture = name_a;
                character_img.texture = img_alien;
            }
        }
    }
}
