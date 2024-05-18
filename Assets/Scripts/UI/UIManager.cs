using TMPro;
using UnityEngine;

namespace Davanci
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scores")]
        [SerializeField] private TextMeshProUGUI TimeText;
        [SerializeField] private TextMeshProUGUI MovesCountText;
        [SerializeField] private TextMeshProUGUI MatchCountText;

        [Header("Tap to start Panel")]
        [SerializeField] private CanvasGroup TapToStartPanel;

        private void Start()
        {
            GameManager.OnTick += OnTick;
            GameManager.OnCardMatchedCallback += OnCardMatched;
            GameManager.OnMoveCallback += OnCardMoved;
        }

        private void OnCardMoved(int count, bool match)
        {
            MovesCountText.SetTextAnimated(count.ToString());
        }

        private void OnCardMatched(int count)
        {
            MatchCountText.SetTextAnimated(count.ToString());
        }

        private void OnTick(int time)
        {
            SetTime(time);
        }
        private void SetTime(int time)
        {
            TimeText.SetTimeText(time);
        }
        public void OnTapToStartClicked()
        {
            StartCoroutine(TapToStartPanel.FadeOut(0.3f));
            GameManager.OnGameStartedCallback?.Invoke();
        }
        private void OnDisable()
        {
            GameManager.OnTick -= OnTick;
        }
    }

}
