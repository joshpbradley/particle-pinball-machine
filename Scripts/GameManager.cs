using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Awake()
    {
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
    }
}