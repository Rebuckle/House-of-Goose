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

    private bool firstCallFinished = false;

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
        //Camera look
        Vector2 mousePos = MousePosFromCenter();
        targetLookAngle = new Vector2 (mousePos.x * maxSideLook, mousePos.y * maxVerticalLook);
        transform.eulerAngles = new Vector3(-targetLookAngle.y, targetLookAngle.x, 0);


        //Mouse click
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
            transform.DOMove(cameraAnchorChair.position, 0.5f);
            
        }
        else
        {
            transform.DOMove(cameraAnchorComputer.position, 0.5f);
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

    void SetPhoneZoom(bool value)
    {
        if (!value)
        {
            phoneReciever.DOMove(phoneAnchorBase.position, 0.5f);
            phoneReciever.DORotate(phoneAnchorBase.eulerAngles, 0.5f);
        }
        else
        {
            phoneReciever.DOMove(phoneAnchorCall.position, 0.5f);
            phoneReciever.DORotate(phoneAnchorCall.eulerAngles, 0.5f);
        }
    }
    void SetNotebookZoom(bool value)
    {
        if (!value)
        {
            notebook.DOMove(notebookAnchorDesk.position, 0.5f);
            notebook.DORotate(notebookAnchorDesk.eulerAngles, 0.5f);
        }
        else 
        {
            notebook.DOMove(notebookAnchorCall.position, 0.5f);
            notebook.DORotate(notebookAnchorCall.eulerAngles, 0.5f);

        }
        notebookZoom = value;
    }

    void ToggleNotebookZoom()
    {
        if (!notebookZoom)
        {
            notebook.DOMove(notebookAnchorCall.position, 0.5f);
            notebook.DORotate(notebookAnchorCall.eulerAngles, 0.5f);
        }
        else 
        {
            notebook.DOMove(notebookAnchorDesk.position, 0.5f);
            notebook.DORotate(notebookAnchorDesk.eulerAngles, 0.5f);

        }
        notebookZoom = !notebookZoom;
    }

    
    void EndPhoneCallCallback()
    {
        inPhoneCall = false;
        SetPhoneZoom(false);
        SetNotebookZoom(false);
        DialogueManager.OnDialogueEnded -= EndPhoneCallCallback;
    }
    void Interact()
    {
        if (inPhoneCall)
        {
            // Clicks on the notebook interact with it, otherwise dialogue skips ahead
            if (Physics.Raycast(Camera.main.ScreenPointToRay (Input.mousePosition), out RaycastHit hit))
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay (Input.mousePosition), out RaycastHit hit))
            {
                Debug.Log("Hit " + hit.collider.name);
                if (hit.collider.transform == phoneBase || hit.collider.transform == phoneReciever)
                {
                    SetNotebookZoom(true);
                    //IF CLIENT WAITING, PICK UP THE PHONE
                    if (!firstCallFinished)
                    {
                        SetPhoneZoom(true);
                        Debug.Log ("Entering main dialogue with " + GameManager.gm.currentClient.name + "...");
                        DialogueManager.instance.StartDialogue(GameManager.gm.currentClient.MainDialogue);
                        inPhoneCall = true;
                        firstCallFinished = true;
                        DialogueManager.OnDialogueEnded += EndPhoneCallCallback;
                    }
                    //ELSE CALL CLIENT BACK
                    else
                    {
                        SetPhoneZoom(true);
                        Debug.Log ("Entering redial dialogue with " + GameManager.gm.currentClient.name + "...");
                        DialogueManager.instance.StartDialogue(GameManager.gm.currentClient.RepeatDialogue);
                        inPhoneCall = true;
                        DialogueManager.OnDialogueEnded += EndPhoneCallCallback;

                    }
                }
                else if (hit.collider.transform == notebook)
                {
                    SetNotebookZoom(true);
                    SetComputerZoom(false);
                    SetManillaZoom(false);
                }
                else if (hit.collider.transform == manilla)
                {
                    ToggleManillaZoom();
                    SetComputerZoom(false);
                    SetNotebookZoom(false);
                }
                else if (hit.collider.transform == computer)
                {
                    SetComputerZoom(true);
                    SetManillaZoom(false);
                    SetNotebookZoom(false);
                }

            }
            else
            {
                SetComputerZoom(false);
                SetManillaZoom(false);
                SetNotebookZoom(false);
            }
        }
    }


    Vector2 MousePosFromCenter()
    {
        return new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f) * 2;

    }
}
