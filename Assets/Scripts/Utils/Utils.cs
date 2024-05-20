using System.Collections;
using TMPro;
using UnityEngine;

namespace Davanci
{
    public static class Utils
    {
        public static void Shuffle<T>(this T[] _array)
        {
            for (int i = _array.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                T temp = _array[i];
                _array[i] = _array[randomIndex];
                _array[randomIndex] = temp;
            }
        }
        public static void SetTimeText(this TextMeshProUGUI _textMeshProUGUI, int _time)
        {
            var minutes = _time / 60;
            var seconds = _time % 60;

            _textMeshProUGUI.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        public static void SetTextAnimated(this TextMeshProUGUI _textMeshProUGUI, string _text)
        {
            _textMeshProUGUI.text = _text;
            _textMeshProUGUI.StartCoroutine(PunchScaleCoroutine(_textMeshProUGUI.rectTransform));
        }
        private static IEnumerator PunchScaleCoroutine(RectTransform rectTransform)
        {
            Vector3 originalScale = rectTransform.localScale;
            Vector3 targetScale = originalScale + Vector3.one * 0.3f;

            float elapsedTime = 0f;
            float duration = 0.3f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0, 1, t));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rectTransform.localScale = originalScale;
        }
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, float duration)
        {
            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = 1f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, float duration)
        {
            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        public static void OnHover(this RectTransform rectTransform)
        {
            CoroutineHandler.Instance.StartCoroutine(HoverCoroutine(rectTransform, Vector3.forward * 5f, 0.2f));
        }

        public static void OnUnHover(this RectTransform rectTransform)
        {
            CoroutineHandler.Instance.StartCoroutine(HoverCoroutine(rectTransform, Vector3.zero, 0.2f));
        }

        private static IEnumerator HoverCoroutine(RectTransform _rectTransform, Vector3 targetRotation, float duration)
        {
            Quaternion startRotation = _rectTransform.localRotation;
            Quaternion endRotation = Quaternion.Euler(targetRotation);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (_rectTransform == null)
                    break;

                _rectTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (_rectTransform != null)
                _rectTransform.localRotation = endRotation;
        }
        public static void Vibrate(this Transform transform, float duration, float strength = 5f)
        {
            CoroutineHandler.Instance.StartCoroutine(VibrateCoroutine(transform, duration, strength));
        }

        private static IEnumerator VibrateCoroutine(Transform transform, float duration, float strength)
        {
            float elapsedTime = 0f;
            Vector3 originalRotation = transform.localRotation.eulerAngles;

            while (elapsedTime < duration)
            {
                // Rotate forward
                transform.localRotation = Quaternion.Euler(originalRotation + Vector3.forward * strength * Mathf.Sin(elapsedTime * Mathf.PI * 2f));
                yield return null;

                elapsedTime += Time.deltaTime;
            }

            // Ensure it finishes at rotation 0
            transform.localRotation = Quaternion.Euler(originalRotation);
        }

        public static IEnumerator FadeIn(this TextMeshProUGUI textMeshProUGUI, float duration)
        {
            float elapsedTime = 0f;
            float startAlpha = textMeshProUGUI.alpha;
            float targetAlpha = 1f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                textMeshProUGUI.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.alpha = targetAlpha;
        }
        public static IEnumerator FadeOut(this TextMeshProUGUI textMeshProUGUI, float duration)
        {
            float elapsedTime = 0f;
            float startAlpha = textMeshProUGUI.alpha;
            float targetAlpha = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                textMeshProUGUI.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.alpha = targetAlpha;
        }
        public static IEnumerator ScaleOverTime(this TextMeshProUGUI textMeshProUGUI, Vector3 targetScale, float duration)
        {
            Vector3 startScale = textMeshProUGUI.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                textMeshProUGUI.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.transform.localScale = targetScale;
        }
        public static IEnumerator SetAnimatedTimeText(this TextMeshProUGUI textMeshProUGUI, int time, float duration)
        {
            yield return textMeshProUGUI.FadeIn(0.3f);

            textMeshProUGUI.text = "00:00";

            int startTime = 0;
            int endTime = time;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                startTime = Mathf.RoundToInt(Mathf.Lerp(0, endTime, t));
                int minutes = startTime / 60;
                int seconds = startTime % 60;
                textMeshProUGUI.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            int finalMinutes = endTime / 60;
            int finalSeconds = endTime % 60;
            textMeshProUGUI.text = string.Format("{0:D2}:{1:D2}", finalMinutes, finalSeconds);
        }
        public static IEnumerator SetAnimatedIntText(this TextMeshProUGUI textMeshProUGUI, int endValue, float duration)
        {
            yield return textMeshProUGUI.FadeIn(0.3f);

            textMeshProUGUI.text = "0";

            int startValue = 0;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                startValue = Mathf.RoundToInt(Mathf.Lerp(0, endValue, t));
                textMeshProUGUI.text = startValue.ToString();
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.text = endValue.ToString();
        }
        public static IEnumerator BounceAsync(this TextMeshProUGUI textMeshProUGUI, float duration, bool fadeOut)
        {
            yield return textMeshProUGUI.FadeIn(duration);

            yield return textMeshProUGUI.ScaleOverTime(Vector3.one * 1.5f, duration);
            yield return textMeshProUGUI.ScaleOverTime(Vector3.one, duration);

            if (fadeOut)
            {
                yield return textMeshProUGUI.FadeOut(duration);
            }
        }
        public static IEnumerator SetAnimatedRishText(this TextMeshProUGUI textMeshProUGUI, string stringToAttach, int endValue, float duration)
        {
            yield return textMeshProUGUI.FadeIn(0.3f);

            textMeshProUGUI.text = stringToAttach + "1";

            int startValue = 0;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                startValue = Mathf.RoundToInt(Mathf.Lerp(0, endValue, t));
                textMeshProUGUI.text = stringToAttach + startValue.ToString();
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.text = stringToAttach + endValue.ToString();
        }
        public static IEnumerator RotateAsync(this Transform transform, Vector3 startRotation, float targetRotationY, float duration)
        {
            float elapsedTime = 0f;
            Quaternion startRotationQuat = Quaternion.Euler(startRotation);
            Quaternion targetRotationQuat = Quaternion.Euler(startRotation + Vector3.up * targetRotationY);

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.rotation = Quaternion.Slerp(startRotationQuat, targetRotationQuat, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotationQuat;
        }
        public static IEnumerator RotateAsync(this RectTransform rectTransform, float targetRotationZ, float duration)
        {
            float elapsedTime = 0f;
            float startRotationZ = rectTransform.localRotation.eulerAngles.z;
            float rotationSpeed = Mathf.Abs(targetRotationZ - startRotationZ) / duration;

            while (elapsedTime < duration)
            {
                float rotationZ = Mathf.Lerp(startRotationZ, targetRotationZ, elapsedTime / duration);
                rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.localRotation = Quaternion.Euler(0f, 0f, targetRotationZ);
        }

        public static IEnumerator MoveAsync(this RectTransform rectTransform, Vector2 targetPosition, float duration)
        {
            Vector2 originalPosition = rectTransform.anchoredPosition;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = targetPosition;
        }
    }
}
