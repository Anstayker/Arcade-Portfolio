using UnityEngine;

public class BunnyMovement : BunnyCharacter
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float horizontalDamping = 10f;

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float inputX = Input.GetAxisRaw("Horizontal");
        float targetVx = inputX * moveSpeed;
        float vx = Mathf.Lerp(rb.linearVelocity.x, targetVx, Mathf.Clamp01(dt * horizontalDamping));
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y); // Solo modifica X
    }
}