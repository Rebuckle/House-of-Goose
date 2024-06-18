using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraAnchorChair;
    public Transform cameraAnchorComputer;

    private bool zoomed = false;
    public float maxSideLook = 30f;
    public float maxVerticalLook = 10f;

    private Vector2 targetLookAngle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = MousePosFromCenter();
        targetLookAngle = new Vector2 (mousePos.x * maxSideLook, mousePos.y * maxVerticalLook);
        transform.eulerAngles = new Vector3(-targetLookAngle.y, targetLookAngle.x, 0);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            ToggleScreenZoom();
        }
    }

    public void ToggleScreenZoom()
    {
        if (!zoomed)
        {
            transform.DOMove(cameraAnchorComputer.position, 0.5f);
            
        } else
        {
            transform.DOMove(cameraAnchorChair.position, 0.5f);
        }
        zoomed = !zoomed;
    }

    private Vector2 MousePosFromCenter()
    {
        return new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f) * 2;

    }
}
