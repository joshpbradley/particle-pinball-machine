using UnityEngine;

// Zooms the camera so that the width of the reference object is fullly in view.
public class CameraFitWidth: MonoBehaviour
{
    // The SpriteRenderer component reference that is to be fully viewable by the camera.
    public SpriteRenderer Reference;

    private void Start()
    {
        Camera.main.orthographicSize = Reference.bounds.size.x * Screen.height / Screen.width * 0.5f;
    }
}