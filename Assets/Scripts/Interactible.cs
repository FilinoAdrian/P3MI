using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.InputModule;
using System;

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour, IInputClickHandler, IFocusable
{
    [Range(0.0f, 1.0f)]
    public float emissionFloat = 0.0f;

    public Color emissionColor = Color.white;

    private Material[] defaultMaterials;

    void Start()
    {
        defaultMaterials = GetComponent<Renderer>().materials;

        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    public void OnFocusEnter()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            float emission = Mathf.PingPong(Time.time, emissionFloat);

            Color finalColor = emissionColor * Mathf.LinearToGammaSpace(emission);

            defaultMaterials[i].SetColor("_EmissionColor", finalColor);
        }
    }

    public void OnFocusExit()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color baseColor = Color.black; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            defaultMaterials[i].SetColor("_EmissionColor", finalColor);
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (ApplicationState.Instance.SelectedInteractible != null)
        {
            ApplicationState.Instance.oldSelectedInteractible = ApplicationState.Instance.SelectedInteractible;
        }

        ApplicationState.Instance.SelectedInteractible = gameObject;
    }
}