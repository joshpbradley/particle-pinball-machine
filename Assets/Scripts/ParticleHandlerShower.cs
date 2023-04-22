using System.Collections;
using UnityEngine;

// Handles the computation of particles in the cosmic ray shower.
public class ParticleHandlerShower : AbstractParticleHandler
{
    /*
     * The energy of the particle.
     * Affects the likelihood of spawning more particles on trigger.
     */
    private float _energy;
    // The specific type of particle.
    private ParticleType _particleType;
    // The particle prefab used for all cosmic ray shower particles.
    public static GameObject s_particlePrefab;
    // The speed of particles in the cosmic ray shower.
    public static float s_speed;

    private int _numOfChildren;

    public void InitialiseShowerParticle(ParticleType particleType, float startAlpha, float energy)
    {
        _energy = energy;
        _particleType = particleType;

        // The number of particles that will spawn in this split event.
        var particleSpawnArray = new int[] { 1, 1, 2, 2, 2, 2, 3, 3 };
        _numOfChildren = particleSpawnArray[s_rand.Next(particleSpawnArray.Length)];

        // Sets the colour of the sprite.
        GetComponentsInChildren<SpriteRenderer>()[1].color = particleType.ParticleColour;
        InitialiseParticle(startAlpha);
    }

    public override IEnumerator StartLife()
    {
        // Quit coroutine if this object has been destroyed.
        if (_rigidbody == null || _rigidbody.Equals(null))
        {
            yield break;
        }
        else
        {
            // Assign velocity based on rotation and speed.
            _rigidbody.velocity = new Vector2(Mathf.Cos(_rigidbody.rotation * Mathf.Deg2Rad), Mathf.Sin(_rigidbody.rotation * Mathf.Deg2Rad)) * s_speed;

            yield return new WaitForSeconds(Random.Range(.8f, 3.3f));

            // Causes a potential split and then destruction of the particle.
            Trigger();
        }
    }

    // Causes a potential split and then destruction of the particle based on the particle's energy level.
    protected override void Trigger()
    {
        if (_energy > 1 && !_hasBeenDestroyed)
        {
            Split();
        }

        base.Trigger();
    }

    // Gets the particle type of the current particle.
    public ParticleType GetParticleType()
    {
        return _particleType;
    }

    public override Color GetTrailColour()
    {
        return _particleType.GetType().IsAssignableFrom(typeof(Proton)) ? Color.green : Color.white;
    }

    public override float GetEnergy()
    {
        return _energy / _numOfChildren;
    }

    public override int GetNumberOfParticles()
    {
        return _numOfChildren;
    }
}