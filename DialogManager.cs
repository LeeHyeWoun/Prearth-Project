using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public Image color_filter, img_base, btn_skip;
    public RawImage ri_character, ri_name_box;
    public Text t_name, t_dialog;
    public Texture
        img_jullian, img_alien1, img_alien2, img_alien3;

    //변수
    Texture img_alien, img_name_j, img_name_a;
    StringReader stringReader;
    string file_name, fileLine, previous_code;
    string location = "dialog/";
    int lineCount = 0;
    int map_num;
    bool fast = false;
    bool change_chracter = false;
    float origin;
    Color color = Color.white;

    //런타임 상수 선언
    readonly string[] names = { "줄리안", "꼬륵이", "뿌직이", "콜록이" };
    readonly Color[] colors = {
        new Color(203/255f,    112 /255f,  143/255f),               // pink
        new Color(11 /255f,    146 /255f,  214/255f),               // blue
        new Color(221/255f,    159 /255f,  60 /255f) };             // yellow

    //커스텀 클래스 인스턴스
    SceneController SC;


    void Awake() {
        map_num = PlayerPrefs.GetInt("TheMapIs", 1);
        file_name = PlayerPrefs.GetString("DIALOG");

        SC = GetComponent<SceneController>();
    }

    private void Start () {
        //전달 값 체크
        if (file_name.Length <= 0)
        {
            print("Error : 전달 받은 값이 없습니다. 대신 test.txt를 출력합니다.");
            file_name = "test";
        }

        //경로 설정
        if(!file_name.Equals("test"))
            location += SC.GetActiveScene_num() + "/";

        //대사 파일 불러오기 및 파일 존재 여부 체크
        TextAsset file = Resources.Load(location + file_name) as TextAsset;
        if (file == null)
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/dialog/' 경로에서 파일<" + file_name + ".txt>를 찾을 수 없습니다.");
            return;
        }
        print("Dialog <" + file_name + ">");
        stringReader = new StringReader(file.text);

        //행성에 따라 배경과 외계인 세팅
        SettingScene();

        //대화 코루틴
        StartCoroutine("Routine_talking");

        //전달값 초기화
        PlayerPrefs.SetString("DIALOG", null);
    }


    bool FormatCheck() {
        // ':'를 기준으로 캐릭터 코드와 대사 분리
        string[] split_Line = fileLine.Split(':');

        //파일 형식 오류 검사
        if (split_Line.Length != 2)
        {// ':' 체크
            print("Error : 파일<" + file_name + "> 파일 형식을 다시 체크해주세요.\n " + lineCount + "번째 줄에':'가 없거나 오용되었습니다.");
            return false;
        }
        if (split_Line[0] != "0" && split_Line[0] != "1")
        {//캐릭터 코드 체크
            print("Error : 파일<" + file_name + "> 파일 형식을 다시 체크해주세요.\n " + lineCount + "번째 줄 첫 글자로 '" + split_Line[0] + "가 들어왔습니다.");
            return false;
        }


        //변경 여부 설정
        if (previous_code == null || !previous_code.Equals(split_Line[0]))
            change_chracter = true;

        //현재 화자 설정, 가공된 대사 저장
        previous_code = split_Line[0];
        fileLine = split_Line[1];

        return true;
    }

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
            ri_character.texture = img_jullian;
        }
        //외계인 이미지로 변경
        else
        {
            t_name.text = names[map_num];
            ri_character.texture = img_alien;
        }
    }

    //기본 이미지 및 색상 등을 설정
    void SettingScene()
    {

        t_name.color = colors[map_num-1];
        btn_skip.color = colors[map_num - 1];
        img_base.color = colors[map_num - 1];
        switch (map_num)
        {
            case 2:
                img_alien = img_alien2;
                break;

            case 3:
                img_alien = img_alien3;
                break;

            default:
                img_alien = img_alien1;
                break;
        }
    }

    //버튼 이벤트 >> 빠르게 보기
    public void Fast() {
        fast = true;
    }

    //버튼 이벤트 >> 넘어가기
    public void Skip()
    {
        StopCoroutine("Routine_talking");

        //창 사라지기
        StartCoroutine(Routine_disappear());
    }

    //코루틴 >> 대화창 등장하기
    IEnumerator Routine_appear()
    {

        lineCount++;

        //첫 대사 입력
        fileLine = stringReader.ReadLine();

        //대사 초기화 및 화자 설정
        t_dialog.text = "";
        ChangeCharacter();

        //투명화
        color.a = 0;
        ri_character.color = color;
        ri_name_box.color = color;
        btn_skip.gameObject.SetActive(false);

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
            ri_character.color = color;
            ri_name_box.color = color;
            yield return null;
        }
        btn_skip.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));

        //한 음절씩 출력
        yield return StartCoroutine(Routine_wording());


    }

    //코루틴 >> 대화창 사라지기
    IEnumerator Routine_disappear()
    {
        //캐릭터 및 대사 설정
        btn_skip.gameObject.SetActive(false);
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));

        for (int i = 0; i < 5; i++)
        {
            color.a -= 0.2f;
            ri_character.color = color;
            ri_name_box.color = color;
            t_dialog.color = color;
            yield return null;
        }
        yield return StartCoroutine(WaitForUnscaledSeconds(0.5f));

        //배경과 대화창 설정
        Color color_bg = color_filter.color;
        float time = 0;
        if (file_name.Substring(3).Equals("clear"))
        {
            for (int i = 0; i < 50; i++)
            {
                //위에서 아래로 내리기
                img_base.transform.Translate(Vector3.down * 10);
                yield return null;
            }
            SC.Change_Scene(10);
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
            if (!file_name.Equals("test"))
                SC.Destroy_Scene();
        }

    }

    //코루틴 >> 한 음절씩 출력
    IEnumerator Routine_wording() {

        fast = false;

        string sentence = "";
        for (int i = 0; i < fileLine.Length; i++)
        {
            if (fast)
            {
                fast = false;
                t_dialog.text = fileLine;
                break;
            }
            sentence += fileLine[i];
            t_dialog.text = sentence;

            yield return StartCoroutine(WaitForUnscaledSeconds(0.05f));
        }

        yield return StartCoroutine(WaitForUnscaledSeconds(1.5f));
    }

    //코루틴 >> 한 문장씩 출력
    IEnumerator Routine_talking() {

        //창 나타내기
        yield return StartCoroutine(Routine_appear());

        //file의 남은 대사가 없을 때까지 반복
        do
        {
            lineCount++;

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

    //코루틴 >> WaitforSecond를 대체
    IEnumerator WaitForUnscaledSeconds(float how)
    {
        float current = 0f;
        while (current < how)
        {
            yield return null;
            current += Time.unscaledDeltaTime;
        }
    }
}
