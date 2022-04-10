using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametrableBulletBehavior : IParametrableBullet
{
    void Update()
    {
        if (IsOutOfBounds())
        {
            gameObject.SetActive(false);
        }
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private bool IsOutOfBounds()
    {
        return (transform.position.x < -5.5 ||
            transform.position.x > 5.5 ||
            transform.position.y < -6 ||
            transform.position.y > 6);
    }
}

