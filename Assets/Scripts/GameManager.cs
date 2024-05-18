using System;
using System.Text.RegularExpressions;
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

        #region Callback Region
        internal static Action OnGameStartedCallback;
        internal static Action<Card> OnCardFlippedCallback;
        internal static Action<int> OnTickCallback;
        internal static Action<int, bool> OnMoveCallback;
        internal static Action<int> OnCardMatchedCallback;
        internal static Action<int, int> OnComboCallback;

        internal static Action<LevelCompletedData> OnLevelCompletedCallback;

        #endregion

        #region Private Variables Region

        private bool IsGameStarted;

        private float timeSinceLastTick = 0f;
        private int TimeSinceStarted;

        private int MatchCount;
        private int MovesCount;

        private int ComboCount;
        private int CurrentComboCount = 1;
        private int MaxComboCount;
        private bool IsCombo;
        private int ComboBonus;

        private Vector2Int LevelDimension;
        private int MatchCountNeeded;

        private LevelCompletedData LevelCompletedData;

        #endregion

        private void OnEnable()
        {
            OnCardFlippedCallback += OnCardFlipped;
            OnGameStartedCallback += OnGameStarted;

            CardComparer.m_OnCardsCompared += OnCardsCompared;

        }
        private void Start()
        {
            CalculateMatchNeeded();
            CardsGenerator.Instance.InitCardsGenerator(LevelDimension);
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
                OnGameStopped();
            }
        }
        private void OnGameStopped()
        {
            TimeSinceStarted = 0;
            MatchCount = 0;
            MovesCount = 0;
            ComboCount = 0;
            MaxComboCount = 1;
        }
        private void OnGameStarted()
        {
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
            }
        }
        private void OnDisable()
        {
            OnCardFlippedCallback -= OnCardFlipped;
            CardComparer.m_OnCardsCompared -= OnCardsCompared;
            OnGameStartedCallback -= OnGameStarted;
        }
    }
}
