using UnityEngine;
using DG.Tweening;

// Initialises the cosmic ray shower by launching the cosmic ray.
public class InitialiseShower : MonoBehaviour
{
    // The prefab assigned to newly spawned particles in the shower.
    public GameObject ParticlePrefab;
    // The speed of the particles in the ray shower.
    public float Speed = 3f;

    private void Awake()
    {
        gameObject.AddComponent<ParticleHandlerSEP>();

        DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(300, 300);
    }

    private void Start()
    {
        gameObject.GetComponent<ParticleHandlerSEP>().InitialiseCosmicRay(ParticlePrefab, Speed);
    }
}