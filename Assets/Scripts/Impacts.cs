using UnityEngine;
using TMPro;
using DG.Tweening;
public class Impacts : MonoBehaviour
{
    private CanvasGroup cg;
    private TextMeshProUGUI tm;

    void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
        cg = GetComponent<CanvasGroup>();
    }

    public void SetImpact(bool particlesDetected, bool particlesNearApproach)
    {
        if(!particlesDetected)
        {
            tm.text = particlesNearApproach ? "Space tech lifespans shortened!" : "Astronaut safety compromised!";
        }
        else
        {
            int numberOfImpacts = 9;
            int impact = PlayerPrefs.GetInt("impact");

                 if (impact < 100 / numberOfImpacts * 1) tm.text = "Video games glitched!";
            else if (impact < 100 / numberOfImpacts * 2) tm.text = "Mobile phones locked-up!";
            else if (impact < 100 / numberOfImpacts * 3) tm.text = "Unexpected stock exchange jumps!";
            else if (impact < 100 / numberOfImpacts * 4) tm.text = "Localised power outage!";
            else if (impact < 100 / numberOfImpacts * 5) tm.text = "Near miss train collisions!";
            else if (impact < 100 / numberOfImpacts * 6) tm.text = "Self-driving cars veer off course!";
            else if (impact < 100 / numberOfImpacts * 7) tm.text = "Loss of aircraft control!";
            else if (impact < 100 / numberOfImpacts * 8) tm.text = "Large-scale internet shutdown!";
            else tm.text = "Global power outage!";
        }

        cg.DOFade(1, 1.5f);
    }
}
