using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public RectTransform progressBarBackground;
    public RectTransform progressBarFill;
    public float gameDuration = 180f;

    private float elapsedTime = 0f;

    void Start()
    {
        progressBarFill.anchoredPosition = new Vector2(progressBarFill.anchoredPosition.x, -progressBarBackground.rect.height / 2);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedTime = Mathf.Clamp(elapsedTime, 0, gameDuration);

        float progress = elapsedTime / gameDuration;
        float backgroundHeight = progressBarBackground.rect.height;
        float startY = -100f;
        float endY = 100f;
        float newYPosition = Mathf.Lerp(startY, endY, progress);

        progressBarFill.anchoredPosition = new Vector2(progressBarFill.anchoredPosition.x, newYPosition);
    }
}
