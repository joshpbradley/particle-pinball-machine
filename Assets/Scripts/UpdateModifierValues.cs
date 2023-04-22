using UnityEngine;
using TMPro;
using System;

public class UpdateModifierValues : MonoBehaviour
{
    public TextMeshProUGUI rateLabel;
    public TextMeshProUGUI energyLabel;
    public TextMeshProUGUI impactLabel;

    void Awake()
    {
        rateLabel.text = PlayerPrefs.GetInt("clustering", 50).ToString() + "%";
        energyLabel.text = PlayerPrefs.GetInt("energy", 50).ToString() + "%";

        // Ensures values that are exactly between two integers are rounded up. E.g. 12.5 --> 13.
        int impact = (int)Math.Round(PlayerPrefs.GetInt("energy", 50) * PlayerPrefs.GetInt("clustering", 50) / 100f, MidpointRounding.AwayFromZero);
        impactLabel.text = impact.ToString() + "%";

        PlayerPrefs.SetInt("impact", impact);
    }
}
