// Represents a movable cosmic ray with movement that moves automatically.
public class CosmicRayStandard : CosmicRayAbstractMovable
{
    // The speed of the cosmic ray in motion.
    public float Speed = 4f;

    void Awake()
    {
        Init();
        speed = Speed;
    }

    void Start()
    {
        InitiateCosmicRay();
    }

    public override void InitiateCosmicRay()
    {
        InitMovement();
    }
}