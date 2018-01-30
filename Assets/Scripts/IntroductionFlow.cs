// Copyright Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using UnityEngine;

public class IntroductionFlow : Singleton<IntroductionFlow>
{
    public GameObject Logo;
    public float IntroLogoTime = 4.5f;
    public float timeToFade = 2.0f;
    public float contentFadeIn = 2.0f;
    public Color logoColor;

    public enum IntroductionState
    {
        IntroductionLogoFadeIn,
        IntroductionLogo,
        IntroductionLogoFadeOut,
        PlacingStartingView,
        StartingViewContentFadeIn,
        IntroductionStateComplete
    }

    public IntroductionState currentState = IntroductionState.IntroductionLogoFadeIn;

    private float timeInState = 0.0f;
    private bool coreSystemsLoaded = false;
    private Renderer logoRenderer;
    private bool logoShown = true;
    private string startingViewContentName;
    private GameObject startingViewContent;
    private Renderer[] renderers;

    private void Start()
    {
        if (Logo == null)
        {
            Debug.LogError("IntroductionFlow : Logo is not defined.");
            Destroy(this);
            return;
        }

        logoRenderer = Logo.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case IntroductionState.IntroductionLogoFadeIn:
                if (logoShown)
                {
                    logoRenderer.material.color = new Color(logoColor.r, logoColor.g, logoColor.b, timeInState / timeToFade);
                }

                if (timeInState >= timeToFade)
                {
                    AdvanceIntroduction();
                }

                break;

            case IntroductionState.IntroductionLogo:
                if (timeInState >= IntroLogoTime)
                {
                    AdvanceIntroduction();
                }

                break;

            case IntroductionState.IntroductionLogoFadeOut:
                if (!logoShown)
                {
                    logoRenderer.material.color = new Color(logoColor.r, logoColor.g, logoColor.b, (timeToFade - timeInState) / timeToFade);
                }

                if (timeInState >= timeToFade)
                {
                    AdvanceIntroduction();
                }

                break;

            case IntroductionState.PlacingStartingView:
                startingViewContentName = ViewLoader.Instance.StartingView + "Content";
                startingViewContent = GameObject.Find(startingViewContentName);

                var soundManager = GameObject.Find("SoundManager");

                TextToSpeechManager tt = soundManager.GetComponent<TextToSpeechManager>();
                tt.SpeakText("Air Tap to place the object");

                if (!startingViewContent.GetComponent<TapToPlace>().IsBeingPlaced)
                {
                    AdvanceIntroduction();
                }

                break;

            case IntroductionState.StartingViewContentFadeIn:
                Color lerpedColor;
                foreach(Renderer renderer in renderers)
                {
                    if (renderer.gameObject.name != "Glass")
                    {
                        foreach (Material material in renderer.materials)
                        {
                            lerpedColor = (Color.Lerp(new Color(material.color.r, material.color.g, material.color.b, 0),
                                                      new Color(material.color.r, material.color.g, material.color.b, 1),
                                                      timeInState / timeToFade));
                            material.color = lerpedColor;
                        }
                    }
                }

                if (timeInState >= timeToFade)
                {
                    AdvanceIntroduction();
                }

                break;
        }

        timeInState += Time.deltaTime;
    }

    private void AdvanceIntroduction()
    {
        switch (currentState)
        {
            case IntroductionState.IntroductionLogo:
                logoShown = false;

                break;

            case IntroductionState.IntroductionLogoFadeOut:
                ViewLoader.Instance.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 2.0f);
                ViewLoader.Instance.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - ViewLoader.Instance.transform.position);
                ViewLoader.Instance.LoadStartingView();

                break;

            case IntroductionState.PlacingStartingView:
                startingViewContentName = ViewLoader.Instance.StartingView + "Content";
                startingViewContent = GameObject.Find(startingViewContentName);
                startingViewContent.GetComponent<TapToPlace>().enabled = false;
                GameObject.Find("Footprint").SetActive(false);

                GameObject parent = GameObject.Find("DesaPengotan");

                Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trs)
                {
                    t.gameObject.SetActive(true);
                }

                renderers = parent.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer renderer in renderers)
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
                        material.SetFloat("_ZWrite", 1);
                    }
                }

                break;
        }

        currentState = (IntroductionState)(currentState + 1);
        if (currentState == IntroductionState.IntroductionStateComplete)
        {
            Destroy(gameObject);
            return;
        }

        timeInState = 0.0f;
    }
}
