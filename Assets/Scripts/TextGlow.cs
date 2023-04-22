using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextGlow : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().DOFade(.5f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
