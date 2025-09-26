using UnityEngine;

public class BunnyBounce : BunnyCharacter
{

    [Header("Bounce / Gravity")]
    [Tooltip("Gravity (negative value pulls down). We apply this manually.")]
    [SerializeField] float gravity = -30f;
    [SerializeField] float bounceVelocity = 12f;
    [SerializeField] float maxFallSpeed = -25f;

    [Header("Ground Check")]
    [SerializeField] Vector2 groundCheckOffset = new Vector2(0f, -0.5f);
    [SerializeField] float groundCheckRadius = 0.12f;
    [SerializeField] LayerMask groundLayer;

    [Header("Timing")]
    [SerializeField] float justGroundedDuration = 0.06f; // short window if needed for animation

    float verticalVelocity = 0f;
    bool wasGrounded = false;
    float justGroundedTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Ground detection
        bool grounded = IsGrounded();

        // Detect landing event
        bool justLanded = grounded && !wasGrounded;

        if (justLanded)
        {
            state = BunnyState.JustGrounded;
            justGroundedTimer = justGroundedDuration;
            // Apply immediate bounce impulse
            verticalVelocity = bounceVelocity;
            state = BunnyState.PreparingJump;
        }

        // If still grounded (not a fresh landing), prevent gravity from pulling player down
        if (grounded && !justLanded)
        {
            // stick to ground if not bouncing
            // keep vertical velocity zero to avoid penetration
            if (verticalVelocity <= 0f)
                verticalVelocity = 0f;
        }

        // If in air, integrate custom gravity
        if (!grounded)
        {
            verticalVelocity += gravity * dt;
            if (verticalVelocity < maxFallSpeed)
                verticalVelocity = maxFallSpeed;
        }

        // State transitions
        if (state == BunnyState.PreparingJump)
        {
            // once we have upward velocity, consider in air
            if (verticalVelocity > 0.01f)
                state = BunnyState.InAir;
        }

        if (state == BunnyState.JustGrounded)
        {
            justGroundedTimer -= dt;
            if (justGroundedTimer <= 0f)
                state = BunnyState.InAir; // fallback
        }

        // Updates vertical velocity
        Vector2 currentVel = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVel.x, verticalVelocity);
        wasGrounded = grounded;
    }

    public bool IsGrounded()
    {
        Vector2 pos = (Vector2)transform.position + groundCheckOffset;
        Collider2D c = Physics2D.OverlapCircle(pos, groundCheckRadius, groundLayer);
        return c != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
    }
}
