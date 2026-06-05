using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    public List<GameObject> segmentsL1 = new();
    public List<GameObject> segmentsL2 = new ();
    public List<GameObject> segmentsL3 = new();
    public float SpawnLenght;
    public float DifficultyIncreasTime;
    public float StartSpawnPlace;

    private float CurrentSpawnPlace;
    private List<GameObject> spawnedSegments = new();
    public int currentLevel;
    private WaitForSecondsRealtime DifficultyIncreasTimer;

    private void Start()
    {
        CurrentSpawnPlace = StartSpawnPlace;
        currentLevel = 0;
        DifficultyIncreasTimer = new WaitForSecondsRealtime(DifficultyIncreasTime);
        StartCoroutine(IncreasDifficulty());
        RNG_Custom.Init(-1);
        SpawnNewSegment();  
    }

    public void SpawnNewSegment()
    {
        GameObject newSegment = null;


        switch (currentLevel)
        {
            case 0:  
                newSegment = GetNextSegment(segmentsL1);
                segmentsL1.Remove(newSegment);
                break;
            case 1:  
                newSegment = GetNextSegment(segmentsL2);
                segmentsL2.Remove(newSegment);
                break;
            case 2:  
                newSegment = GetNextSegment(segmentsL3);
                segmentsL3.Remove(newSegment);
                break;
            default:
                newSegment = GetNextSegment(segmentsL3);
                segmentsL3.Remove(newSegment); 
                break;
        }
        if (newSegment == null) return;
        newSegment.transform.SetPositionAndRotation(new Vector3(CurrentSpawnPlace, 0), Quaternion.identity);
        spawnedSegments.Add(newSegment);
        CurrentSpawnPlace += SpawnLenght;
    }

    public GameObject GetNextSegment(List<GameObject> levelList)
    {
        if (levelList.Count == 0)
        {
            currentLevel--;
            SpawnNewSegment();
            return null;
        }
        var rNumber = RNG_Custom.NextInt(0, levelList.Count - 1);
        return levelList[rNumber];
    }

    public void DespawnSegment()
    {
       var despawnSegment = spawnedSegments[0];
       despawnSegment.transform.SetPositionAndRotation(despawnSegment.GetComponent<SegmentInfo>().StartingPosition, Quaternion.identity);
        switch (despawnSegment.GetComponent<SegmentInfo>().Level)
        {
            case 0: segmentsL1.Add(despawnSegment);
                break;
            case 1:
                segmentsL2.Add(despawnSegment);
                break;
            case 2:
                segmentsL3.Add(despawnSegment);
                break;
            default: segmentsL1.Add(despawnSegment);
                break;
        }
       spawnedSegments.Remove(despawnSegment);
    }

    public IEnumerator IncreasDifficulty()
    {
        yield return DifficultyIncreasTimer;
        currentLevel = 1;
        yield return DifficultyIncreasTimer;
        currentLevel = 2;
    }
}
