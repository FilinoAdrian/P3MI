using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Animator infoAnim;
    public GameObject buildingInfo;

    private bool isInfoOpened = false;
    private ScrollSnapRect scrollImage;
    private bool slideShowPlayed = false;

    // Use this for initialization
    void Start () {
        scrollImage = buildingInfo.GetComponentInChildren<ScrollSnapRect>(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (scrollImage.isActiveAndEnabled)
        {
            if (!slideShowPlayed)
            {
                scrollImage.StartCoroutine("SlideShow");
                slideShowPlayed = true;
            }
        }
	}

    public void OpenInfo()
    {
        if (!isInfoOpened)
        {
            isInfoOpened = true;
            infoAnim.Play("Open_Info_Animation");
        }
        else
        {
            isInfoOpened = false;
            infoAnim.Play("Close_Info_Animation");
            scrollImage.StopCoroutine("SlideShow");
            slideShowPlayed = false;
        }
    }
}
