using UnityEngine;

/*
 * Returns the vector representing the given camera's
 * width and height, as well as the camera's position.
 */
public static class CameraExtensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
       var screenAspect = (float)Screen.width / (float)Screen.height;
       float cameraHeight = camera.orthographicSize * 2;
        
        return new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
    }
}
