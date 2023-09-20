using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoyoteCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            PlayerController.current.isGrounded = true;

            PlayerController.current.playerRB.velocity += Vector3.down * 0.1f;
        }
    }
}
