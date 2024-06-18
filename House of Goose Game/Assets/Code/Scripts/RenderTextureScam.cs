using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTextureScam : MonoBehaviour
{
    private RenderTexture target;
    private RenderTexture temporaryRT;
    private Camera cam;

    private Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
                if (cam == null)
                {
                    Debug.LogError("No Camera component found. Do better.");
                    return null;
                }
            }
            return cam;
        }
    }


    private void OnPreRender()
    {
        temporaryRT = RenderTexture.GetTemporary(Screen.width, Screen.height);
        Cam.targetTexture = temporaryRT;
    }

    private void OnPostRender()
    {
        Graphics.Blit(temporaryRT, target);
        Cam.targetTexture = null;

        RenderTexture.ReleaseTemporary(temporaryRT);
    }
}