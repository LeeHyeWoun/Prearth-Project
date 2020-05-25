using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public Image color_filter, img_base, img_name, img_character;
    public GameObject go_skip;
    public Text t_name, t_dialog;
    public Sprite[] sprite_character;
    public SpriteAtlas bitPart_characters;

    //변수
    Sprite img_alien;
    StringReader stringReader;
    StringBuilder sentence = new StringBuilder("");
    Color color = Color.white;
    string file_name, fileLine, previous_code;
    string location = "dialog/";
    bool fast = false;
    bool change_chracter = false;
    float origin;
    int planet_num;
#if DEV_TEST
    int lineCount = 0;
#endif

    //런타임 상수 선언
    readonly string[] names = { "줄리안", "꼬륵이", "뿌직이", "콜록이" };
    readonly Color[] colors = {
        new Color(203/255f,    112 /255f,  143/255f),               // pink
        new Color(11 /255f,    146 /255f,  214/255f),               // blue
        new Color(221/255f,    159 /255f,  60 /255f) };             // yellow

    //커스텀 클래스 인스턴스
    SceneController SC = SceneController.Instance;


    void Awake() {
        file_name = PlayerPrefs.GetString("DIALOG");
        planet_num = SceneController.Instance.Planet_num;
    }

    void Start () {
        Time.timeScale = 0;

#if DEV_TEST
        //전달 값 체크
        if (file_name.Length <= 0)
        {
            print("Error : 전달 받은 값이 없습니다. 대신 test.txt를 출력합니다.");
            file_name = "test";
        }
#endif
        //대사 파일 불러오기
        location += SceneController.Instance.GetActiveScene_num() + "/";
        TextAsset file = Resources.Load(location + file_name) as TextAsset;

#if DEV_TEST
        // 파일 존재 여부 체크
        if (file == null)
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/dialog/' 경로에서 파일<" + file_name + ".txt>를 찾을 수 없습니다.");
            return;
        }
        print("Dialog <" + file_name + ">");
