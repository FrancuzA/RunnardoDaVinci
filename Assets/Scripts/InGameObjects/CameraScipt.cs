using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    private Dependencies _dep;
    private Player _player;
    private GameObject _playerObject;

    public float YOffset;

    private void Start()
    {
        _dep = Dependencies.Instance;
    }

    void Update()
    {
        if (_playerObject == null)
        {
            _player = _dep.GetDependancy<Player>();
            if (_player != null)
                _playerObject = _player.gameObject;
            else
                return;
        }

        var newPosition = new Vector3(_playerObject.transform.position.x+5, YOffset , -1);
        gameObject.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
    }
}