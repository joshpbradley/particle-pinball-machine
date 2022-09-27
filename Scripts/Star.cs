using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    // The minimum scale factor of the star.
    public float MinScale;
    // The maximum scale factor of the star.
    public float MaxScale;

    public float AnimationSpeed = 1f;

    public float StartAlpha = .3f;
    public float EndAlpha = .6f;

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;

    private float actualAnimationSpeed;

    private float randScale;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        actualAnimationSpeed = Random.Range(AnimationSpeed * .8f, AnimationSpeed * 1.2f);
    }

    private void Start()
    {
        _spriteRenderer.DOFade(EndAlpha, actualAnimationSpeed).From(StartAlpha).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).Goto(Random.Range(0, actualAnimationSpeed), true);

        // Randomise the scale of the star.
        randScale = Random.Range(MinScale, MaxScale);
        _transform.localScale = Vector3.one * randScale;
    }
}
