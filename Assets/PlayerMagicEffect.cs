using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerMagicEffect : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem magicParticles;
    [SerializeField] private ParticleSystem[] fireworks;

    [Header("References")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Collider playerCollider;

    [Header("Settings")]
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private string finishTag = "Finish";

    private Rigidbody rb;
    private bool isGrounded;
    private bool isInMagic = false;
    private bool hasFinished = false;

    // Компоненты новой системы ввода
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        // Инициализация Input System
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        // Получаем компоненты
        rb = GetComponent<Rigidbody>();
        magicParticles = GetComponentInChildren<ParticleSystem>();

        // Убедимся, что эффекты изначально отключены
        if (magicParticles != null)
            magicParticles.gameObject.SetActive(false);

        foreach (var firework in fireworks)
        {
            if (firework != null)
                firework.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Управление игроком (только если не в магическом эффекте и не закончил игру)
        if (!isInMagic && !hasFinished)
        {
            HandlePlayerMovement();
        }
    }

    void HandlePlayerMovement()
    {
        // Получаем вектор движения из Input System
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magic") && !isInMagic)
        {
            TriggerMagicEffect();
        }
        else if (other.CompareTag(finishTag) && !hasFinished)
        {
            TriggerFinishEffect();
        }
    	else if (other.CompareTag("Portal") && !hasFinished && !isInMagic)
    	{
            TriggerPortalEffect();
    	}
    }

    public void TriggerMagicEffect()
    {
        Debug.Log("Игрок в магии");
        isInMagic = true;

        // Отключаем визуализацию игрока
        if (playerRenderer != null)
            playerRenderer.enabled = false;

        // Активируем и запускаем систему частиц магии
        if (magicParticles != null)
        {
            magicParticles.gameObject.SetActive(true);
            magicParticles.Play();

            // Запускаем таймер удаления
            Invoke(nameof(DestroyPlayer), destroyDelay);
        }
    }

    void TriggerPortalEffect()
    {
    	Debug.Log("Портал активирован! Переход на следующий уровень...");

   	 // Отключаем дальнейшее срабатывание
    	hasFinished = true;

    	// Визуальная обратная связь (опционально)
   	 StartCoroutine(ShowPortalTransitionEffect());

    	// Запускаем переход с задержкой
    	Invoke(nameof(LoadNextLevel), 1f);
    }

    System.Collections.IEnumerator ShowPortalTransitionEffect()
    {
    	// Здесь можно добавить:
    	// - активацию частиц портала
    	// - изменение цвета экрана
    	// - воспроизведение звука
   	yield return new WaitForSeconds(1f); // длительность эффекта
    }

    void LoadNextLevel()
    {
    	int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    	int totalScenes = SceneManager.sceneCountInBuildSettings;

   	if (currentSceneIndex + 1 < totalScenes)
    	{
        	SceneManager.LoadScene(currentSceneIndex + 1);
    	}
    	else
    	{
        	Debug.LogWarning("Это последняя сцена! Нет следующего уровня.");
        	// Альтернатива: переход на главное меню
        	SceneManager.LoadScene(0);
    	}
    }

    void TriggerFinishEffect()
    {
        Debug.Log("Финиш! Показ фейерверков!");
        hasFinished = true;

        // Отключаем управление и физику
        enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Запускаем все фейерверки
        foreach (var firework in fireworks)
        {
            if (firework != null)
            {
                firework.gameObject.SetActive(true);
                firework.Play();
            }
        }

        ShowWinMessage();
    }

    void ShowWinMessage()
    {
        Debug.Log("<color=green>Поздравляем! Вы прошли игру!</color>");
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}