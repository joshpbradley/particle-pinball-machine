using TMPro;
using UnityEngine;

/*
 * Detects proton particles that are attached to the associated collider.
 * 
 * This class also updates the supplied label to the total number of protons
 * detected by this collider.
 */
public class ParticleDetection : MonoBehaviour
{
    // The supplied label of to update with the number of protons detected.
    public TextMeshProUGUI ProtonLabel;
    // The supplied label of to update with the number of particles detected that aren't protons.
    public TextMeshProUGUI NonProtonLabel;

    // The number of protons that have collided with the associated collider.
    private int _protonCount = 0;
    // The total number of particles that have collided with the associated collider.
    private int _nonProtonCount = 0;
    // Tracks whether a particle has been detected.
    private bool _particleDetected = false;

    private void Start()
    {
        ProtonLabel.text = _protonCount.ToString();
        NonProtonLabel.text = _nonProtonCount.ToString();
    }

	public void OnTriggerEnter2D(Collider2D col)
    {
        // If this trigger is the first time the collider has detected a particle.
        if (!_particleDetected &&
                typeof(ParticleType).IsAssignableFrom(col.gameObject.GetComponentInParent<ParticleHandlerShower>().GetParticleType().GetType()))
        {
            FindObjectOfType<CalculateDistance>().SetHasOverlapped();
            _particleDetected = true;
        }

        // True if the detected object is a proton.
        if (typeof(Proton).IsAssignableFrom(col.gameObject.GetComponentInParent<ParticleHandlerShower>().GetParticleType().GetType()))
        {
            _protonCount++;

            if (ProtonLabel != null) ProtonLabel.text = _protonCount.ToString();
        }
        else
        {
            _nonProtonCount++;
            
            if (NonProtonLabel != null) NonProtonLabel.text = _nonProtonCount.ToString();
        }

        col.gameObject.transform.parent.GetComponent<ParticleHandlerShower>().DestroyParticle(false);
    }
}