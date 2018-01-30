// Copyright Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Collections;
using HoloToolkit.Unity;
using HoloToolkit.Examples.InteractiveElements;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ToolManager : Singleton<ToolManager>
{
    public InteractiveToggle PlaceButton;
    public InteractiveToggle MoveButton;
    public InteractiveToggle RotateButton;
    public InteractiveToggle ScaleButton;
    public InteractiveToggle SelectButton;
    public InteractiveToggle ExplodeButton;
    public InteractiveButton CloseButton;

    private GameObject selectedGameObject;
    private MoveComponentOnClick[] moveScripts;
    private MeshRenderer[] buttonRenderer;

    private Vector3[] forwards = new Vector3[4];

    private void Start()
    {
        buttonRenderer = gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    private void FollowBoundingBox(bool smooth)
    {
        // Get positions for each side of the bounding box
        // Choose the one that's closest to us
        forwards[0] = ApplicationState.Instance.SelectedGameObject.transform.forward;
        forwards[1] = ApplicationState.Instance.SelectedGameObject.transform.right;
        forwards[2] = -ApplicationState.Instance.SelectedGameObject.transform.forward;
        forwards[3] = -ApplicationState.Instance.SelectedGameObject.transform.right;
        Vector3 scale = ApplicationState.Instance.SelectedGameObject.transform.localScale;
        float maxXYScale = Mathf.Max(scale.x, scale.y);
        float closestSoFar = Mathf.Infinity;
        Vector3 finalPosition = Vector3.zero;
        Vector3 headPosition = Camera.main.transform.position;

        for (int i = 0; i < forwards.Length; i++)
        {
            Vector3 nextPosition = ApplicationState.Instance.SelectedGameObject.transform.position +
            (forwards[i] * -maxXYScale) +
            (Vector3.up * (scale.y * 0.05f));

            float distance = Vector3.Distance(nextPosition, headPosition);
            if (distance < closestSoFar)
            {
                closestSoFar = distance;
                finalPosition = nextPosition;
            }
        }

        // Follow our bounding box
        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, finalPosition, 0.5f);
        }
        else
        {
            transform.position = finalPosition;
        }
        // Rotate on the y axis
        Vector3 eulerAngles = Quaternion.LookRotation((ApplicationState.Instance.SelectedGameObject.transform.position - finalPosition).normalized, Vector3.up).eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        transform.eulerAngles = eulerAngles;
    }

    private void Update()
    {
        FollowBoundingBox(true);

        if (ApplicationState.Instance.SelectedGameObject.name != ViewLoader.Instance.StartingView + "Content")
        {
            ShowButton(CloseButton);
            ShowToggle(ExplodeButton);
        }
        else
        {
            HideButton(CloseButton);
            HideToggle(ExplodeButton);
        }

        selectedGameObject = ApplicationState.Instance.SelectedGameObject;

        if (PlaceButton.HasSelection)
            selectedGameObject.GetComponent<TapToPlace>().enabled = true;
        else
            selectedGameObject.GetComponent<TapToPlace>().enabled = false;

        if (MoveButton.HasSelection)
            selectedGameObject.GetComponent<HandDragging>().SetDragging(true);
        else
            selectedGameObject.GetComponent<HandDragging>().SetDragging(false);

        if (RotateButton.HasSelection)
            selectedGameObject.GetComponent<HandRotate>().SetRotating(true);
        else
            selectedGameObject.GetComponent<HandRotate>().SetRotating(false);

        if (ScaleButton.HasSelection)
            selectedGameObject.GetComponent<HandResize>().SetResizing(true);
        else
            selectedGameObject.GetComponent<HandResize>().SetResizing(false);

        if (ExplodeButton.HasSelection)
        {
            moveScripts = selectedGameObject.GetComponentsInChildren<MoveComponentOnClick>();

            foreach (MoveComponentOnClick moveScript in moveScripts)
            {
                moveScript.enabled = true;
            }
        }
        else
        {
            moveScripts = selectedGameObject.GetComponentsInChildren<MoveComponentOnClick>();

            foreach (MoveComponentOnClick moveScript in moveScripts)
            {
                moveScript.enabled = false;
            }
        }
    }

    private void ShowButton(InteractiveButton button)
    {
        button.gameObject.SetActive(true);
    }

    private void HideButton(InteractiveButton button)
    {
        button.gameObject.SetActive(false);
    }

    private void ShowToggle(InteractiveToggle button)
    {
        button.gameObject.SetActive(true);
    }

    private void HideToggle(InteractiveToggle button)
    {
        button.gameObject.SetActive(false);
    }

    public void ShowTools()
    {
        foreach(MeshRenderer renderer in buttonRenderer)
        {
            renderer.enabled = true;
        }
    }

    public void HideTools()
    {
        foreach (MeshRenderer renderer in buttonRenderer)
        {
            renderer.enabled = false;
        }
    }

    public void ResetPosition()
    {
        Vector3 startPos = ApplicationState.Instance.SelectedGameObject.transform.TransformPoint(
            ApplicationState.Instance.SelectedInteractible.GetComponent<HandDragging>().startPos);
        StartCoroutine(MoveToPosition(startPos, 1f));
        ApplicationState.Instance.SelectedInteractible.GetComponent<HandDragging>().startPosHasSet = false;
    }

    public void CloseObject()
    {
        TransitionManager.Instance.CloseView(ApplicationState.Instance.SelectedGameObject.name);
        ApplicationState.Instance.SelectedGameObject = null;
    }

    IEnumerator MoveToPosition(Vector3 newPosition, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = ApplicationState.Instance.SelectedInteractible.transform.position;
        while (elapsedTime < time)
        {
            if (Vector3.Distance(startingPos, newPosition) < 1f)
            {
                ApplicationState.Instance.SelectedInteractible.transform.position = newPosition;
            }

            ApplicationState.Instance.SelectedInteractible.transform.position = 
                Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
