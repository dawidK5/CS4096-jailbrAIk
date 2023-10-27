using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{ // Singleton for managing game
// private static GameObject gameManagerObj;
// private static GameManager instance;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }


    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    // public void Awake()
    // {
    //   gameManagerObj = GameObject.FindGameObjectWithTag("GameMgr");
    //   instance = gameManagerObj.GetComponent<GameManager>();
    // }
    // public static GameManager Instance()
    // {
    //   if (instance == null)
    //   {
    //     gameManagerObj = GameObject.FindGameObjectWithTag("GameMgr");
    //     instance = gameManagerObj.GetComponent<GameManager>();
    //   }
    //   return instance;
    // }

    // public void RunCoroutine(IEnumerator coroutine)
    // {
    //   Debug.Log(coroutine.ToString());
    //   StartCoroutine(coroutine);
    // }

    // public void EndCoroutine(IEnumerator coroutine)
    // {
    //   StopCoroutine(coroutine);
    // }

    public static void StopGame()
    {
      UnityEditor.EditorApplication.isPlaying = false;
    }
}
