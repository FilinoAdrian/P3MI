using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PointOfInterest : MonoBehaviour, IInputClickHandler
{
    public string sceneName;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject newContent = GameObject.Find(sceneName + "Content");
        if (!newContent)
        {
            TransitionManager.Instance.LoadView(sceneName);
        }
    }
}