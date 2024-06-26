using System;
using System.Collections;
using UnityEngine;

namespace Davanci
{
    public struct LevelCompletedData
    {
        public string Grade;
        public int MovesCount;
        public int Time;
        public int ComboCount;
        public int MaxComboCount;
    }
    public class GameManager : SingletonMB<GameManager>
    {
        private const float DelayBetweenCards = 0.1f;
        private const int TimeToCheckForHint = 20;
        private const int TimeBetweenHints = 10;

        #region Callback Region
        internal static Action OnGameStartedCallback;
        internal static Action OnCardStartFlippingCallback;
        internal static Action<Card> OnCardFlippedCallback;
        internal static Action<int> OnTickCallback;
        internal static Action<int, bool> OnMoveCallback;
        internal static Action<int> OnCardMatchedCallback;
        internal static Action<int, int> OnComboCallback;
        internal static Action<GameSaveHolder> OnLevelLoadedCallback;
        internal static Action<bool> OnGamePausedCallback;
        internal static Action OnGameEndCallback;

        internal static Action<LevelCompletedData> OnLevelCompletedCallback;

        #endregion

        #region Private Variables Region

        private bool IsGameStarted;

        private float timeSinceLastTick = 0f;
        private int TimeSinceStarted;
        private int TimeSinceLastMatch;

        private float timeSinceLastHint;

        private int MatchCount;
        private int MovesCount;

        private int ComboCount;
        private int CurrentComboCount = 1;
        private int MaxComboCount;
        private bool IsCombo;
        private int ComboBonus;

        private Vector2Int LevelDimension;
        private int MatchCountNeeded;
        private bool ShowCardsAtStart;
        private LevelCompletedData LevelCompletedData;

        #endregion

        private void OnEnable()
        {
            OnCardFlippedCallback += OnCardFlipped;
            OnGameStartedCallback += OnGameStarted;
            OnGamePausedCallback += OnGamePaused;
            OnGameEndCallback += OnGameEnd;

            CardComparer.m_OnCardsCompared += OnCardsCompared;

        }
        private void Start()
        {
            Application.targetFrameRate = 60;

            CalculateMatchNeeded();

            ShowCardsAtStart = Database.GetShowCardsAtStart();

            //check if there's a save 
            if (GameSave.HasSave(out GameSaveHolder gameSaveHolder))
            {
                if (gameSaveHolder.cardSaves.Count > 0)
                {
                    ReloadDataFromSave(gameSaveHolder);
                    CardsGenerator.Instance.ReloadLevel(gameSaveHolder, LevelDimension);
                }
                else
                {
                    GameSave.DeleteSave();
                    OnGameEnd();
                    CardsGenerator.Instance.InitCardsGenerator(LevelDimension);
                }
            }
            else
            {
                CardsGenerator.Instance.InitCardsGenerator(LevelDimension);
            }
        }
        private void ReloadDataFromSave(GameSaveHolder gameSaveHolder)
        {
            TimeSinceStarted = gameSaveHolder.Time;
            ComboCount = gameSaveHolder.ComboCount;
            MatchCount = gameSaveHolder.MatchCount;
            MovesCount = gameSaveHolder.MovesCount;
            MaxComboCount = gameSaveHolder.MaxComboCount;

            OnLevelLoadedCallback?.Invoke(gameSaveHolder);
        }
        private void CalculateMatchNeeded()
        {
            if (LevelsManager.Instance != null)
            {
                LevelDimension = Database.GetLevelDimension(LevelsManager.Instance.m_CurrentLevel);
                MatchCountNeeded = (LevelDimension.x * LevelDimension.y) / 2;
            }
        }

