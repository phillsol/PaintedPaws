using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public Rigidbody playerRB;
    SpriteRenderer spriteRenderer;

    [SerializeField] float speed = 10;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float airClamp;
    [SerializeField] float jumpClamp;

    public float flipRate = 1;
    public float squishRate = 1;
    public float squashRate = 1;
    bool faceRight = true;
    bool landing = false;
    float wait;
    

    Vector3 prevVelocity;

    [SerializeField]
    float currentGravityScale;
    [SerializeField]
    float jumpGravityScale = 0.2f;
    [SerializeField]
    float fallGravityScale = 1f;
    public static float globalGravity = -9.81f;


    public bool isGrounded;
    bool isJumping;

    public bool isFrozen;
    public bool inConversation;
    public Transform ConversationLocation;
    public bool ChatFaceRight;
    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        playerRB.useGravity = false;

        isGrounded = true;
    }

    public void FreezePlayer()
    {
        isFrozen = true;
    }
    public void UnFreezePlayer()
    {
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            playerRB.velocity = Vector3.zero;
            return;
        }

        if (!inConversation)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized * speed * Time.deltaTime;
            playerRB.velocity += moveDirection;
        }
        else
        {
            MoveToLocation();
        }
        

        GravityCheck();


        if (!isGrounded && playerRB.velocity.y > 0.01)
        {
            playerRB.velocity = new Vector3(Mathf.Clamp(playerRB.velocity.x, -airClamp, airClamp), playerRB.velocity.y, Mathf.Clamp(playerRB.velocity.z, -airClamp, airClamp));
        }




        SpriteFlip();
        SpriteSquishAndSquash();

        prevVelocity = playerRB.velocity;

        //spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.z * -100 + 30000);


    }

    void FixedUpdate()
    {
        if (!inConversation)
        {
            if (isGrounded && Input.GetKey(KeyCode.Space) && !isJumping) //jumps if on ground, can hold jump to jump again as soon as grounded again
            {
                float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (globalGravity * currentGravityScale) * playerRB.mass);
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                
                isJumping = true;

            }
            playerRB.velocity = new Vector3(playerRB.velocity.x, Mathf.Clamp(playerRB.velocity.y, -jumpClamp, jumpClamp), playerRB.velocity.z);
        }

    }

    public void MoveToLocation()
    {
        Vector3 locationDirection = ConversationLocation.position - transform.position;

        if (locationDirection.x < 0.1 && locationDirection.z < 0.1)
        {
            faceRight = ChatFaceRight;
        }
        else
        {
            playerRB.velocity += locationDirection * speed * Time.deltaTime;
        }
        

    }

    void GravityCheck()
    {
        if (playerRB.velocity.y < -0.01)
        {
            currentGravityScale = fallGravityScale;
            isJumping = false;
        }
        else
        {
            currentGravityScale = jumpGravityScale;
        }
        Vector3 gravity = globalGravity * currentGravityScale * Vector3.up;
        playerRB.AddForce(gravity, ForceMode.Acceleration);


        if (currentGravityScale == jumpGravityScale && playerRB.velocity.y < -0.01)
        {
            isGrounded = true;
        }


    }

    void SpriteFlip()
    {
        if (playerRB.velocity.x > 1)
        {
            faceRight = true;
        }
        else if (playerRB.velocity.x < -1)
        {
            faceRight = false;
        }

        if (faceRight) { spriteRenderer.transform.localScale = new Vector3(Mathf.Lerp(spriteRenderer.transform.localScale.x, 1, Time.deltaTime * flipRate), 1, 1); }
        else { spriteRenderer.transform.localScale = new Vector3(Mathf.Lerp(spriteRenderer.transform.localScale.x, -1, Time.deltaTime * flipRate), 1, 1); }
    }
    
    void SpriteSquishAndSquash()
    {
        if (playerRB.velocity.y > 0.01)
        {
            spriteRenderer.transform.localScale = new Vector3(Mathf.Lerp(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.x * 0.8f, Time.fixedDeltaTime * squishRate),
                                                                Mathf.Lerp(spriteRenderer.transform.localScale.y, 1.2f, Time.fixedDeltaTime * squishRate), 1);
        }

        
        if (playerRB.velocity.y < -0.01)
        {
            landing = true;
        }
        if (landing && playerRB.velocity.y >= 0)
        {
            
            wait += Time.deltaTime;
            
            spriteRenderer.transform.localScale = new Vector3(Mathf.Lerp(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.x * 1.2f, Time.fixedDeltaTime * squashRate),
                                                                Mathf.Lerp(spriteRenderer.transform.localScale.y, 0.8f, Time.fixedDeltaTime * squashRate), 1);

            if (wait >= 0.1)
            {
                landing = false;
                wait = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Ground"))
        {

            isJumping = false;

            playerRB.velocity = new Vector3(prevVelocity.x, 0, prevVelocity.z);
        }

        

    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}