using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scores")]
        [SerializeField] private TextMeshProUGUI TimeText;
        [SerializeField] private TextMeshProUGUI MovesCountText;
        [SerializeField] private TextMeshProUGUI MatchCountText;
        [SerializeField] private TextMeshProUGUI ComboText;
        [SerializeField] private TextMeshProUGUI ComboTextAnimation;
        [SerializeField] private Animator ComboTextAnimator;
        [Header("Start Panel")]
        [SerializeField] private CanvasGroup TapToStartPanel;   

        [Header("Level Completed Panel")]
        [SerializeField] private CanvasGroup LevelCompletedCanvasGroup;
        [SerializeField] private TextMeshProUGUI TotalMovesCountText;
        [SerializeField] private TextMeshProUGUI TotalTimeText;
        [SerializeField] private TextMeshProUGUI ComboBonus;
        [SerializeField] private TextMeshProUGUI MaxComboText;
        [SerializeField] private TextMeshProUGUI GradeText;
        [SerializeField] private CanvasGroup LevelCompletedButtonsHolder;
            
        [Header("Pause Panel")]
        [SerializeField] private CanvasGroup PausePanel;
        [SerializeField] private Image EnabledSoundImage;
        [SerializeField] private Image DisabledSoundImage;
        private bool SoundState = true;

        private void Start()
        {
            GameManager.OnTickCallback += OnTick;
            GameManager.OnCardMatchedCallback += OnCardMatched;
            GameManager.OnMoveCallback += OnCardMoved;
            GameManager.OnLevelCompletedCallback += OnLevelCompleted;
            GameManager.OnComboCallback += OnCombo;
            GameManager.OnLevelLoadedCallback += OnLevelLoaded;
        }
        private void OnDestroy()
        {
            GameManager.OnTickCallback -= OnTick;
            GameManager.OnCardMatchedCallback -= OnCardMatched;
            GameManager.OnMoveCallback -= OnCardMoved;
            GameManager.OnLevelCompletedCallback -= OnLevelCompleted;
            GameManager.OnLevelLoadedCallback -= OnLevelLoaded;
            GameManager.OnComboCallback -= OnCombo;
        }
        private void OnDisable()
        {
          
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
        private void OnCombo(int count, int currentComboCount)
        {
            ComboText.SetTextAnimated(count.ToString());

            ComboTextAnimation.text = "X" + currentComboCount.ToString();
            ComboTextAnimator.SetTrigger("StartAnimation");
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

            yield return ComboBonus.SetAnimatedRishText("X", levelCompletedData.ComboCount, 0.5f);

            yield return MaxComboText.SetAnimatedRishText("X", levelCompletedData.MaxComboCount, 0.5f);

            GradeText.text = levelCompletedData.Grade;

            yield return GradeText.BounceAsync(0.3f, false);

            yield return LevelCompletedButtonsHolder.FadeIn(.3f);
        }
        private void OnLevelLoaded(GameSaveHolder gameSaveHolder)
        {
            MovesCountText.text = gameSaveHolder.MovesCount.ToString();
            MatchCountText.text = gameSaveHolder.MatchCount.ToString();
            ComboText.text = gameSaveHolder.ComboCount.ToString();
            SetTime(gameSaveHolder.Time);
        }
        public void OnSoundButtonClicked(bool _state)
        {
            SoundState = _state;
            if(SoundState)
            {
                EnabledSoundImage.gameObject.SetActive(true);
                DisabledSoundImage.gameObject.SetActive(false);
            }
            else
            {
                EnabledSoundImage.gameObject.SetActive(false);
                DisabledSoundImage.gameObject.SetActive(true);
            }   
            SoundManager.OnSoundSateChangedCallback?.Invoke(SoundState);
        }
        public void OnPauseClicked()
        {
            GameManager.OnGamePausedCallback(true);
            SoundState = SoundManager.SoundActivated;
            if (SoundState)
            {
                EnabledSoundImage.gameObject.SetActive(true);
                DisabledSoundImage.gameObject.SetActive(false);
            }
            else
            {
                EnabledSoundImage.gameObject.SetActive(false);
                DisabledSoundImage.gameObject.SetActive(true);
            }
            StartCoroutine(PausePanel.FadeIn(0.3f));
        }
        public void OnResumeClicked()
        {
            GameManager.OnGamePausedCallback(false);
            StartCoroutine(PausePanel.FadeOut(0.3f));
        }
        public void OnRestartClicked()
        {
            LevelsManager.Instance.OnRestartButtonClicked();
            GameManager.OnGameEndCallback?.Invoke();
        }
        public void OnNextLevelClicked()
        {
            LevelsManager.Instance.OnNextLevelButtonClicked();
        }
        public void OnBackToLevelMenuClicked()
        {
            LevelsManager.Instance.OnBackToLevelMenuClicked();
            GameManager.OnGameEndCallback?.Invoke();
        }

    }

}
