using UnityEngine;
using TMPro;
using System.Collections;

 /*
  * Updates the supplied label GameObject with the
  * distance between child Collider2D components and
  * the supplied Earth Colider2D.
  */
public class CalculateDistance : MonoBehaviour
{
    // The label to update with the current distance calculated value.
	public TextMeshProUGUI Label;
    /*
     * The Earth GameObject's Collider2D component - the shared object
     * that all child Collider2D components calculate their distance from.
     */
    public Collider2D EarthColl;
    /*
     * The real life distance in feet between the earth's surface and the
     * starting cosmic ray position.
     */
    public int TotalDistanceInFeet = 150000;

    // The amount of time between label updates.
    public float UpdateInterval = .1f;

    // The initial distance between the cosmic ray and Earth colliders.
    private float _initialDistance;
    // Stores the current smallest distance calculated in the script's lifetime.
    private float _shortestDistance;
    // States whether an overlap of child colliders and the earth collider has been detected.
    private bool _overlapDetected = false;

    private float _currentDistanceInFeet;

    private bool _canExit = false;

    readonly KinectController _kinectController = KinectController.GetInstance();

    private void Start()
    {
        StartCoroutine(LabelUpdates());
    }

    /*
     * Iteratively calculates distance values between child Collider2D
     * components and the supplied Earth Collider2D, and updates the supplied
     * label's text.
     *
     * This routine loops continuously
     */
    private IEnumerator LabelUpdates()
	{
        /*
         * Forces waiting for the next frame. 
         * Testing has found it is required to calculate an accurate
         * initial distance between the cosmic ray and Earth.
         */
        yield return 0;

        var cosmicRayCollider = GetComponentInChildren<Collider2D>();
        _initialDistance = Physics2D.Distance(cosmicRayCollider, EarthColl).distance;
        _shortestDistance = _initialDistance;
        
        // Loops until no child colliders exist or an overlap has been detected.
        while (true)
        {
            // Pauses the coroutine for updateInterval seconds.
            yield return new WaitForSeconds(UpdateInterval);

            Collider2D[] collArr = GetComponentsInChildren<Collider2D>();

            if(collArr.Length == 0)
            {
                /* 
                 * If the number of children is 0 then wait and check again.
                 * 
                 * This should prevent the potential occurence of collArr
                 * being 0 because of searching for children just as all
                 * children have been destroyed.
                 */
                yield return new WaitForSeconds(.1f);

                if(GetComponentsInChildren<Collider2D>().Length == 0)
                {
                    _kinectController.InitSensor();
                    _kinectController.AddGestureCallback(LightBuzz.Vitruvius.GestureType.SwipeRight, Exit);
                    _canExit = true;

                    StartCoroutine(RunImpactScript());

                    break;
                }
            }
            else if (_overlapDetected)
            {
                if(!Label.text.Equals("0")) Label.text = "0";
                continue;
            }
            else
            {
                ColliderDistance2D iterationDistance;

                foreach (Collider2D coll in collArr)
                {
                    // Both checks required to determine if the object has been destroyed.
                    if(coll == null || coll.Equals(null))
                    {
                        continue;
                    }
                    else
                    {
                        iterationDistance = Physics2D.Distance(coll, EarthColl);
                    }

                    // Updates the shortest distance if it is valid and a new shortest has been found.
                    if (iterationDistance.distance < _shortestDistance && iterationDistance.distance > 0)
                    {
                        _shortestDistance = iterationDistance.distance;
                    }
                }

                // Calculates the proportion of the initial distance remaining, and then multiplies by total distance in feet.
                _currentDistanceInFeet = _shortestDistance / _initialDistance * TotalDistanceInFeet;
                Label.text = Mathf.RoundToInt(_currentDistanceInFeet).ToString();
            }
        }
    }

    // Used to set that an overlap has been detected from external scripts.
    public void SetHasOverlapped()
    {
        _overlapDetected = true;
    }

   IEnumerator RunImpactScript()
   {
        yield return new WaitForSecondsRealtime(4f);
        FindObjectOfType<Impacts>().SetImpact(_overlapDetected, _currentDistanceInFeet < TotalDistanceInFeet / 2);
   }

    void Update()
    {
        if(_canExit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Exit();
            }
        }
    }

    void Exit()
    {
        _kinectController.CloseSensor();
        PlayerPrefs.DeleteAll();
        FindObjectOfType<SceneTransitionController>().LoadNextScene();
    }
}
