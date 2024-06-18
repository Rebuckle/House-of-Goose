using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Player : HoG
{
    
    bool inPhoneCall = false;
    public Transform cameraAnchorChair;
    public Transform cameraAnchorComputer;

    private bool computerZoom = false;

    public Transform notebookAnchorDesk;
    public Transform notebookAnchorCall;
    private bool notebookZoom = false;
    private bool manillaZoom = false;
    public float maxSideLook = 30f;
    public float maxVerticalLook = 10f;
    private Vector2 targetLookAngle;



    public Transform phoneBase;
    public Transform phoneReciever;

    public Transform phoneAnchorBase;
    public Transform phoneAnchorCall;
    public Transform notebook;
    public Transform manilla;
    public Transform computer;
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

        if (Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    void SetManillaZoom(bool value)
    {

    }
    void ToggleManillaZoom()
    {
        if (!manillaZoom)
        {
            
        }
        else
        {

        }
    }

    void SetComputerZoom(bool value)
    {
        if (!value)
        {
            transform.DOMove(cameraAnchorComputer.position, 0.5f);
            
        }
        else
        {
            transform.DOMove(cameraAnchorChair.position, 0.5f);
        }
        computerZoom = value;
    }

    void ToggleComputerZoom()
    {
        if (!computerZoom)
        {
            transform.DOMove(cameraAnchorComputer.position, 0.5f);
            
        } else
        {
            transform.DOMove(cameraAnchorChair.position, 0.5f);
        }
        computerZoom = !computerZoom;
    }

    void SetNotebookZoom(bool value)
    {

    }

    void ToggleNotebookZoom()
    {
        if (!notebookZoom)
        {

        }
    }

    
    void Interact()
    {
        if (inPhoneCall)
        {
            // Clicks on the notebook interact with it, otherwise dialogue skips ahead
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit))
            {
                if (hit.collider.transform == computer)
                {
                    //Play computer quack sound
                }
                else if (hit.collider.transform == notebook)
                {
                    
                }
            }
            else
            {
                
                DialogueManager.instance.SkipLine();
            }
        }
        else
        {
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit))
            {
                if (hit.collider.transform == phoneBase || hit.collider.transform == phoneReciever)
                {
                    //IF CLIENT WAITING, PICK UP THE PHONE
                    //ELSE CALL CLIENT BACK
                }
                else if (hit.collider.transform == notebook)
                {
                    ToggleNotebookZoom();
                    SetComputerZoom(false);
                    SetNotebookZoom(false);
                }
                else if (hit.collider.transform == manilla)
                {
                    ToggleManillaZoom();
                    SetComputerZoom(false);
                    SetNotebookZoom(false);
                }
                else if (hit.collider.transform == computer)
                {
                    ToggleComputerZoom();
                    SetManillaZoom(false);
                    SetNotebookZoom(false);

                }
            }
        }
    }


    Vector2 MousePosFromCenter()
    {
        return new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f) * 2;

    }
}
