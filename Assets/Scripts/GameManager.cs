using System;
using UnityEngine;

namespace Davanci
{
    public class GameManager : SingletonMB<GameManager>
    {
        private const float DelayBetweenCards = 0.1f;

        internal static Action<Card> OnCardFlippedCallback;

        private void OnEnable()
        {
            OnCardFlippedCallback += OnCardFlipped;
            CardComparer.m_OnCardsCompared += OnCardsCompared;
        }

        private void OnCardFlipped(Card newCard)
        {
            CardComparer.OnCardFlipped(newCard);
        }
        private void OnCardsCompared(bool _isMatch, Card _card1, Card _card2)
        {
            if (_isMatch)
            {
                _card1.CollectCard();
                _card2.CollectCard(DelayBetweenCards);
            }
            else
            {
                _card1.HideCard();
                _card2.HideCard();
            }
        }
        private void OnDisable()
        {
            OnCardFlippedCallback -= OnCardFlipped;
            CardComparer.m_OnCardsCompared -= OnCardsCompared;
        }
    }
}
