using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // GameObjects
    public GameObject bulletPrefab;
    public GameObject hitBox;
    public AudioSource audioSourceFire;
    public AudioSource audioSourceHit;
    private Rigidbody2D rb2D;
    public Image lifeBar;
    public SpriteRenderer playerSprite;

    // Parameters
    public float speed;
    public float fireDelay;
    public float focusModifier;
    public float maxHealth = 3f;

    //Locals
    float activeModifier = 1;
    float cooldown = 0;

    //Private
    private float health;
    private bool isInvincible = false;
    private bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        health = maxHealth;
        lifeBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetAxis("Fire1") == 1 && cooldown < 0 && canFire)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            audioSourceFire.Play();
            cooldown = fireDelay;
        }

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
        lifeBar.fillAmount = 1;
        MakeInvincible();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * activeModifier * Time.deltaTime;
        rb2D.MovePosition(rb2D.position + velocity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isInvincible && (other.tag == "enemyBullet" || other.tag == "enemy")) {
            lifeBar.fillAmount -= 1f / maxHealth;
            audioSourceHit.Play();
            health--;
            if (health <= 0) {
                // Destruction Sequence
                GameManager.instance.TriggerContinue();
            }
            else
            {
                MakeInvincible();
            }
            if (other.tag == "enemyBullet") {
                Destroy(other.gameObject);
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
            canFire = true;
        }
        else
        {
            isInvincible = true;
            canFire = false;
        }
    }
}
