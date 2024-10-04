using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Background : MonoBehaviour
{
    public PlayerController playerController;

    float backGroundspeed = 2.0f;
    void Update()
    {
        BackGroundMove();
    }

    public void BackGroundMove()
    {
        if (Input.GetKey(KeyCode.A) && !playerController.dieState)
        {
            transform.Translate(Vector2.right * backGroundspeed * Time.smoothDeltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && !playerController.dieState)
        {
            transform.Translate(Vector2.left * backGroundspeed * Time.smoothDeltaTime);
        }
    }

}