using UnityEditor;
using UnityEngine;
using System;
using DG.Tweening;

public class Player : HoG
{
    private const float TWEEN_SPEED = 0.5f;
    private Vector3 MANILLA_COVER_ANGLE_OPEN = new Vector3(0, 90, -135);
    private Vector3 MANILLA_COVER_ANGLE_CLOSED = new Vector3(0, 90, 0);

    public float maxSideLook = 30f;
    public float maxVerticalLook = 10f;

    bool inPhoneCall = false;

    private SFXHandler sFXHandler;
    public AudioClip phonePickUp_Audio;
    public AudioClip phoneHangUp_Audio;
    public AudioClip notebookPickUp_Audio;
    public AudioClip manilaPickUp_Audio;
    public AudioClip computerRejectQuack_Audio;
    public AudioClip computerClickOn_Audio;
    public AudioClip phoneRing_Audio;
    public AudioClip phoneBusy_Audio;


    private bool computerZoom = false;
    private bool notebookZoom = false;
    private bool manillaZoom = false;
    private Vector2 targetLookAngle;

    private bool firstCallFinished = false;

    public Transform cameraAnchorChair;
    public Transform cameraAnchorComputer;

    public Transform phoneBase;
    public Transform phoneReciever;
    public Transform phoneAnchorBase;
    public Transform phoneAnchorCall;
   
    public Transform notebookAnchorDesk;
    public Transform notebookAnchorCall;
    public Transform notebook;
   
    public Transform manillaOpenRootAnchor;
    public Transform manillaOpenCoverAnchor;
    private Transform[] manillas;
    private Transform[] manillaBaseAnchors;
    private int selectedManillaIndex = -1;

    [HideInInspector] public Transform computer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        DOTween.Init();
        GameManager gm = GameObject.Find("Game Controller").GetComponent<GameManager>();
        gm.OnGameStarted += OnGameStarted;
        sFXHandler = GameObject.Find("sfxManager").GetComponent<SFXHandler>();

