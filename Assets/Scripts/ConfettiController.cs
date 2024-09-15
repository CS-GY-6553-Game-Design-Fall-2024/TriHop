using UnityEngine;

public class ConfettiOnCanvasActive : MonoBehaviour
{
    public ParticleSystem confettiSystem;
    void OnEnable()
    {
        if (confettiSystem != null)
        {
            confettiSystem.gameObject.SetActive(true);
            confettiSystem.Play();
        }
    }
    void OnDisable() {
        if (confettiSystem != null) {
            confettiSystem.Stop();
            confettiSystem.gameObject.SetActive(false);
        }
    }
}
