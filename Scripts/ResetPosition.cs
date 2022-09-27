using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private void Start()
    {
        GetComponent<RectTransform>().position = Vector3.zero;
    }

    private void Reset()
    {
        GetComponent<RectTransform>().position = Vector3.zero;
    }
}