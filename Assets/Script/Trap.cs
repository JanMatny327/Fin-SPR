using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public static Trap Instance;
    private float timer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public void TrapHit(float taget)
    {
        if(timer >= taget)
        {
            PlayerController.Instance.gameData.hp -= 10;
            timer = 0f;
        }
    }
}
