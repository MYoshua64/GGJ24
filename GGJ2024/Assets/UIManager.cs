using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadIntoGame()
    {
        SceneManager.LoadScene("ezra Sceane 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
