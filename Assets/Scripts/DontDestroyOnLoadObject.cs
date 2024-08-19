using UnityEngine;

public class DontDestroyOnLoadObject: MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
