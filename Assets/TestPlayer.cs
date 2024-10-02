using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector2.left * 10f * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector2.down * 10f * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector2.right * 10f * Time.deltaTime);
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector2.up * 10f * Time.deltaTime);
    }
}
