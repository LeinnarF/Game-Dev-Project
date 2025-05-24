using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static Vector3 spawnPosition;

    void Awake()
    {
        spawnPosition = transform.position;
    }
}