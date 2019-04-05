using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector2 maxXandY, minXandY;

    private CameraFollow camerafollow;
    void Start()
    {
        camerafollow = FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            camerafollow.maxXAndY = maxXandY;
            camerafollow.minXAndY = minXandY;
        }
    }
    
}
