using System;
using UnityEngine;

namespace Davanci
{
    public class GameManager : SingletonMB<GameManager>
    {
        private const float DelayBetweenCards = 0.1f;

        #region Callback Region
        internal static Action OnGameStartedCallback;
        internal static Action<Card> OnCardFlippedCallback;
        internal static Action<int> OnTick;
        internal static Action<int, bool> OnMoveCallback;
        internal static Action<int> OnCardMatchedCallback;

        #endregion

        #region Private Variables Region

        private bool IsGameStarted;

        private float timeSinceLastTick = 0f;
        private int TimeSinceStarted;

        private int MatchCount;
        private int MovesCount;

        private Vector2Int LevelDimension;
        private int MatchCountNeeded;
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
            }
            OnMoveCallback?.Invoke(MovesCount, _isMatch);
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
                OnTick?.Invoke(TimeSinceStarted);
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
