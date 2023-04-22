using TMPro;
using UnityEngine;

// Represents a movable cosmic ray with movement that can be initiated by the user.
public class CosmicRayInteractive : CosmicRayAbstractMovable
{
    // The speed of the particle when in motion.
    public float Speed = 1.8f;
    public AnimationCurve EnergyCurve;
    public Animator BarLoop;
    public Animator SunLoop;

    private float oscillationState;
    private bool _hasLaunched = false;
    private int _energyPercentile = -1;
    private int _clusteringPercentile = -1;
    // The supplied label of to update with the number of protons detected.
    private TextMeshProUGUI _energyLabel;
    // The supplied label of to update with the total number of particles detected.
    private TextMeshProUGUI _occurrenceLabel;
    // The supplied label of to update with the total number of particles detected.
    private TextMeshProUGUI _promptLabel;

    private const int MAX_GREEN_COMPONENT = 0xcc;
    private const int MIN_GREEN_COMPONENT = 0x47;

    private readonly KinectController _kinectController = KinectController.GetInstance();

    private void Awake()
    {
        Init();

        _energyLabel = GameObject.Find("Energy Value").GetComponent<TextMeshProUGUI>();
        _occurrenceLabel = GameObject.Find("Rate Value").GetComponent<TextMeshProUGUI>();
        _promptLabel = GameObject.Find("Interaction Prompt").GetComponent<TextMeshProUGUI>();
    }

    protected override void Init()
    {
        base.Init();

        speed = Speed;
        _kinectController.InitSensor();
        _kinectController.AddGestureCallback(LightBuzz.Vitruvius.GestureType.SwipeRight, Controller);
        rb.Sleep();
    }

    void Update()
    {
        if(!_hasLaunched)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Controller();
            }
        }
    }

    // Puts the cosmic ray in motion if user input has been detected.
    public override void InitiateCosmicRay()
    {
        _hasLaunched = true;

        // Sets the particle colour based on the progress through the sun loops animation.
        oscillationState = SunLoop.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        BarLoop.enabled = false;

        if (oscillationState != 1) oscillationState = 1 - Mathf.Abs(oscillationState - 0.5f) * 2;

        float greenComponent = MAX_GREEN_COMPONENT - oscillationState * (MAX_GREEN_COMPONENT - MIN_GREEN_COMPONENT);
        particleColour = new Color(1f, greenComponent/255f, 0);
        GetComponentsInChildren<SpriteRenderer>()[1].color = particleColour;

        InitMovement();
    }

    // Normalises the clustering value betweeon 0-100.
    private int CalculateClusteringPercentile()
    {
        int percentage = Mathf.RoundToInt(oscillationState * 100);
        return percentage;
    }

    private int CalculateEnergyPercentile(long timeElapsed)
    {
        // The timeElapsed variable has not been set.
        if(timeElapsed == -1)
        {
            return 50;
        }

        float minimumTime = EnergyCurve.keys[0].time;
        float maximumTime = EnergyCurve.keys[EnergyCurve.length - 1].time;

        return Mathf.RoundToInt(EnergyCurve.Evaluate(Mathf.Clamp(timeElapsed, minimumTime, maximumTime)));
    }

    private void Controller()
    {
        InitiateCosmicRay();
        SetPercentileVariables();
        UpdateLabels();
        _kinectController.CloseSensor();
        FindObjectOfType<BodySourceView>().enabled = false;
    }

    private void SetPercentileVariables()
    {
        _clusteringPercentile = CalculateClusteringPercentile();
        _energyPercentile = CalculateEnergyPercentile(_kinectController.GetGestureController().GetGestures()[0].TimeElapsed);

        PlayerPrefs.SetInt("clustering", _clusteringPercentile);
        PlayerPrefs.SetInt("energy", _energyPercentile);
    }

    private void UpdateLabels()
    {
        _occurrenceLabel.text = _clusteringPercentile + "%";
        _energyLabel.text = _energyPercentile + "%";

        Destroy(_promptLabel.gameObject);
    }
}