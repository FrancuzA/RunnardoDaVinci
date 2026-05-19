using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    private Dependencies _dep;
    private Player _player;
    private GameObject _playerObject;

    private void Start()
    {
        _dep = Dependencies.Instance;
        _player = _dep.GetDependancy<Player>();
        _playerObject = _player.gameObject;
        }

    // Update is called once per frame
    void Update()
    {
        var newPosition =new Vector3( _playerObject.transform.position.x,transform.position.y, -1);
        gameObject.transform.SetPositionAndRotation(newPosition,Quaternion.identity);
    }
}
