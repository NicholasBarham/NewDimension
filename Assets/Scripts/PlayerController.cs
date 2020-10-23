using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private Animator anim = null;

    [SerializeField]
    private float speed = 0.1f;

    [SerializeField]
    private PlayerAudio playerAudio = null;

    [SerializeField]
    private bool isGrounded;

    public bool IsGrounded
    { 
        get
        {
            return isGrounded;
        }
        set 
        { 
            if(!IsGrounded && value)
            {
                playerAudio.PlayLanding();
            }

            isGrounded = value;
        } 
    }

    [SerializeField]
    private Transform groundChecker = null;
    [SerializeField]
    private float groundCheckerOffset = 0.2f;
    [SerializeField]
    private float groundCheckerLength = 0.075f;

    private Vector3 checkerOffset = Vector3.zero;

    #region JUMPING PHYISCS VARIABLES
    [Header("JUMPING")]
    [SerializeField]
    private float jumpForce = 4.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float holdJumpGravityMultiplier = 0.75f;

    [Header("Rising")]
    [SerializeField]
    private float risingGravity = -0.5f;

    [Header("Apex")]
    [SerializeField]
    private float apexRisingVelocityThreshold = 0.2f;
    [SerializeField]
    private float apexFallingVelocityThreshold = -0.2f;
    [SerializeField]
    private float apexGravity = 0.1f;

    [Header("Falling")]
    [SerializeField]
    private float fallingGravity = 5f;
    #endregion

    public LayerMask groundMask;

    [SerializeField]
    private Transform[] lanes = new Transform[3];
    private int currentLanePos = 1;

    [SerializeField]
    private Transform spawn = null;
    [SerializeField]
    private UnityEvent onSpawn = null;
    [SerializeField]
    private UnityEvent onDeath = null;

    private bool isDead = false;

    public bool IsDead => isDead;

    private void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if (anim == null)
            anim = GetComponent<Animator>();

        if (playerAudio == null)
            playerAudio = GetComponent<PlayerAudio>();

        checkerOffset = new Vector3(0f, 0f, groundCheckerOffset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 checkerPos = groundChecker.position;

        Gizmos.DrawLine(checkerPos, checkerPos + groundChecker.up * -1 * groundCheckerLength);
        Gizmos.DrawLine(checkerPos + checkerOffset, (checkerPos + checkerOffset) + (groundChecker.up * -1 * groundCheckerLength));
        Gizmos.DrawLine(checkerPos - checkerOffset, (checkerPos - checkerOffset) + (groundChecker.up * -1 * groundCheckerLength));
    }

    void Update()
    {
        if (IsDead)
            return;

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            IsGrounded = false;
            playerAudio.PlayJump();
        }

        if (Physics.Raycast(groundChecker.position, groundChecker.up * -1, out RaycastHit hitInfo, groundCheckerLength, groundMask) ||
                Physics.Raycast(groundChecker.position + checkerOffset, groundChecker.up * -1, out hitInfo, groundCheckerLength, groundMask) ||
                Physics.Raycast(groundChecker.position - checkerOffset, groundChecker.up * -1, out hitInfo, groundCheckerLength, groundMask))
        {

            if (hitInfo.collider.CompareTag("Ground"))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
        else
        {
            IsGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.A) && IsGrounded)
        {
            if (currentLanePos > 0)
            {
                currentLanePos--;
                rb.position = new Vector3(lanes[currentLanePos].position.x, transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && IsGrounded)
        {
            if (currentLanePos < 2)
            {
                currentLanePos++;
                rb.position = new Vector3(lanes[currentLanePos].position.x, transform.position.y, transform.position.z);
            }
        }

        anim.SetFloat("MovementSpeed", rb.velocity.magnitude);
        anim.SetBool("IsGrounded", IsGrounded);
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        //rb.MovePosition(new Vector3(lanes[currentLanePos].localPosition.x, transform.position.y, transform.position.z) * laneChangeSpeed * Time.fixedDeltaTime);

        // The jumping/falling physics to control player
        if (!IsGrounded)
        {
            if (rb.velocity.y > apexRisingVelocityThreshold)
            {
                if (Input.GetButton("Jump"))
                    rb.AddForce(transform.up * (risingGravity * holdJumpGravityMultiplier));
                else
                    rb.AddForce(transform.up * risingGravity);
            }
            else if (rb.velocity.y <= apexRisingVelocityThreshold && rb.velocity.y >= apexFallingVelocityThreshold)
            {
                if (Input.GetButton("Jump"))
                    rb.AddForce(transform.up * apexGravity * holdJumpGravityMultiplier);
                else
                    rb.AddForce(transform.up * apexGravity);
            }
            else if (rb.velocity.y < apexFallingVelocityThreshold)
            {
                rb.AddForce(transform.up * fallingGravity);
            }
        }
    }

    public void Spawn()
    {
        if (spawn != null)
            transform.position = spawn.position;

        currentLanePos = 1;
        rb.position = new Vector3(lanes[currentLanePos].position.x, transform.position.y, transform.position.z);

        onSpawn?.Invoke();
    }

    public void Die()
    {
        if(!isDead)
            onDeath?.Invoke();
    }

    public void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }
}
