using UnityEngine;
using FMODUnity;

public class Terminal : MonoBehaviour
{
    [SerializeField] private EventReference keyboardEvent;

    public void OnKeyDown()
    {
        RuntimeManager.PlayOneShot(keyboardEvent);
    }
}
