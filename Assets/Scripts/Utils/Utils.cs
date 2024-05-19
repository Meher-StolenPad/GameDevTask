using DG.Tweening;
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
            if (_textMeshProUGUI != null)
                _textMeshProUGUI.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.3f);
        }
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, float duration)
        {
            yield return canvasGroup.DOFade(1f, duration).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }).WaitForCompletion();
        }
        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, float duration)
        {
            yield return canvasGroup.DOFade(0f, duration).OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }).WaitForCompletion();
        }
        public static void OnHover(this RectTransform _rectTransform)
        {
            _rectTransform.DOLocalRotate(Vector3.forward * 5f, 0.2f, RotateMode.Fast);
        }
        public static void OnUnHover(this RectTransform _rectTransform)
        {
            _rectTransform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast);
        }
        public static void Vibrate(this Transform _transform, float duration,float strength = 5f)
        {
            // Create a sequence
            Sequence sequence = DOTween.Sequence();

            // Add the vibration tweens to the sequence
            sequence.Append(_transform.DOLocalRotate(Vector3.forward * strength, 0.1f, RotateMode.Fast).SetEase(Ease.InOutSine));
            sequence.Append(_transform.DOLocalRotate(Vector3.forward * -strength, 0.1f, RotateMode.Fast).SetEase(Ease.InOutSine));

            // Set the sequence to loop for the specified duration
            sequence.SetLoops((int)(duration / 0.2f), LoopType.Yoyo);
        }
        public static IEnumerator FadeIn(this TextMeshProUGUI _textMeshProUGUI, float duration)
        {
            yield return _textMeshProUGUI.DOFade(1f, duration).WaitForCompletion();
        }
        public static IEnumerator FadeOut(this TextMeshProUGUI _textMeshProUGUI, float duration)
        {
            yield return _textMeshProUGUI.DOFade(1f, duration).WaitForCompletion();
        }
        public static IEnumerator SetAnimatedTimeText(this TextMeshProUGUI _textMeshProUGUI, int time, float duration)
        {
            yield return _textMeshProUGUI.FadeIn(0.3f);

            _textMeshProUGUI.text = "00:00";

            Sequence sequence = DOTween.Sequence();

            int startTime = 0;
            int endTime = time;

            sequence.Append(DOTween.To(() => startTime, x =>
            {
                startTime = x;
                int minutes = startTime / 60;
                int seconds = startTime % 60;
                _textMeshProUGUI.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }, endTime, 0.5f).SetEase(Ease.Linear));

            yield return sequence.WaitForCompletion();
        }
        public static IEnumerator SetAnimatedIntText(this TextMeshProUGUI _textMeshProUGUI, int endValue, float duration)
        {
            yield return _textMeshProUGUI.FadeIn(0.3f);

            _textMeshProUGUI.text = "0";

            int startValue = 0;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => 0, x =>
            {
                startValue = x;
                _textMeshProUGUI.text = startValue.ToString();
            }, endValue, duration).SetEase(Ease.Linear));

            yield return sequence.WaitForCompletion();
        }
        public static IEnumerator BounceAsync(this TextMeshProUGUI _textMeshProUGUI, float duration, bool fadeOut)
        {
            _textMeshProUGUI.DOFade(1f, duration);

            yield return _textMeshProUGUI.transform.DOScale(1.5f, duration).OnComplete(() =>
            {
                _textMeshProUGUI.transform.DOScale(1f, duration).OnComplete(() =>
                {
                    if (fadeOut)
                    {
                        _textMeshProUGUI.DOFade(0f, duration);
                        _textMeshProUGUI.transform.DOScale(0f, duration);
                    }
                });
            }).WaitForCompletion();
        }
        public static void Bounce(this TextMeshProUGUI textMeshProUGUI, float duration, bool fadeOut)
        {
            textMeshProUGUI.DOFade(1f, duration);

            textMeshProUGUI.transform.DOScale(1.5f, duration).OnComplete(() =>
            {
                textMeshProUGUI.transform.DOScale(1f, duration).OnComplete(() =>
                {
                    if (fadeOut)
                    {
                        textMeshProUGUI.DOFade(0f, duration);
                        textMeshProUGUI.transform.DOScale(0f, duration);
                    }
                });
            });
        }
        public static IEnumerator SetAnimatedRishText(this TextMeshProUGUI _textMeshProUGUI, string stringToAttach, int endValue, float duration)
        {
            yield return _textMeshProUGUI.FadeIn(0.3f);

            _textMeshProUGUI.text = stringToAttach + "1";

            int startValue = 0;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => 0, x =>
            {
                startValue = x;
                _textMeshProUGUI.text = stringToAttach + startValue.ToString();
            }, endValue, duration).SetEase(Ease.Linear));

            yield return sequence.WaitForCompletion();
        }
    }
}