        Invoke("CallSpawnDelay", 3); //ring phone

    }

    private void CallSpawnDelay()
    {
        sFXHandler.PlaySound(phoneRing_Audio);
    }

    private void OnGameStarted(object gm, GameStartArgs gsa)
    {
        manillas = gsa.manillas.ToArray();
        CreateManillaAnchors();
    }

    void CreateManillaAnchors()
    {
        if(manillas.Length == 0)
        {
            Debug.Log("No manilla envelopes are loaded into the Player object yet. Wait a frame and try again");
            return;
        }

        manillaBaseAnchors = new Transform[manillas.Length];
        Transform baseAnchor = new GameObject("Manilla Anchors").transform;
        for  (int i = 0; i < manillas.Length; i++)   
        {
            Transform anchor = new GameObject("Anchor_" + manillas[i].name).transform;
            anchor.position = manillas[i].position;
            anchor.rotation = manillas[i].rotation;
            anchor.parent = baseAnchor;
            manillaBaseAnchors[i] = anchor;
        }
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


    void SetComputerZoom(bool value)
    {
        if (!value)
        {
            transform.DOMove(cameraAnchorChair.position, TWEEN_SPEED);
            
        }
        else
        {
            transform.DOMove(cameraAnchorComputer.position, TWEEN_SPEED);
        }
        computerZoom = value;
    }

    void ToggleComputerZoom()
    {
        if (!computerZoom)
        {
            transform.DOMove(cameraAnchorComputer.position, TWEEN_SPEED);

            
        } else
        {
            transform.DOMove(cameraAnchorChair.position, TWEEN_SPEED);
        }
        computerZoom = !computerZoom;
    }

    void SetPhoneZoom(bool value)
    {
        if (!value)
        {
            phoneReciever.DOMove(phoneAnchorBase.position, TWEEN_SPEED);
            phoneReciever.DORotate(phoneAnchorBase.eulerAngles, TWEEN_SPEED);
        }
        else
        {
            phoneReciever.DOMove(phoneAnchorCall.position, TWEEN_SPEED);
            phoneReciever.DORotate(phoneAnchorCall.eulerAngles, TWEEN_SPEED);
        }
    }
    void SetNotebookZoom(bool value)
    {
        if (!value)
        {
            notebook.DOMove(notebookAnchorDesk.position, TWEEN_SPEED);
            notebook.DORotate(notebookAnchorDesk.eulerAngles, TWEEN_SPEED);
        }
        else 
        {
            notebook.DOMove(notebookAnchorCall.position, TWEEN_SPEED);
            notebook.DORotate(notebookAnchorCall.eulerAngles, TWEEN_SPEED);

        }
        notebookZoom = value;
    }

    void ToggleNotebookZoom()
    {
        if (!notebookZoom)
        {
            notebook.DOMove(notebookAnchorCall.position, TWEEN_SPEED);
            notebook.DORotate(notebookAnchorCall.eulerAngles, TWEEN_SPEED);
        }
        else 
        {
            notebook.DOMove(notebookAnchorDesk.position, TWEEN_SPEED);
            notebook.DORotate(notebookAnchorDesk.eulerAngles, TWEEN_SPEED);

        }
        notebookZoom = !notebookZoom;
    }

    void SetManillaZoom(int index, bool value)
    {
        if(!value)
        {
            manillaZoom = false;
            if (selectedManillaIndex != -1)
            {
                manillas[selectedManillaIndex].DOMove(manillaBaseAnchors[selectedManillaIndex].position, TWEEN_SPEED);
                manillas[selectedManillaIndex].DORotate(manillaBaseAnchors[selectedManillaIndex].eulerAngles, TWEEN_SPEED);
                manillas[selectedManillaIndex].Find("Armature").Find("root").Find("folder_front").DOLocalRotate(MANILLA_COVER_ANGLE_CLOSED, TWEEN_SPEED);
                selectedManillaIndex = -1;
            }
            return;
        }
        //Reset old manilla
        if (selectedManillaIndex != -1)
        {
            if (manillas[index] != manillas[selectedManillaIndex])
            {
                manillas[selectedManillaIndex].DOMove(manillaBaseAnchors[selectedManillaIndex].position, TWEEN_SPEED);
                manillas[selectedManillaIndex].DORotate(manillaBaseAnchors[selectedManillaIndex].eulerAngles, TWEEN_SPEED);
                manillas[selectedManillaIndex].Find("Armature").Find("root").Find("folder_front").DOLocalRotate(MANILLA_COVER_ANGLE_CLOSED, TWEEN_SPEED);
            }

        }
        selectedManillaIndex = index;
        manillas[selectedManillaIndex].DOMove(manillaOpenRootAnchor.position, TWEEN_SPEED);
        manillas[selectedManillaIndex].DORotate(manillaOpenRootAnchor.eulerAngles, TWEEN_SPEED);
        manillas[selectedManillaIndex].Find("Armature").Find("root").Find("folder_front").DOLocalRotate(MANILLA_COVER_ANGLE_OPEN, TWEEN_SPEED);

        manillaZoom = true;


        
    }

    private void OnGameStart(object sender, EventArgs e)
    {

    }

    void EndPhoneCallCallback()
    {
        inPhoneCall = false;
        SetPhoneZoom(false);
        SetNotebookZoom(false);
        DialogueManager.OnDialogueEnded -= EndPhoneCallCallback;
        sFXHandler.PlaySound(phoneHangUp_Audio);
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
                    sFXHandler.PlaySound(computerRejectQuack_Audio);
                }
                else if (hit.collider.transform == notebook)
                {
                    
                }
                else
                {
                    DialogueManager.instance.SkipLine();
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
                    SetPhoneZoom(true);
                    inPhoneCall = true;
                    DialogueManager.OnDialogueEnded += EndPhoneCallCallback;

                    sFXHandler.PlaySound(phonePickUp_Audio);

                    //IF CLIENT WAITING, PICK UP THE PHONE
                    if (!firstCallFinished)
                    {
                        Debug.Log ("Entering main dialogue with " + GameManager.gm.currentClient.name + "...");
                        DialogueManager.instance.StartDialogue(GameManager.gm.currentClient.MainDialogue);
                        firstCallFinished = true;
                    }
                    //ELSE CALL CLIENT BACK
                    else
                    {
                        Debug.Log ("Entering redial dialogue with " + GameManager.gm.currentClient.name + "...");
                        DialogueManager.instance.StartDialogue(GameManager.gm.currentClient.RepeatDialogue);
                    }
                }
                else if (hit.collider.transform == notebook)
                {
                    SetNotebookZoom(true);
                    SetComputerZoom(false);
                    SetManillaZoom(-1, false);
                    sFXHandler.PlaySound(notebookPickUp_Audio);
                }

                else if (hit.collider.transform == computer)
                {
                    SetComputerZoom(true);
                    SetNotebookZoom(false);
                    SetManillaZoom(-1, false);
                    sFXHandler.PlaySound(computerClickOn_Audio);
                }
                else
                {
                    bool hitManilla = false;
                    for (int i = 0; i < manillas.Length; i++)
                    {
                        if (hit.collider.transform == manillas[i] || hit.collider.transform.root == manillas[i])
                        {
                            Debug.Log("Zooming to manilla " + i);
                            SetManillaZoom(i, true);
                            SetComputerZoom(false);
                            SetNotebookZoom(false);
                            hitManilla = true;
                            sFXHandler.PlaySound(manilaPickUp_Audio);
                            break;
                        }
                    }
                    if (!hitManilla)
                    {
                        SetComputerZoom(false);
                        SetNotebookZoom(false);
                        SetManillaZoom(-1, false);                        
                    }
                    
                }

            }
            else
            {
                SetComputerZoom(false);
                SetNotebookZoom(false);
                SetManillaZoom(-1, false);
            }
        }
    }


    Vector2 MousePosFromCenter()
    {
        return new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f) * 2;
    }
}
