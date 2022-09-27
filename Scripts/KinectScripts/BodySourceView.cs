using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;
using LightBuzz.Vitruvius;

public class BodySourceView : MonoBehaviour 
{
    public GameObject BodySourceManagerGameObject;
    public Texture image;

    public float MinX, MaxX, MinY, MaxY, MinZ, MaxZ;

    private Vector3 _leftHandPosition;
    private bool _display = false;
    private BodySourceManager _bsm;
    
    void Awake()
    {
        if (!BodySourceManagerGameObject.TryGetComponent(out _bsm))
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (_bsm.GetData() != null)
        {
            // Returns the closest tracked body, or null if one are detected.
            Body closest = BodyExtensions.Closest(_bsm.GetData());

            // Checks that a body has been found and its left hand has been tracked.
            if (closest == null || closest.Joints[JointType.HandLeft].TrackingState != TrackingState.Tracked)
            {
                _display = false;
            }
            else
            {
                Kinect.Joint j = closest.Joints[JointType.HandLeft];

                _leftHandPosition = new Vector3(
                    j.Position.X,
                    j.Position.Y,
                    j.Position.Z
                );

                _display = true;
            }
        }
    }

    void OnGUI()
    {
        if (_display)
        {
            // Clamps the measured hand position to set bounds.
            float x = Mathf.Clamp(_leftHandPosition.x, MinX, MaxX);
            float y = Mathf.Clamp(_leftHandPosition.y, MinY, MaxY);

            // Finds the proportion of the distance that the hand position fits within the bounds in the x and y dimension.
            x = (x - MinX) / (MaxX - MinX);
            y = (y - MinY) / (MaxY - MinY);

            // Draws the hand to the UI within the bounds.
            GUI.DrawTexture(new Rect(x * Screen.width - 50, Screen.height - (y * Screen.height) - 50, 100, 100), image);
        }
    }
}
