using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuShootAssist : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime), Space.World);
    }
}
