using UnityEngine;

public class ConfettiOnCanvasActive : MonoBehaviour
{
    public ParticleSystem confettiSystem;
    void OnEnable()
    {
        if (confettiSystem != null)
        {
            confettiSystem.Play();
        }
    }
}
