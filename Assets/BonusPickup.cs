using UnityEngine;
using UnityEngine.UI;

public class BonusPickup : MonoBehaviour
{
    [Header("Настройки бонуса")]
    public ParticleSystem particleSystem; // Ссылка на систему частиц
    public AudioClip pickupSound; // Звук подбора (опционально)
    public float destroyDelay = 2f; // Задержка удаления после воспроизведения

    void Start()
    {
        // Получаем компонент ParticleSystem у дочернего объекта
        particleSystem = GetComponentInChildren<ParticleSystem>();

        // Убедимся, что система частиц изначально отключена
        if (particleSystem != null)
            particleSystem.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupBonus();
        }
    }

    void PickupBonus()
    {
        Debug.Log("Бонус подобран!");

        // Активируем и запускаем систему частиц
        if (particleSystem != null)
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();

            // Запускаем таймер удаления
            Invoke(nameof(DestroyBonus), destroyDelay);
        }

        // Отключаем коллайдер бонуса, чтобы избежать повторного срабатывания
        GetComponent<Collider>().enabled = false;

    }

    void DestroyBonus()
    {
        // Удаляем весь объект бонуса после завершения анимации частиц
        Destroy(gameObject);
    }
}