#endif
        stringReader = new StringReader(file.text);

        //행성에 따라 배경과 외계인 세팅
        SettingScene();

        //대화 코루틴
        StartCoroutine("Routine_talking");

        //전달값 초기화
        PlayerPrefs.SetString("DIALOG", null);
    }

    #region FormatCheck : 문장의 형식을 확인
    bool FormatCheck() {
        // ':'를 기준으로 캐릭터 코드와 대사 분리
        string[] split_Line = fileLine.Split(':');

#if DEV_TEST
        //파일 형식 오류 검사
        if (split_Line.Length != 2)
        {// ':' 체크
            print("Error : 파일<" + file_name + "> 파일 형식을 다시 체크해주세요.\n " + lineCount + "번째 줄에':'가 없거나 오용되었습니다.");
            return false;
        }
#endif

        //변경 여부 설정
        if (previous_code == null || !previous_code.Equals(split_Line[0]))
            change_chracter = true;

        //현재 화자 설정, 가공된 대사 저장
        previous_code = split_Line[0];
        fileLine = split_Line[1];

        return true;
    }
    #endregion

    #region ChangeCharacter : 화자 설정
    void ChangeCharacter() {

        //파일 형식 체크
        if (!FormatCheck())
            return;

        //변경 여부 체크
        if (!change_chracter)
            return;

        //줄리안 이미지로 변경
        if (previous_code.Equals("0"))
        {
            t_name.text = names[0];
            img_character.sprite = sprite_character[3];
        }
        //외계인 이미지로 변경
        else if (previous_code.Equals("1"))
        {
            t_name.text = names[planet_num + 1];
            img_character.sprite = img_alien;
        }
        else {
            t_name.text = previous_code;
            img_character.sprite = bitPart_characters.GetSprite("img_alien" +(planet_num+1)+"_"+ file_name.Substring(6,1));
        }
    }
    #endregion

    #region SettingScene : 기본 이미지 및 색상 등을 설정
    void SettingScene()
    {

        t_name.color = colors[planet_num];
        go_skip.GetComponent<Image>().color = colors[planet_num];
        img_base.color = colors[planet_num];
        img_alien = sprite_character[planet_num];
    }
    #endregion

    //public 함수---------------------------------------------------------------------------------
    #region 버튼 이벤트 - Fast : 빠르게 보기
    public void Fast() {
        fast = true;
    }
    #endregion

    #region 버튼 이벤트 - Skip : 넘어가기
    public void Skip()
    {
        SoundManager.Instance.Play_effect(0);
        StopCoroutine("Routine_talking");

        //창 사라지기
        StartCoroutine("Routine_disappear");
    }
    #endregion

    //코루틴--------------------------------------------------------------------------------------
    #region 코루틴 - Routine_appear : 대화창 등장하기
    IEnumerator Routine_appear()
    {

#if DEV_TEST
        lineCount++;
#endif

        //첫 대사 입력
        fileLine = stringReader.ReadLine();

        //대사 초기화 및 화자 설정
        t_dialog.text = "";
        ChangeCharacter();

        //투명화
        color.a = 0;
        img_character.color = color;
        img_name.color = color;
        go_skip.SetActive(false);

        //아래에서 시작하기
        img_base.transform.Translate(Vector3.down * 500f);

        Color color_bg = color_filter.color;
        origin = color_bg.a;
        float time = 0;
        for (int i = 0; i < 50; i++)
        {
            //배경 어두워지기
            time = i * 0.02f;
            color_bg.a = origin * time;
            color_filter.color = color_bg;

            //아래에서 위로 올라오기
            img_base.transform.Translate(Vector3.up * 10);

            yield return null;
        }

        //캐릭터 등장
        for (int i = 0; i < 5; i++)
        {
            color.a += 0.2f;
            img_character.color = color;
            img_name.color = color;
            yield return null;
        }
        go_skip.SetActive(true);
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));

        //한 음절씩 출력
        yield return StartCoroutine(Routine_wording());


    }
    #endregion

    #region 코루틴 - Routine_disappear : 대화창 사라지기
    IEnumerator Routine_disappear()
    {
        //캐릭터 및 대사 설정
        go_skip.SetActive(false);
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));

        for (int i = 0; i < 5; i++)
        {
            color.a -= 0.2f;
            img_character.color = color;
            img_name.color = color;
            t_dialog.color = color;
            yield return null;
        }
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));


        //배경과 대화창 설정
        Color color_bg = color_filter.color;
        float time = 0;
        if (file_name.Substring(file_name.IndexOf('_')+1).Equals("clear"))
        {
            for (int i = 0; i < 50; i++)
            {
                //위에서 아래로 내리기
                img_base.transform.Translate(Vector3.down * 10);
                yield return null;
            }
            Time.timeScale = 1;

            if (PlayerPrefs.GetInt("tmp_Clear") < (SC.GetActiveScene_num() - 1))
                SC.Load_Scene(15);
            else {
                //메인용 배경음악으로 초기화
                SoundManager.Instance.BGM_reset();

                //02_Map으로 돌아가기
                SC.Load_Scene(1);
            }

        }
        else {
            for (int i = 0; i < 50; i++)
            {
                //배경 밝아지기
                time = i * 0.02f;
                color_bg.a = origin * (1 - time);
                color_filter.color = color_bg;

                //위에서 아래로 내리기
                img_base.transform.Translate(Vector3.down * 10);
                yield return null;
            }
            Time.timeScale = 1;
            SC.Destroy_Scene();
        }

    }
    #endregion

    #region 코루틴 - Routine_talking : 한 문장씩 출력
    IEnumerator Routine_talking() {

        //창 나타내기
        yield return StartCoroutine(Routine_appear());

        //file의 남은 대사가 없을 때까지 반복
        do
        {
#if DEV_TEST
            lineCount++;
#endif

            //대사 입력
            fileLine = stringReader.ReadLine();

            if (fileLine != null)
            {
                //대사 초기화 및 화자 설정
                t_dialog.text = "";
                ChangeCharacter();

                //한 음절씩 출력
                yield return StartCoroutine(Routine_wording());
            }
            else
                break;
        }
        while (true);

        //창 사라지기
        yield return StartCoroutine(Routine_disappear());

    }
    #endregion

    #region 코루틴 - Routine_wording : 한 음절씩 출력
    IEnumerator Routine_wording()
    {

        fast = false;

        sentence.Remove(0, sentence.Length);
        for (int i = 0; i < fileLine.Length; i++)
        {
            if (fast)
            {
                fast = false;
                t_dialog.text = fileLine;
                break;
            }
            sentence.Append(fileLine[i]);
            t_dialog.text = sentence.ToString();

            yield return StartCoroutine(WaitForUnscaledSeconds(0.05f));
        }

        yield return StartCoroutine(WaitForUnscaledSeconds(1.5f));
    }
    #endregion

    #region 코루틴 - WaitForUnscaledSeconds : WaitforSecond를 대체
    IEnumerator WaitForUnscaledSeconds(float how)
    {
        float current = 0f;
        while (current < how)
        {
            yield return null;
            current += Time.unscaledDeltaTime;
        }
    }
    #endregion
}
