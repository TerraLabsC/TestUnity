using UnityEngine;

public abstract class TriggerHandler : MonoBehaviour, ITriggerHandler
{
    protected bool isProcessing = false;

    public virtual bool CanHandle(Collider other)
    {
        return !isProcessing && CheckTag(other);
    }

    protected abstract bool CheckTag(Collider other);

    public void Handle(Collider other)
    {
        if (isProcessing) return;
        isProcessing = true;
        OnHandle(other);
    }

    protected abstract void OnHandle(Collider other);
}