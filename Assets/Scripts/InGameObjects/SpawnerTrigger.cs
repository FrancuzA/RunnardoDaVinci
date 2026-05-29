using UnityEngine;
using UnityEngine.Events;

public class SpawnerTrigger : MonoBehaviour
{
    public UnityEvent OnSpawn;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"entered {other}");
        if (other.CompareTag("Spawn"))
        {
            OnSpawn.Invoke();
            this.enabled = false;
        }
    }
}
