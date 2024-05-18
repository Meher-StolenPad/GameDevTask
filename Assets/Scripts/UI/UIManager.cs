using System.Collections;
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

        [Header("Start Panel")]
        [SerializeField] private CanvasGroup TapToStartPanel;

        [Header("Level Completed Panel")]
        [SerializeField] private CanvasGroup LevelCompletedCanvasGroup;
        [SerializeField] private TextMeshProUGUI TotalMovesCountText;
        [SerializeField] private TextMeshProUGUI TotalTimeText;
        [SerializeField] private TextMeshProUGUI GradeText;

        private void Start()
        {
            GameManager.OnTickCallback += OnTick;
            GameManager.OnCardMatchedCallback += OnCardMatched;
            GameManager.OnMoveCallback += OnCardMoved;
            GameManager.OnLevelCompletedCallback += OnLevelCompleted;
        }
        public void OnTapToStartClicked()
        {
            StartCoroutine(TapToStartPanel.FadeOut(0.3f));
            GameManager.OnGameStartedCallback?.Invoke();
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

        private void OnLevelCompleted(LevelCompletedData levelCompletedData)
        {
            StartCoroutine(LevelCompletedCoroutine(levelCompletedData));
        }
        private IEnumerator LevelCompletedCoroutine(LevelCompletedData levelCompletedData)
        {
            yield return new WaitForSeconds(0.5f);

            yield return LevelCompletedCanvasGroup.FadeIn(0.3f);

            yield return TotalTimeText.SetAnimatedTimeText(levelCompletedData.Time, 0.5f);

            yield return TotalMovesCountText.SetAnimatedIntText(levelCompletedData.MovesCount, 0.5f);

            GradeText.text = levelCompletedData.Grade;

            yield return GradeText.BounceAsync(0.3f, false);
        }
        private void OnDisable()
        {
            GameManager.OnTickCallback -= OnTick;
            GameManager.OnCardMatchedCallback -= OnCardMatched;
            GameManager.OnMoveCallback -= OnCardMoved;
            GameManager.OnLevelCompletedCallback -= OnLevelCompleted;
        }
    }

}
