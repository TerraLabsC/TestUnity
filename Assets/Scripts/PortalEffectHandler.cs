using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEffectHandler : TriggerHandler
{
    [Header("Portal Effect Settings")]
    [SerializeField] private float transitionDelay = 1f;

    protected override bool CheckTag(Collider other) => other.CompareTag("Portal");

    protected override void OnHandle(Collider other)
    {
        Debug.Log("Портал активирован! Переход на следующий уровень...");

        var player = GetComponent<PlayerMagicEffects>();
        if (player != null) player.enabled = false;

        StartCoroutine(Transition());
    }

    private System.Collections.IEnumerator Transition()
    {
        yield return new WaitForSeconds(transitionDelay);
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
            SceneManager.LoadScene(currentSceneIndex + 1);
        else
            SceneManager.LoadScene(0);
    }
}