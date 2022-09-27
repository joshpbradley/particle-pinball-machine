using UnityEngine;

// Handles the events in the Transition scene of the app.
public class EventHandler : MonoBehaviour
{
    // The events that can be subscribed to.
    public enum EventType { NEXT_SCENE, STATIONARY, LAUNCH_AESTHETIC, START_CHAIN };
    // The subscribed event.
    public EventType Event;
    // Rigidbody2D components attached to the aesthetic sprites in the scene.
    public Rigidbody2D[] AestheticRigidbodies;
    // The Rigidbody2D component of the cosmic ray.
    public Rigidbody2D ParticleRigidbody;
    // If true, the event is triggered on a collider entering the associated collider.
    public bool OnEnter = false;
    // If true, the event is triggered on a collider exiting the associated collider.
    public bool OnExit = false;
    // The current index inspected in the AestheticRigidbodies array.
    private static int s_aestheticRigidbodiesIndex;

    private const float _AESTHETIC_SEP_VEL_MULTIPLIER = 1f;

    public EventHandler()
    {
    }

    private void Awake()
    {
        s_aestheticRigidbodiesIndex = 0;
    }

    // Events that can be triggered upon a collider entering this collider instance.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnEnter)
        {
            switch (Event)
            {
                // Loads the next scene.
                case EventType.NEXT_SCENE:
                    FindObjectOfType<SceneTransitionController>().LoadNextScene();
                    break;
                // Stops the rigidbody associated with the collider.
                case EventType.START_CHAIN:
                    StartAestheticChain();
                    break;
            }
        }
    }

    // Events that can be triggered upon a collider exiting this collider instance.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (OnExit)
        {
            switch (Event)
            {
                // Loads the next scene.
                case EventType.NEXT_SCENE:
                    FindObjectOfType<SceneTransitionController>().LoadNextScene();
                    break;
                // Launches the next Rigidbody2D in the supplied array.
                case EventType.LAUNCH_AESTHETIC:
                    LaunchAesthetic(collision.attachedRigidbody);
                    break;
                // Stops the rigidbody associated with the collider.
                case EventType.STATIONARY:
                    Stop(collision.attachedRigidbody);
                    break;
            }
        }
    }

    // Launch the Rigidbody components of the supplied aesthetic objects or the cosmic ray.
    private void LaunchAesthetic(Rigidbody2D rb)
    {
        int index = s_aestheticRigidbodiesIndex;

        // Determmines whether an element after the current index position exists.
        if(index == 0)
        {
            AestheticRigidbodies[0].velocity = -ParticleRigidbody.velocity / _AESTHETIC_SEP_VEL_MULTIPLIER;
        }
        else if (index < AestheticRigidbodies.Length)
        {
            // Sets the next Rigidbody velocity to that of the previous one.
            AestheticRigidbodies[index].velocity = AestheticRigidbodies[index - 1].velocity;
        }
        else
        {
            EndAestheticChain(rb);
        }

        s_aestheticRigidbodiesIndex++;
    }

    // Stops a GameObject by setting its Rigidbody2D velocity to 0.
    private void Stop(Rigidbody2D rb)
    {
        if(rb != null)
        {
            // Sets the current object's velocity to 0.
            rb.velocity = Vector2.zero;
        }
    }

    private void StartAestheticChain()
    {
        if (s_aestheticRigidbodiesIndex == 0)
        {
            LaunchAesthetic(null);
            Stop(ParticleRigidbody);
        }
    }

    private void EndAestheticChain(Rigidbody2D rb)
    {
        ParticleRigidbody.velocity = -rb.velocity * _AESTHETIC_SEP_VEL_MULTIPLIER;
    }
}