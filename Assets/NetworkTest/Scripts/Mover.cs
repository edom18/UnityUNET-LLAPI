using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 0.05f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position -= Vector3.left * 0.05f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * 0.05f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= Vector3.up * 0.05f;
        }
    }
}
