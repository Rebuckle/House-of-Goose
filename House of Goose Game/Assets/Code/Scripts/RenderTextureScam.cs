using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTextureScam : MonoBehaviour
{
    private RenderTexture target;
    private RenderTexture temporaryRT;
    private Camera mainCamera;

    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = GetComponent<Camera>();
                if (mainCamera == null)
                {
                    Debug.LogError("No Camera component found. Do better.");
                    return null;
                }
            }
            return mainCamera;
        }
    }


    private void OnPreRender()
    {
        temporaryRT = RenderTexture.GetTemporary(Screen.width, Screen.height);
        mainCamera.targetTexture = temporaryRT;
    }

    private void OnPostRender()
    {
        Graphics.Blit(temporaryRT, target);
        mainCamera.targetTexture = null;

        RenderTexture.ReleaseTemporary(temporaryRT);
    }
}