using UnityEngine;
using UnityEngine.Rendering;

public class SegmentInfo : MonoBehaviour
{
    public int Level;
    public Vector3 StartingPosition = Vector3.zero;
    void Start()
    {
       StartingPosition = transform.position; 
    }

}
