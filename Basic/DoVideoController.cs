using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DoVideoController : MonoBehaviour {

    public VideoClip[] videos;
    public VideoPlayer video_player;
    public AudioSource video_audio;
    public GameObject go_video,go_UI, go_bask, go_next, go_shadow;
    public Text txt_title;
    public Image img_mask;
    public RawImage ri_video,ri_play;

    public Texture[] texture_play;

    //변수
    SoundManager SM;
    Color alpha = Color.white;
    float volum;
    int stage_num;
    bool page = false;

    //상수
    readonly string[] titles = new string[2] { "Intro : 세 개의 행성", "Outro : 지구 이전 이야기" };

    void Start()
    {
        SM = SoundManager.Instance;
        stage_num = PlayerPrefs.GetInt("tmp_Clear", 0); ;
        volum = PlayerPrefs.GetFloat("tmp_bgm", 1f);
        video_audio.volume = volum;
    }

    void OnEnable()
    {
        go_shadow.SetActive(false);
        go_UI.SetActive(true);
        video_player.time = 0;
        alpha.a = 0.3f;
        ri_video.color = alpha;

        StartCoroutine(routine_Appear());
    }

    public void Play()
    {
        SM.Play_effect(0);

        if (page && stage_num < 9)
            return;

        if(video_player.clip == null)
            video_player.clip = videos[0];

        if (video_player.isPlaying)
        {
            SM.SetBGM_Volum(volum);
            go_UI.SetActive(true);
            video_player.Pause();
            alpha.a = 0.3f;
        }
        else
        {
            SM.SetBGM_Volum(0);
            go_UI.SetActive(false);
            ri_video.color = Color.white;
            video_player.Play();
            alpha.a = 1f;
        }
        ri_video.color = alpha;
    }

    public void SetClip(int num)
    {
        video_player.clip = videos[num];
    }

    public void BE_ChangVideo(bool next)
    {
        SoundManager.Instance.Play_effect(0);
        page = next;
        go_next.SetActive(!next);
        go_bask.SetActive(next);
        txt_title.text = titles[next ? 1 : 0];
        video_player.clip = videos[next ? 1 : 0];
        video_player.time = 0;

        if (next && stage_num < 9)
            ri_play.texture = texture_play[1];
        else
            ri_play.texture = texture_play[0];
    }

    IEnumerator routine_Appear()
    {
        Color alpha = Color.black;
        for (int i = 40; i > 0; i--)
        {
            alpha.a = 1 - (0.01f * i);
            img_mask.color = alpha;
            go_video.transform.localPosition = Vector3.down * i * 0.5f;
            yield return null;
        }
        go_shadow.SetActive(true);
    }
}
