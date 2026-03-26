using UnityEngine;


public class FinishEffectHandler : TriggerHandler
{
    [Header("Finish Effect Settings")]
    [SerializeField] private ParticleSystem[] fireworks;
    [SerializeField] private Rigidbody rb;

    protected override bool CheckTag(Collider other) => other.CompareTag("Finish");

    protected override void OnHandle(Collider other)
    {
        Debug.Log("Финиш! Показ фейерверков!");

        var player = GetComponent<PlayerMagicEffects>();
        if (player != null) player.enabled = false;
        if (rb != null) rb.isKinematic = true;

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

    private void ShowWinMessage()
    {
        Debug.Log("<color=green>Поздравляем! Вы прошли игру!</color>");
    }
}