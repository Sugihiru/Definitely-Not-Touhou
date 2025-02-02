using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject hitBox;
    private Rigidbody2D rb2D;
    public SpriteRenderer playerSprite;
    public AudioSource audioSourceHit;

    public float speed;
    public float focusModifier;
    public float maxHealth = 3f;

    private float activeModifier = 1;
    private float health;
    private bool isInvincible = false;
    private PlayerFire playerFire;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerFire = GetComponent<PlayerFire>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Focus") == 1)
        {
            hitBox.SetActive(true);
            activeModifier = focusModifier;
        }
        else
        {
            hitBox.SetActive(false);
            activeModifier = 1;
        }
    }

    public void Respawn()
    {
        health = maxHealth;
        MakeInvincible();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * activeModifier * Time.deltaTime;
        rb2D.MovePosition(rb2D.position + velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && (other.tag == "enemyBullet" || other.tag == "enemy"))
        {
            audioSourceHit.Play();
            health--;
            if (health <= 0)
            {
                GameManager.instance.GameOver();
            }
            else
            {
                MakeInvincible();
            }
            if (other.tag == "enemyBullet")
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    // Make invincible for a short amount of time, with a blinking animation
    private void MakeInvincible()
    {
        isInvincible = true;
        StartCoroutine("BlinkPlayerSprite");
        Invoke("StopInvincible", 1.5f);
    }


    //Coroutine
    private IEnumerator BlinkPlayerSprite()
    {
        const float blinkSpd = 0.05f;
        while (isInvincible)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(blinkSpd);
        }
        playerSprite.enabled = true;
    }

    private void StopInvincible()
    {
        isInvincible = false;
    }

    // Make player active or inactive
    // If inactive, player is invincible, unable to fire, but still able to move
    // Used in boss spawning animation for example
    public void MakeActive(bool active)
    {
        if (active)
        {
            isInvincible = false;
            playerFire.canFire = true;
        }
        else
        {
            isInvincible = true;
            playerFire.canFire = false;
        }
    }
}
