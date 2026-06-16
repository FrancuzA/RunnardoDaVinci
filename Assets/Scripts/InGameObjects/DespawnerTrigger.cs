using UnityEngine;
using UnityEngine.Events;

public class DespawnerTrigger : MonoBehaviour
{
    public UnityEvent OnDespawn;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Despawn"))
        {
            OnDespawn.Invoke();
        }
    }
}
