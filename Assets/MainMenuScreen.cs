using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public void ExitButton() {
        Debug.Log("Exit called");
        Application.Quit();
    }

    public void StartButton() {
        Debug.Log("Start called");
        SceneManager.LoadScene("InitialScene");
    }
}
