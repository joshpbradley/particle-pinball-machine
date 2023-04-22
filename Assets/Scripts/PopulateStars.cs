using UnityEngine;

// Fills the camera bounds with randomly sized, shaped, and positioned star sprites.
public class PopulateStars : MonoBehaviour
{
    // The number of stars spawned per unit area of the display.
    public float StarDensity;
    // The probability of a star having the accent sprite shape.
    public float StarAccentProbability;
    // The accent star prefab.
    public GameObject StarAccent;
    // The standard star prefab.
    public GameObject StarMain;

    public Transform parent;

    void Start()
    {
        GameObject currentStar;
        Bounds cameraBounds = CameraExtensions.OrthographicBounds(Camera.main);

        var width = cameraBounds.size.x / 2;
        var height = cameraBounds.size.y / 2;

        var starCount = Mathf.RoundToInt(StarDensity * width * height);

        float randY;
        float randX;
        
        for(int i = 0; i < starCount; i++)
        {
            // Generate a random position within the camera view.
            randX = Random.Range(-width, width);
            randY = Random.Range(-height, height);

            // Determines the prefab assigned to the current star.
            currentStar = Random.value < StarAccentProbability ? StarAccent : StarMain;

            // Create the star GameObject.
            Instantiate(currentStar, new Vector2(randX, randY), Quaternion.identity, parent.transform);
        }
    }
}