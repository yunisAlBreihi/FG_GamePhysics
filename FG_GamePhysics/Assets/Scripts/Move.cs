using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += Physics.gravity * Time.deltaTime;
    }
}
