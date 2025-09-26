using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BunnyCharacter : MonoBehaviour
{
    public enum BunnyState { InAir, PreparingJump, JustGrounded }
    public BunnyState CurrentState => state;
    protected BunnyState state = BunnyState.InAir;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // we control gravity manually
            rb.gravityScale = 0f;
            // freeze rotation so physics doesn't spin player
            rb.freezeRotation = true;
        }
    }
}
