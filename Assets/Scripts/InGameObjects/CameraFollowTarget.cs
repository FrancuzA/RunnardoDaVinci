using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform player;
    public float fixedY;
    public float xOffset = 5f;

    void LateUpdate()
    {
        if (player == null) return;
        transform.position = new Vector3(player.position.x + xOffset, fixedY, 0);
    }
}