using HoloToolkit.Unity;
using UnityEngine;

public class ApplicationState : Singleton<ApplicationState>
{
    public GameObject SelectedGameObject;
    public GameObject SelectedInteractible;
    public GameObject oldSelectedInteractible;
}
