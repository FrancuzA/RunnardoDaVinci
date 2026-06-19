using Unity.VisualScripting;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Vector3 movementScale = Vector3.one;

    private Transform _camera;
    private float _lastX;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Scale(_camera.position, movementScale);
        float deltaX = transform.position.x - _lastX;
        Debug.Log($"[Parallax:{gameObject.name}] x={transform.position.x}, deltaX={deltaX}");
        _lastX = transform.position.x;
    }
}
