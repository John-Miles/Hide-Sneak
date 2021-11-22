using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")] [Tooltip("The minimum angle the camera is able to rotate along the X axis")]
    public float minX = -70f;

    [Tooltip("The maximum angle the camera is able to rotate around the X axis")]
    public float maxX = 70f;

    float sensitivityX;
    float sensitivityY;

    public Camera cam;
    public AudioListener audio;
    [SyncVar] float rotY = 0f;
    [SyncVar] float rotX = 0f;
    GameManager gm;
    private bool inGame;
    public bool isPaused = false;

    public GameObject pauseMenu;

    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        enabled = true;
        cam.enabled = true;
        audio.enabled = true;
        GetSens();
        inGame = true;
    }

    public void GetSens()
    {
        sensitivityX = PlayerPrefs.GetFloat("MouseSensX", 5);
        sensitivityY = PlayerPrefs.GetFloat("MouseSensY", 5);
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

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            PauseGame();
        }

        else
        {
            ResumeGame();
        }
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inGame = false;
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGame = true;
    }
}