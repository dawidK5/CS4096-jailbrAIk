using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour
{
    public void BeginGameButton() {
        Debug.Log("Start called");
        SceneManager.LoadScene("InitialScene");
    }
}
