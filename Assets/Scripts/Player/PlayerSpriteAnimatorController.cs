using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteAnimatorController : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") < -0.1f) {
            anim.SetBool("IsTurningLeft", true);
            anim.SetBool("IsTurningRight", false);
        }
        else if (Input.GetAxis("Horizontal") > 0.1f) {
            anim.SetBool("IsTurningLeft", false);
            anim.SetBool("IsTurningRight", true);
        }
        else {
            anim.SetBool("IsTurningLeft", false);
            anim.SetBool("IsTurningRight", false);
        }
    }
}
