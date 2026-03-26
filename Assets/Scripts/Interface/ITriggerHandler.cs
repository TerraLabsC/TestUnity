using UnityEngine;

public interface ITriggerHandler
{
    bool CanHandle(Collider other);
    void Handle(Collider other);
}