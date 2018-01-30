// Copyright Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public delegate void SceneLoaded(GameObject content);

public class ViewLoader : Singleton<ViewLoader>
{
    public string StartingView;

    private string CoreSystems = "CoreSystems";
    private bool coreSystemsLoaded = false;

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadCoreSystemsAsync());
        //ToolManager.Instance.HideTools();
    }

    private IEnumerator LoadCoreSystemsAsync()
    {
        yield return null;

        GameObject coreSystems = GameObject.Find(CoreSystems);

        if (coreSystems == null)
        {
            var coreSystemsOp = SceneManager.LoadSceneAsync(CoreSystems, LoadSceneMode.Additive);
            
            while (!coreSystemsOp.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        if (!coreSystemsLoaded)
        {
            coreSystemsLoaded = true;
        }
    }

    public void LoadStartingView()
    {
        LoadViewAsync(StartingView);
    }

    public void LoadViewAsync(string sceneName, SceneLoaded sceneLoadedCallback = null)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("ViewLoader: no scene name specified when calling LoadViewAsync() - cannot load the scene");
            return;
        }

        StartCoroutine(LoadViewAsyncInternal(sceneName, sceneLoadedCallback));
    }

    public IEnumerator LoadViewAsyncInternal(string viewName, SceneLoaded sceneLoadedCallback)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(viewName, LoadSceneMode.Additive);

        if (loadOperation == null)
        {
            throw new InvalidOperationException(string.Format("ViewLoader: Unable to load {0}. Make sure that the scene is enabled in the Build Settings.", viewName));
        }

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("ViewLoader: Loaded" + viewName);

        GameObject newContent = GameObject.Find("Content");

        if (newContent)
        {
            if (viewName != StartingView)
            {
                GameObject startingViewContent = GameObject.Find(StartingView + "Content");

                Bounds bounds = GetBoundsForAllChildren(startingViewContent);

                newContent.name = viewName + "Content";
                newContent.transform.position = startingViewContent.transform.position + new Vector3(0, bounds.size.y * 2f, 0);
                newContent.transform.rotation = Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0);
                newContent.transform.SetParent(gameObject.transform, true);
            }
            else
            {
                newContent.name = viewName + "Content";
                newContent.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.5f);
                newContent.transform.rotation = Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0);
                newContent.transform.SetParent(gameObject.transform, true);
            }
        }

        if (sceneLoadedCallback != null)
        {
            sceneLoadedCallback(newContent);
        }
    }

    public void UnloadView(string viewName)
    {
        SceneManager.UnloadSceneAsync(viewName);
    }

    private Bounds GetBoundsForAllChildren(GameObject findMyBounds)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);

        foreach (var curRenderer in findMyBounds.GetComponentsInChildren<Renderer>())
        {
            if (result.extents == Vector3.zero)
            {
                result = curRenderer.bounds;
            }
            else
            {
                result.Encapsulate(curRenderer.bounds);
            }
        }

        return result;
    }
}