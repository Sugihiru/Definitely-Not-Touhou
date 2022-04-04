using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametrableBulletBehavior : IParametrableBullet
{
    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
}

