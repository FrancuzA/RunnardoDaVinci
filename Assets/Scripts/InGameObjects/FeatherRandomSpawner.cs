using UnityEngine;

public class FeatherRandomSpawner : MonoBehaviour
{
    public GameObject FeatherObject;
    private void Start()
    {
        RNG_Custom.Init();
        FeatherObject.SetActive(false);
      int i = RNG_Custom.NextInt(0, 4);
        Debug.Log(i);
      if(i == 1) FeatherObject.SetActive(true);
    }
}
