using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class WorldSpaceVideo : MonoBehaviour
{

    public Material playButtonMaterial;
    public Material pauseButtonMaterial;
    public Renderer playButtonRenderer;
    public VideoClip[] videoClips;
    public TextMesh currentMinutes;
    public TextMesh currentSeconds;
    public TextMesh totalMinutes;
    public TextMesh totalSeconds;
    public PlayHeadMover playHeadMover;

    private VideoPlayer videoPlayer;
    private int videoClipIndex;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Use this for initialization
    void Start ()
    {
        videoPlayer.targetTexture.Release();
        videoPlayer.clip = videoClips[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (videoPlayer.isPlaying)
        {
            SetCurrentTimeUI();
            playHeadMover.MovePlayhead(CalculatePlayheadFraction());
        }
	}

    public void SetNextClip()
    {
        videoClipIndex++;

        if (videoClipIndex >= videoClips.Length)
        {
            videoClipIndex = videoClipIndex % videoClips.Length;
        }

        videoPlayer.clip = videoClips[videoClipIndex];
        SetTotalTimeUI();
        videoPlayer.Play();
    }

    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            playButtonRenderer.material = playButtonMaterial;
        }
        else
        {
            videoPlayer.Play();
            SetTotalTimeUI();
            playButtonRenderer.material = pauseButtonMaterial;
        }
    }

    public void CloseVideoSystem()
    {

    }

    void SetCurrentTimeUI()
    {
        string minutes = Mathf.Floor((int)videoPlayer.time / 60).ToString("00");
        string seconds = ((int)videoPlayer.time % 60).ToString("00");

        currentMinutes.text = minutes;
        currentSeconds.text = seconds;
    }

    void SetTotalTimeUI()
    {
        string minutes = Mathf.Floor((int)videoPlayer.clip.length / 60).ToString("00");
        string seconds = ((int)videoPlayer.clip.length % 60).ToString("00");

        totalMinutes.text = minutes;
        totalSeconds.text = seconds;
    }

    double CalculatePlayheadFraction()
    {
        double fraction = (double)videoPlayer.frame / (double)videoPlayer.clip.frameCount;
        return fraction;
    }
}
