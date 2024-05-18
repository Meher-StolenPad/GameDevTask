using DG.Tweening;
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
        public static void SetTextAnimated(this TextMeshProUGUI textMeshProUGUI, string text)
        {
            textMeshProUGUI.text = text;
            textMeshProUGUI.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f);
        }
    }
}
