using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameManager instance = null;

  public GameOverScreen gameOverScreen;
  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new GameObject("GameManager").AddComponent<GameManager>();
        // DontDestroyOnLoad(instance); awake deals with that, not needed

      }
      return instance;
    }
  }

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    DontDestroyOnLoad(instance); // if we want persistent state
    // NodeManager.AssignNearestNeighbours();
  }

  public void Start()
  {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
  }

  public static void GameOver()
  {
      Instance.gameOverScreen.Setup();
  }

  public static void StopGame()
  {
      UnityEditor.EditorApplication.isPlaying = false;
  }
}
