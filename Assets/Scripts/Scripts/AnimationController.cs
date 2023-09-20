using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    [SerializeField] PlayerController playerController;

    float horizontal;
    float vertical;
    void Awake()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Walking anim if walking
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (playerController.isGrounded && !playerController.inConversation)
        {
            animator.SetBool("isWalking", (Mathf.Abs(horizontal) > 0.8 || Mathf.Abs(vertical) > 0.8));
        }

        


    }
}
