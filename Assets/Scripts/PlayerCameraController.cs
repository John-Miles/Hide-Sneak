using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [Tooltip("The minimum angle the camera is able to rotate along the X axis")]
    public float minX = -70f;
    [Tooltip("The maxiimum angle the camera is able to rotate around the X axis")]
    public float maxX = 70f;

     float sensitivityX;
     float sensitivityY;

     public Camera cam;
     public AudioListener audio;
    [SyncVar]
    float rotY = 0f;
    [SyncVar]
    float rotX = 0f;
    GameManager gm;
    private bool inGame;
    public bool active = false;

    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        enabled = true;
        cam.enabled = true;
        audio.enabled = true;
        sensitivityX = PlayerPrefs.GetFloat("MouseSensX", 5);
        sensitivityY = PlayerPrefs.GetFloat("MouseSensY", 5);
        inGame = true;
    }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        
    }

    private void Start()
    {
        CmdRepeatActiveCheck();
    }

    [Command(requiresAuthority = false)]
    public void CmdRepeatActiveCheck()
    {
        RpcRequestActiveCheck();
    }
    [ClientRpc]
    public void RpcRequestActiveCheck()
    {
        gm.CmdRefreshList();
        gm.CmdAddToActive(gameObject);
        gm.CmdUpdatePlayerListAndCheck();
        
    }
    void Update()
    {
        if (inGame)
        {
            rotY += Input.GetAxis("Mouse X") * sensitivityX;
            rotX += Input.GetAxis("Mouse Y") * sensitivityY;

            rotX = Mathf.Clamp(rotX, minX, maxX);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);

        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Mistake happened here 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inGame = false;
            //bring up menu call here
        }

        if (Cursor.visible && Input.GetMouseButtonDown(1))
        {
            //rename this function to be called on resume button click event
            //hide pause menu UI
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inGame = true;
        }
    }
}
