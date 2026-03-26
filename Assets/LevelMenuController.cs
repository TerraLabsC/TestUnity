using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas;

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void BackToMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
