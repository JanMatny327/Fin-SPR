using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("타겟 위치")]
    public Transform target;

    [Header("카메라 경계")] 
    public Vector2 minCam;
    public Vector2 maxCam;

    public BoxCollider2D boxColl;

    //움직임 속도
    private float smooth = 0.1f;

    //카메라 위치
    private Vector3 _offset;

    private float pointx = 62f;
    private float pointy = 13.52f;
    public bool isBossRoom = false;

    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>(); 
        //카메라 최소 위치
        minCam.x = -15f; minCam.y = -2f;

        //카메라 최대 위치
        maxCam.x = 150f; maxCam.y = 20f;
    }
    private void Update()
    {
        if (target.transform.position.y > pointy) 
        {
            if(target.transform.position.x < pointx)
            {
                transform.position = new Vector3(54.4f, 17f, -1f);
            }
        }
        else
        {
            Fallowcm();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BossGate-1")
        {
            transform.position = new Vector3(54.4f, 17f, -1f);
            isBossRoom = true;
        }
    }

    void Fallowcm()
    {
        if (isBossRoom == false)
        {
            Vector3 Camposition = Vector3.Lerp(transform.position, target.position, smooth);

            //최소와 최대 카메라 위치
            transform.position = new Vector3(
                Mathf.Clamp(Camposition.x, minCam.x, maxCam.x) + _offset.x,
                Mathf.Clamp(Camposition.y, minCam.y, maxCam.y) + _offset.y, -10f + _offset.z);
        }
    }
}
