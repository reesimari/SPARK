using UnityEngine;

public class NewEmptyCSharpScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float fallMultiplier = 2f;   // higher = falls faster. try 1.5 to 2.5

    public Transform groundCheck;       // empty object at his feet auhhh
    public LayerMask groundLayer;       // the layer your platforms are on

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;
    private Collider2D col;
    private ContactFilter2D groundFilter;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        groundFilter.SetLayerMask(groundLayer);
        groundFilter.SetNormalAngle(45f, 135f);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = col.IsTouching(groundFilter);

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("jump");
}

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate(){
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // extra gravity only while falling, so he goes up normally but comes down faster
        if (rb.linearVelocity.y < 0){
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * rb.gravityScale
                                 * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}