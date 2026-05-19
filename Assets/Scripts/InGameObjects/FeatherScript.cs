using UnityEngine;

public class FeatherScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Dependencies.Instance.GetDependancy<PointsManager>()?.StartMultip();
            gameObject.SetActive(false);
        }
        
    }
}
