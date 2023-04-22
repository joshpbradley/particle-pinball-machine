using System.Collections;
using UnityEngine;

// Handles the computation of particles in the cosmic ray shower.
public class ParticleHandlerSEP : AbstractParticleHandler
{
    // Initialises the cosmic ray particle.
    public void InitialiseCosmicRay(GameObject particlePrefab, float speed)
    {
        s_parent = gameObject.transform.parent;
        ParticleHandlerShower.s_speed = speed;
        ParticleHandlerShower.s_particlePrefab = particlePrefab;

        InitialiseParticle(_trailEndAlpha);
    }

    public override IEnumerator StartLife()
    {
        yield return new WaitForSeconds(2f);

        // Causes a potential split and then destruction of the particle.
        Trigger();
    }

    public override Color GetTrailColour()
    {
        return Color.white;
    }

    protected override void Trigger()
    {
        if (!_hasBeenDestroyed)
        {
            Split();
        }

        base.Trigger();
    }

    public override float GetEnergy()
    {
        var showerIntensity = 9;

        return showerIntensity * (float)PlayerPrefs.GetInt("energy", 50)/100f;
    }

    public override int GetNumberOfParticles()
    {
        var maxChildParticles = 5;

        if(PlayerPrefs.GetInt("clustering", -1) == 100)
        {
            return maxChildParticles;
        }
        else
        {
            return Mathf.FloorToInt(PlayerPrefs.GetInt("clustering", 50) * (maxChildParticles + 1) / 100f);
        }
    }
}