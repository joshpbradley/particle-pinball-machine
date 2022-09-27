using UnityEngine;

// Base class for cosmic ray instances.
public class CosmicRayBase : MonoBehaviour
{
    // The colour of the inner cosmic ray circle.
    protected static Color particleColour = new Color(1, 204f / 255f, 0);
    protected TrailRenderer tr;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        tr = GetComponentInChildren<TrailRenderer>();
        GetComponentsInChildren<SpriteRenderer>()[1].color = particleColour;
    }
}