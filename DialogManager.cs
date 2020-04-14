using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public Image dialogBase;
    public RawImage character_img, character_name;
    public Text chatTxt;
    public Texture
        img_jullian, img_alien1, img_alien2, img_alien3,
        img_name_j1, img_name_j2, img_name_j3, 
        img_name_a1, img_name_a2, img_name_a3;

    //변수
    Texture img_alien, img_name_j, img_name_a;
    StringReader stringReader;
    string file_name, fileLine, previous_code;
    int lineCount = 0;
    bool fast = false;
    bool change_chracter = false;

    //상수
    const string LOCATION = "dialog/";

    readonly Color color_base1 = new Color(203/255f, 112/255f, 143/255f);
    readonly Color color_base2 = new Color(11/255f, 146/255f, 214/255f);
    readonly Color color_base3 = new Color(221/255f, 159/255f, 60/255f);


    private void Start () {
    
        //전달 값 체크
        file_name = PlayerPrefs.GetString("DIALOG");
        if (file_name.Length <= 0)
        {
            print("Error : 전달 받은 값이 없습니다. 대신 test.txt를 출력합니다.");
            file_name = "test";
        }

        //대사 파일 불러오기 및 파일 존재 여부 체크
        TextAsset file = Resources.Load(LOCATION + file_name) as TextAsset;
        if (file == null)
        {
            print("Error : 파일명을 다시 체크해주세요.\n 'Resource/dialog/' 경로에서 파일<" + file_name + ".txt>를 찾을 수 없습니다.");
            return;
        }
        print("Dialog <" + file_name + ">");
        stringReader = new StringReader(file.text);

        //행성에 따라 배경과 외계인 세팅
        SettingScene(file_name.Equals("test") ? true : false);

        //대화 코루틴
        StartCoroutine(Routine_talking());

        //전달값 초기화
        PlayerPrefs.SetString("DIALOG", null);
    }


    bool FormatCheck() {
        // ':'를 기준으로 캐릭터 코드와 대사 분리
        string[] split_Line = fileLine.Split(':');

        //파일 형식 오류 검사
        if (split_Line.Length != 2)                                         // ':' 체크
        {
            print("Error : 파일<" + file_name + "> 파일 형식을 다시 체크해주세요.\n " + lineCount + "번째 줄에':'가 없거나 오용되었습니다.");
            return false;
        }
        if (split_Line[0] != "0" && split_Line[0] != "1")                   //캐릭터 코드 체크
        {
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
            character_img.texture = img_jullian;
            character_name.texture = img_name_j;
        }
        //외계인 이미지로 변경
        else
        {
            character_img.texture = img_alien;
            character_name.texture = img_name_a;
        }
    }

    void SettingScene(bool test = false)
    {
        int mapNum = test ? 3 : PlayerPrefs.GetInt("TheMapIs");
        switch (mapNum)
        {
            case 2:
                dialogBase.color = color_base2;
                img_alien = img_alien2;
                img_name_j = img_name_j2;
                img_name_a = img_name_a2;
                break;
            case 3:
                dialogBase.color = color_base3;
                img_alien = img_alien3;
                img_name_j = img_name_j3;
                img_name_a = img_name_a3;
                break;
            default:
                dialogBase.color = color_base1;
                img_alien = img_alien1;
                img_name_j = img_name_j1;
                img_name_a = img_name_a1;
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
        if (file_name.Substring(3).Equals("clear"))
            GetComponent<SceneController>().Change_Scene(10);

        else
            GetComponent<SceneController>().Destroy_Scene();

    }

    //코루틴 >> 한 음절씩 출력
    IEnumerator Routine_wording() {

        fast = false;

        string sentence = "";
        float current;
        for (int i = 0; i < fileLine.Length; i++)
        {
            if (fast)
            {
                fast = false;
                chatTxt.text = fileLine;
                break;
            }
            sentence += fileLine[i];
            chatTxt.text = sentence;

            current = 0f;
            while (current < 0.05f)
            {
                yield return null;
                current += Time.unscaledDeltaTime;
            }

            yield return null;
        }

        current = 0f;
        while (current < 1.5f)
        {
            yield return null;
            current += Time.unscaledDeltaTime;
        }

        yield return null;

    }

    //코루틴 >> 팝업
    IEnumerator Routine_appear() {

        lineCount++;

        //첫 대사 입력
        fileLine = stringReader.ReadLine();

        //대사 초기화 및 화자 설정
        chatTxt.text = "";
        ChangeCharacter();
        
        //아래에서 위로 올라오기
        dialogBase.transform.Translate(new Vector3(0, -500f, 0));
        for (int i = 0; i < 50; i++)
        {
            dialogBase.transform.Translate(new Vector3(0, 10f, 0));
            yield return null;
        }

        //한 음절씩 출력
        yield return StartCoroutine(Routine_wording());


    }
    IEnumerator Routine_disappear() {
        //아래로 내리기
        for (int i = 0; i < 50; i++)
        {
            dialogBase.transform.Translate(new Vector3(0, -10f, 0));
            yield return null;
        }
        if(file_name.Substring(3).Equals("clear"))
            GetComponent<SceneController>().Change_Scene(10);

        if (!file_name.Equals("test"))
            GetComponent<SceneController>().Destroy_Scene();
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
                chatTxt.text = "";
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
