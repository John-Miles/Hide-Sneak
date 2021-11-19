using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {        
      NetworkClient.Shutdown();
      SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
