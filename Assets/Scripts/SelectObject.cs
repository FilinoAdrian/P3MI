using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SelectObject : MonoBehaviour, IInputClickHandler
{
    private void Start()
    {
        
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // If not already selected - select, otherwise, deselect
        if (ApplicationState.Instance.SelectedGameObject != gameObject)
        {
            ApplicationState.Instance.SelectedGameObject = gameObject;
        }
    }
}