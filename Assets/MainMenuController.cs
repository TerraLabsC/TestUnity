using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelMenuCanvas;

    public void PlayGame()
    {
        levelMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
