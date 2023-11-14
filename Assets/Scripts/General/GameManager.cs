using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameOver()
    {
        gameOverScreen.Setup();
    }

    public static void StopGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
