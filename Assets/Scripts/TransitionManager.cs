using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class TransitionManager : Singleton<TransitionManager>
{
    Color lerpedColor;
    float timeToFade = 2f;

    public void LoadView(string sceneName)
    {
        ViewLoader.Instance.LoadViewAsync(sceneName, SceneLoaded);
    }

    public void CloseView(string sceneName)
    {
        GameObject content = GameObject.Find(sceneName + "Content");

        StartCoroutine(SceneUnloadedCoroutine(content));

        ViewLoader.Instance.UnloadView(sceneName);
    }

    private void SceneLoaded(GameObject content)
    {
        StartCoroutine(SceneLoadedCoroutine(content));
    }

    private IEnumerator SceneLoadedCoroutine(GameObject content)
    {
        Renderer[] renderers = content.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.name != "Glass")
            {
                foreach (Material material in renderer.materials)
                {
                    material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
                    material.SetFloat("_ZWrite", 1);
                    StartCoroutine(FadeIn(material));
                }
            }
        }

        yield return null;
    }

    private IEnumerator SceneUnloadedCoroutine(GameObject content)
    {
        Renderer[] renderers = content.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.name != "Glass")
            {
                foreach (Material material in renderer.materials)
                {
                    StartCoroutine(FadeOut(material, content));
                }
            }
        }

        yield return null;
    }

    IEnumerator FadeIn(Material material)
    {
        float time = 0;
        while (time < timeToFade)
        {
            Color c = material.color;
            c.a = time / timeToFade;
            material.color = c;
            time += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator FadeOut(Material material, GameObject content)
    {
        float time = 0;
        while (time < timeToFade)
        {
            Color c = material.color;
            c.a = 1 - (time / timeToFade);
            material.color = c;
            time += Time.deltaTime;

            yield return null;
        }
        
        Destroy(content);
    }
}
