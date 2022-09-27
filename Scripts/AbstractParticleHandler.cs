using System.Collections;
using UnityEngine;
using DG.Tweening;

// Handles the computation of particles in the cosmic ray shower.
public abstract class AbstractParticleHandler : MonoBehaviour
{
    // The parent GameObject of cosmic ray shower particles.
    public static Transform s_parent;

    protected float _trailEndAlpha = -1f;
    // The specific type of particle.
    protected Rigidbody2D _rigidbody;
    protected bool _hasBeenDestroyed = false;
    protected static readonly System.Random s_rand = new();

    private SpriteRenderer[] _spriteRenderers;

    public abstract IEnumerator StartLife();
    public abstract float GetEnergy();
    public abstract int GetNumberOfParticles();
    public abstract Color GetTrailColour();

    private void SetTrailProperties(float startAlpha)
    {
        // .2f is the alpha of the very start of the CRS.
        float alpha = (startAlpha < 0 ? 0 : startAlpha);
        _trailEndAlpha = alpha + .15f;

        var trailColor = GetTrailColour();
        var gradient = new Gradient();

        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(trailColor, 0f), new GradientColorKey(trailColor, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 1f), new GradientAlphaKey(_trailEndAlpha, 0f) }
        );

        GetComponentInChildren<TrailRenderer>().colorGradient = gradient;
    }

    // Finalises initialisation of all cosmic ray shower particles.
    protected void InitialiseParticle(float startAlpha)
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();

        SetTrailProperties(startAlpha);

        StartCoroutine(StartLife());
    }

    // Causes a potential split and then destruction of the particle based on the particle's energy level.
    protected virtual void Trigger()
    {
        DestroyParticle(true);
    }

    // Spawns new particles that project from the current particle's position.
    protected void Split()
    {
        // The most recent GameObject spawned in the split.
        GameObject o;
        // The maximum angle size in degrees that the particle can rotate within.
        var angleRange = 30;
        // The angle of rotation for the newly spawned particle.
        float angle;

        for(var i = 0; i < GetNumberOfParticles(); i++)
        {
            // Samples a random value from the valid angle values.
            angle = Random.Range(0, angleRange) - (float)(angleRange / 2f);

             // Quit coroutine if this object has been destroyed.
            if (_rigidbody == null || _rigidbody.Equals(null))
            {
                break;
            }
            else
            {
                // Generates the new particle.
                o = Instantiate(ParticleHandlerShower.s_particlePrefab, _rigidbody.position, Quaternion.Euler(0, 0, Mathf.RoundToInt(angle)), s_parent);

                // Assigns the ParticleHandler script to the particle.
                ParticleHandlerShower ph = o.GetComponent<ParticleHandlerShower>();

                ParticleType toSpawn = s_rand.NextDouble() < .2f ? new Proton() : new NonProton();

                // The energy value for SEP --> shower is simply copied, and the energy is split evenly from shower --> shower.
                ph.InitialiseShowerParticle(toSpawn, _trailEndAlpha, GetEnergy());
            }
        }
    }

    // Destroys the shower particle.
    public void DestroyParticle(bool enable)
    {
        // If the particle has not already been destroyed.
        if (!_hasBeenDestroyed)
        {
            _hasBeenDestroyed = true;

            // Sets velocity to zero (to stop the trail).
            _rigidbody.velocity = Vector2.zero;

            StartCoroutine(KillParticle());

            if(enable) GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    private IEnumerator KillParticle()
    {
        var fadeTime = .3f;
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(_spriteRenderers[0].DOFade(0, fadeTime));

        for (int i = 1; i < _spriteRenderers.Length; i++)
        {
            mySequence.Join(_spriteRenderers[i].DOFade(0, fadeTime));
        }

        yield return mySequence.SetEase(Ease.InQuad).WaitForCompletion();
        
        // Destroys the particle child object, keeping the trail.
        Destroy(transform.GetChild(0).gameObject);
    }
}