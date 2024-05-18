using System;
using UnityEngine;

namespace Davanci
{
    public class GameManager : SingletonMB<GameManager>
    {
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
                Debug.Log("Match !");
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
