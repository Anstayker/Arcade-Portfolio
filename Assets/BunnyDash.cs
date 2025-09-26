using UnityEngine;

public class BunnyDash : BunnyCharacter
{
    [Header("Dash Settings")]
    [SerializeField] float dashForce = 18f;
    [SerializeField] float dashDuration = 0.18f;
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] float dashCooldown = 0f; // Si quieres cooldown adicional

    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private int lastDashDir = 1;
    
    // Referencia a BunnyBounce para saber si está en el suelo
    private BunnyBounce bounce;

    public bool IsDashing => isDashing;

    protected override void Awake()
    {
        base.Awake();
        bounce = GetComponent<BunnyBounce>();
    }

    void Update()
    {
        // Recarga dash al tocar el suelo
        if (bounce != null && bounce.IsGrounded() && !isDashing)
        {
            canDash = true;
        }

        // Cooldown opcional
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Input dash
        if (Input.GetKeyDown(dashKey) && canDash && dashCooldownTimer <= 0f && !isDashing)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            int dashDir = inputX != 0 ? (int)Mathf.Sign(inputX) : lastDashDir;
            lastDashDir = dashDir;
            StartDash(dashDir);
        }

        // Dash timer
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    void StartDash(int direction)
    {
        isDashing = true;
        canDash = false;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        // Aplica velocidad horizontal fuerte, mantiene la vertical
        rb.linearVelocity = new Vector2(direction * dashForce, rb.linearVelocity.y);
        // Opcional: desactiva gravedad o rebote aquí si quieres un dash "puro"
    }

    void EndDash()
    {
        isDashing = false;
        // Opcional: puedes restaurar control aquí
    }
}
