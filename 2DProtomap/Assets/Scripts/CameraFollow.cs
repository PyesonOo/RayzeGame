using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; //Koordinanten des Player
    [SerializeField] float timeoffset;
    [SerializeField] Vector3 offsetPos;

    [SerializeField] Vector3 boundsMin;
    [SerializeField] Vector3 boundsMax;
    private void LateUpdate()
    {
        if(player != null)
        {
            Vector3 startPos = transform.position;  //Startposition der Kamera
            Vector3 targetPos = player.position;

            targetPos.x += offsetPos.x;
            targetPos.y += offsetPos.y;
            targetPos.z = transform.position.z;     //Einfrieren der z Achse

            targetPos.x = Mathf.Clamp(targetPos.x, boundsMin.x, boundsMax.x);   // Festhalten der Kamera der X und Y Grenzen
            targetPos.y = Mathf.Clamp(targetPos.y, boundsMin.y, boundsMax.y);

            float t = 1f - Mathf.Pow(1f - timeoffset,Time.deltaTime * 30);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
        }

    }
}
