using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float ttl = 1f;
    public int damage = 20;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        Destroy(this.gameObject, ttl);
    }
}
