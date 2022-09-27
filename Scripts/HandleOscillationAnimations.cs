using System.Collections;
using UnityEngine;

public class HandleOscillationAnimations : MonoBehaviour
{
    public Animator[] animations;
    public float MaximumPlayTime = .1f;
    public float MinimumPlayTime = .1f;
    public float Speed = 1f;

    void Start()
    {
        StartCoroutine(AnimationHandler());
    }

    private IEnumerator AnimationHandler()
    {
        yield return new WaitForEndOfFrame();

        // Ensures all animations begin playing in unison. Used for the sun oscillation system.
        animations[0].Play("Sun Loop State", 0);
        animations[1].Play("Bar Loop State", 0);

        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].speed = Speed;
        }

        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(MinimumPlayTime, MaximumPlayTime));

            // The direction is flipped approximately half of the time.
            if (Random.Range(0, 2) == 1) continue;

            yield return StartCoroutine(ReverseAnimations());
        }
    }

    private IEnumerator ReverseAnimations()
    {
        float normalisedProgress = 1 - animations[0].GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

        for (int i = 0; i < animations.Length; i++)
        {
           animations[i].Play(i == 0 ? "Sun Loop State" : "Bar Loop State", 0, normalisedProgress);
        }

        yield return this;
    }
}