        private void OnCardFlipped(Card newCard)
        {
            CardComparer.OnCardFlipped(newCard);
        }
        private void OnCardsCompared(bool _isMatch, Card _card1, Card _card2)
        {
            MovesCount++;

            if (_isMatch)
            {
                MatchCount++;

                OnCardMatchedCallback?.Invoke(MatchCount);

                _card1.CollectCard();
                _card2.CollectCard(DelayBetweenCards);

                GameSave.OnCardsUpdated(_card1.m_Index, _card2.m_Index);
                TimeSinceLastMatch = 0;
            }
            else
            {
                _card1.HideCard();
                _card2.HideCard();
                CurrentComboCount = 1;
                IsCombo = false;
            }
            OnMoveCallback?.Invoke(MovesCount, _isMatch);

            if (IsCombo)
            {
                ComboCount++;
                CurrentComboCount *= 2;
                ComboBonus += CurrentComboCount;

                MaxComboCount = MaxComboCount < CurrentComboCount ? CurrentComboCount : MaxComboCount;

                OnComboCallback?.Invoke(ComboCount, CurrentComboCount);
            }

            IsCombo = _isMatch;

            GameSave.OnDataChanged(MatchCount, MovesCount, TimeSinceStarted, ComboCount, MaxComboCount);

            CheckLevelCompleted();
        }
        public string CalculateScore()
        {
            int baseScore = 80;
            int comboCountBonus = ComboCount * 5;
            int movePenalty = (MovesCount - MatchCount) * 5;

            int timeNeeded = LevelDimension.x * LevelDimension.y * 2;

            int timePenalty = TimeSinceStarted > timeNeeded ? Mathf.FloorToInt((TimeSinceStarted - timeNeeded) / 10f) * 3 : -20;

            int totalScore = baseScore + comboCountBonus + ComboBonus + -movePenalty - timePenalty;

            return Database.CalculateGrade(totalScore);
        }
        private void CheckLevelCompleted()
        {
            if (MatchCount >= MatchCountNeeded)
            {
                LevelCompletedData.MovesCount = MovesCount;
                LevelCompletedData.Time = TimeSinceStarted;
                LevelCompletedData.Grade = CalculateScore();
                LevelCompletedData.ComboCount = ComboCount;
                LevelCompletedData.MaxComboCount = MaxComboCount;
                OnLevelCompletedCallback?.Invoke(LevelCompletedData);
                IsGameStarted = false;
                OnGameEnd();
            }
        }
        private void OnGamePaused(bool state) => IsGameStarted = !state;

        private void OnGameEnd()
        {
            TimeSinceStarted = 0;
            MatchCount = 0;
            MovesCount = 0;
            ComboCount = 0;
            MaxComboCount = 1;
            GameSave.DeleteSave();
        }
        private void OnGameStarted()
        {
            if (ShowCardsAtStart)
            {
                StartCoroutine(ShowCardsCoroutine());
            }
            else
            {
                IsGameStarted = true;
            }
        }
        private IEnumerator ShowCardsCoroutine()
        {
            //show cards 
            CardsGenerator.Instance.ShowCards();

            float cellcount = LevelDimension.x * LevelDimension.y;

            var waitTime = Mathf.Clamp(cellcount * 0.1f, 1f, 3f);

            yield return new WaitForSeconds(waitTime);
            //wait for hover time

            CardsGenerator.Instance.HideCards();
            IsGameStarted = true;
        }
        private void Update()
        {
            if (!IsGameStarted) return;

            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= 1f)
            {
                timeSinceLastTick -= 1f;
                TimeSinceStarted++;
                OnTickCallback?.Invoke(TimeSinceStarted);

                // Increment the time since the last match and hint
                TimeSinceLastMatch++;
                timeSinceLastHint++;

                // Check if it's time to activate a hint
                if (TimeSinceLastMatch >= TimeToCheckForHint && timeSinceLastHint >= TimeBetweenHints)
                {
                    ActivateHint();
                    timeSinceLastHint = 0; // Reset the time since the last hint
                }
            }
        }
        private void ActivateHint()
        {
            CardsGenerator.Instance.Hint();
        }
        private void OnDisable()
        {
            OnCardFlippedCallback -= OnCardFlipped;
            CardComparer.m_OnCardsCompared -= OnCardsCompared;
            OnGameStartedCallback -= OnGameStarted;
        }
    }
}
