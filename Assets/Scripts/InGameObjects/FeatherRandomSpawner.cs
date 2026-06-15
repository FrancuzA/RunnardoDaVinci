using UnityEngine;

public class FeatherRandomSpawner : MonoBehaviour
{
    public GameObject FeatherObject;
    private void Start()
    {
        FeatherObject.SetActive(false);
      int i = RNG_Custom.NextInt(0, 4);
      if(i == 1) FeatherObject.SetActive(true);
    }
}
