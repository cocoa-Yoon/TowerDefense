using UnityEngine;
using System.Collections;

public class DisableEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    SpriteRenderer sr;
    Color baseColor;
    Coroutine currentRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    public void Play(float disableTime)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(EffectRoutine(disableTime));
    }

    IEnumerator EffectRoutine(float disableTime)
    {
        // 1. 무력화 시간 대기
        yield return new WaitForSeconds(disableTime);

        // 2. 페이드 아웃
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            // Color 구조체를 새로 생성하지 않고 직접 수정 (최적화)
            Color tempColor = sr.color;
            tempColor.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = tempColor;
            yield return null;
        }

        // 3. 확실하게 투명하게 만든 후 파괴
        sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        Destroy(gameObject);
    }
}