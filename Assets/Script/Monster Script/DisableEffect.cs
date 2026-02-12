using UnityEngine;
using System.Collections;

public class DisableEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    SpriteRenderer sr;
    Color baseColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    public void Play(float disableTime)
    {
        StartCoroutine(EffectRoutine(disableTime));
    }

    IEnumerator EffectRoutine(float disableTime)
    {
        // 무력화 시간 동안 유지
        yield return new WaitForSeconds(disableTime);

        // 서서히 사라지기
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            yield return null;
        }

        Destroy(gameObject);
    }
}