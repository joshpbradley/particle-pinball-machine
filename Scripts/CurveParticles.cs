using UnityEngine;

// Controls the Coronal Mass Ejection (CME) animations around the Sun.
public class CurveParticles : MonoBehaviour
{
    // The speed of the particles in the particle system.
    public float ParticleSpeed = 20f;
    // The highest angular velocity that a particle can be assigned.
    public int UpperAngularVelocity = 270;
    // The highest angular velocity that a particle can be assigned.
    public int LowerAngularVelocity = 200;
    // The particle System that animates the particles.
    private ParticleSystem _particleSystem;
    // The vector associated with the supplied particle speed.
    private Vector3 _normalisedVel;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _normalisedVel = Vector3.up * ParticleSpeed * .001f;
    }

    private void FixedUpdate()
    {
        var particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        // Gets the number of active particles in the particle system.
        int numParticlesAlive = _particleSystem.GetParticles(particles);

        ParticleSystem.Particle p;
        float angularVelocity;

        for (int i = 0; i < numParticlesAlive; ++i)
        {
            p = particles[i];

            angularVelocity = GetParticleAngularVelocity(p);

            // True if this particle has not yet been emitted.
            if (Init(p))
            {
                // Assign a start rotation to the particle.
                p.rotation = GetStartRotation(p);
                // Set the lifetime for the particle, based on the angular velocity.
                p.startLifetime = 360 / Mathf.Abs(angularVelocity);
                // Set the remaining lifetime of the particle.
                p.remainingLifetime = p.startLifetime;
            }
            else
            {
                // Rotate the particle based on its angular velocity.
                if (GetParticleDirectionInverted(p))
                {
                    p.rotation += angularVelocity * Time.deltaTime;
                }
                else
                {
                    p.rotation -= angularVelocity * Time.deltaTime;
                }
            }

            // Update the position based on the supplied speed and the rotation direction.
            p.position += Quaternion.AngleAxis(p.rotation, Vector3.forward) * _normalisedVel;

            particles[i] = p;
        }

        // Updates the particlesystem with the updated particles.
        _particleSystem.SetParticles(particles, numParticlesAlive);
    }

    /*
     * Calculates the starting rotation of the given particle.
     * This is calculated by setting the rotation to the normal of the
     * particle system.
     */
    private float GetStartRotation(ParticleSystem.Particle p)
    {
        // The vector between the posticle and the particle system origin.
        Vector2 emissionVector = p.position - _particleSystem.transform.position;
        // The normal to the emission vector.
        Vector2 normalToEmission = Vector2.Perpendicular(emissionVector);

        /*
         * Generates an initial angle for the rotation.
         * The result is in the range [-180, 180]; so it needs to be normalised.
         */
        float angle = Vector3.SignedAngle(Vector3.up, normalToEmission, Vector3.forward);

        // Inverts the rotation if the particle is non-inverted (moving clockwise).
        return (angle + (GetParticleDirectionInverted(p) ? 180 : 360)) % 360;
    }

    // Determines whether the given particle has already been emitted.
    private bool Init(ParticleSystem.Particle p)
    {
        // float.PositiveInfinity is the initial lifetime of a particle.
        return p.startLifetime == float.PositiveInfinity;
    }

    // Generates an angular velocity for a particle within supplied upper/lower bounds.
    private float GetParticleAngularVelocity(ParticleSystem.Particle p)
    {
        return p.randomSeed % (UpperAngularVelocity - LowerAngularVelocity) + LowerAngularVelocity;
    }

    // Determines whether the particle will move clockwise or anticlockwise.
    private bool GetParticleDirectionInverted(ParticleSystem.Particle p)
    {
        return p.randomSeed % 2 == 0;
    }
}