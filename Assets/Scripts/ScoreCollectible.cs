using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollectible : MonoBehaviour
{
    public int scoreValue;
    public float fallSpeed;

    private void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            GameManager.instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
