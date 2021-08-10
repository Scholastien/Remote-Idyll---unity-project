using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObscuringItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeInRoutine()
    {
        Color c = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
        float currentAlpha = spriteRenderer.color.a;
        float distance = 1 - currentAlpha;

        while (1f - currentAlpha > 0.01f)
        {
            currentAlpha = currentAlpha + distance / Settings.fadeOutSeconds * Time.deltaTime;
            spriteRenderer.color = new Color(c.r, c.g, c.b, currentAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator FadeOutRoutine()
    {
        Color c = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
        float currentAlpha = spriteRenderer.color.a;
        float distance = currentAlpha - Settings.targetAlpha;

        while (currentAlpha - Settings.targetAlpha > 0.01f)
        {
            currentAlpha = currentAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime;
            spriteRenderer.color = new Color(c.r, c.g, c.b, currentAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(c.r, c.g, c.b, Settings.targetAlpha);
    }
}
