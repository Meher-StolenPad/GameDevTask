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
    }
}
