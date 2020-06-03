using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour {

    public VideoPlayer vp;
    public VideoClip intro, outro;
    public RawImage RI_background, RI_video;

    int stage_num;

    void Start () {
        SoundManager.Instance.SetBGM_Volum(0);
        stage_num = PlayerPrefs.GetInt("tmp_Clear", -3);
        if (stage_num < 9)
            vp.clip = intro;
        else
            vp.clip = outro;

        vp.Play();
        vp.loopPointReached += OnMovieFinished;
    }

    void OnMovieFinished(VideoPlayer player)
    {
        StartCoroutine("End");
    }

    public void Skip() {
        SoundManager.Instance.Play_effect(0);
        StartCoroutine("End");
    }

    IEnumerator End() {
        vp.Stop();

        Color color_bg = RI_background.color;
        Color color_video = RI_video.color;

        float alpha = 1;
        while (alpha > 0)
        {
            alpha *= 0.94f;
            if (alpha < 0.2)
                alpha -= 0.05f;

            color_bg.a = alpha;
            color_video.a = alpha;

            RI_background.color = color_bg;
            RI_video.color = color_video;
            yield return null;
        }

        SoundManager.Instance.SetBGM_Volum(PlayerPrefs.GetFloat("tmp_bgm", 1f));
        PlayerPrefs.SetInt("tmp_Clear",stage_num+1);

        if (stage_num < 9)
            SceneController.Instance.Load_Scene(12);
        else
            SceneController.Instance.Destroy_Scene();
    }
}
