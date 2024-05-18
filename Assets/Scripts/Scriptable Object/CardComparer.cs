using System;

namespace Davanci
{
    public static class CardComparer
    {
        public static Action<bool, Card, Card> m_OnCardsCompared;

        private static Card PreviousCard;

        internal static void OnCardFlipped(Card newCard)
        {
            if (PreviousCard != null)
            {
                CompareCards(newCard, PreviousCard);
                return;
            }

            // Update the previous card to the newly flipped card
            PreviousCard = newCard;
        }
        internal static void CompareCards(Card card1, Card card2)
        {
            bool isMatch;

            lock (PreviousCard)
            {
                isMatch = card1.CompareTo(card2) == 0;
            }
            PreviousCard = null;

            m_OnCardsCompared?.Invoke(isMatch, card1, card2);
        }
    }
}