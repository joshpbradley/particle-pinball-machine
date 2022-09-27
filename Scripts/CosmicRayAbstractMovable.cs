using UnityEngine;

// Abstract class for the cosmic rays that can move.
public abstract class CosmicRayAbstractMovable : CosmicRayBase
{
    // The speed of the cosmic ray in motion.
    protected static float speed;
    // The Rigidbody2D component associated with the cosmic ray.
    protected static Rigidbody2D rb;
    // Interface for initialising motion in derived cosmic rays.
    public abstract void InitiateCosmicRay();

    // Initialises the cosmic ray. This must be called by immediately derived classes.
    protected override void Init()
    {
        base.Init();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    protected void InitMovement()
    {
        tr.emitting = false;
        tr.Clear();
        // Move the cosmic ray
        rb.velocity = Vector2.right * speed;
        tr.emitting = true;
    }
}