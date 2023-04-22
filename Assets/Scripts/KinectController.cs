using Windows.Kinect;
using System;
using LightBuzz.Vitruvius;

public sealed class KinectController
{
    private static KinectController _instance = null;

    private KinectSensor _sensor;
    private MultiSourceFrameReader _reader;
    private GestureController _gestureController;

    public GestureController GetGestureController() { return _gestureController; }

    public KinectSensor GetSensor()
    { 
        return _sensor;
    }

    public static KinectController GetInstance()
    {
        if (_instance is null)
        {
            _instance = new KinectController();
        }

        return _instance;
    }

    private KinectController() { }

    public void InitSensor()
    {
        _sensor = KinectSensor.GetDefault();

        if (!_sensor.IsOpen)
        {
            _sensor.Open();
        }

        _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
        _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
    }

    public void AddGestureCallback(GestureType gt, Action callback)
    {
        _gestureController = new GestureController(gt);

        _gestureController.GestureRecognized += (object sender, GestureEventArgs e) => {
            callback();
        };
    }

    private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
    {
        var reference = e.FrameReference.AcquireFrame();

        using var frame = reference.BodyFrameReference.AcquireFrame();

        if (frame != null)
        {
            Body body = BodyExtensions.Closest(frame.Bodies());

            if (body != null)
            {
                _gestureController.Update(body);
            }
        }
    }

    public void CloseSensor()
    {
        if(_sensor != null && _sensor.IsOpen)
        {
            _sensor.Close();
        }
        _sensor = null;

        if(_reader != null)
        {
            _reader.Dispose();
            _reader = null;
        }
    }
}