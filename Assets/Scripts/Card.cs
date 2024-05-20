using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class Card : MonoBehaviour, IComparable<Card>
    {
        #region Public variables Region
        public bool m_IsCollected;
        public int m_Id;
        public int m_Index;
        #endregion

        #region Serialize Variables Region
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform CardHolder;
        [SerializeField] private Image BackCardImage;
        [SerializeField] private Image FaceCardImage;

        #endregion

        #region Private Variable Region

        private bool CardState;
        private RectTransform DiscardPile;
        #endregion

        #region Comparer Region
        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;
            return m_Id.CompareTo(other.m_Id);
        }
        #endregion

        #region UI Functions
        //function called from UI
        public void OnCardClicked()
        {
            if (CardState) return;
            ShowCard();
        }
        public void OnHover()
        {
            if (CardState) return;
            CardHolder.OnHover();
        }
        public void OnUnHover()
        {
            if (CardState) return;
            CardHolder.OnUnHover();
        }
        #endregion

        internal void Init(int _id, Sprite _face, RectTransform _endpoint, int index)
        {
            m_Id = _id;
            FaceCardImage.sprite = _face;
            DiscardPile = _endpoint;
            m_Index = index;
        }
        internal void Reload(int _id, Sprite _face, RectTransform _endpoint, int index, bool collected)
        {
            m_Id = _id;
            FaceCardImage.sprite = _face;
            DiscardPile = _endpoint;
            m_Index = index;
            m_IsCollected = collected;
            if (m_IsCollected)
            {
                CardHolder.transform.SetParent(DiscardPile);
                CardHolder.anchoredPosition = Vector2.zero;
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            }
        }
        internal void DisableCard(int index)
        {
            m_Id = -1;
            m_Index = index;
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        internal void ShowCard(bool invokeCallback = true)
        {
            CardState = true;
            if (m_IsCollected) return;

            CardHolder.localEulerAngles = Vector3.zero;

            if (invokeCallback)
                GameManager.OnCardStartFlippingCallback?.Invoke();

            StartCoroutine(FlipCardCoroutine(invokeCallback));
        }

        private IEnumerator FlipCardCoroutine(bool invokeCallback)
        {
            const float flipDuration = 0.3f;

            Vector3 originalRotation = CardHolder.localEulerAngles;
            yield return CardHolder.RotateAsync(Vector3.zero, 90f, flipDuration);

            BackCardImage.gameObject.SetActive(false);
            FaceCardImage.gameObject.SetActive(true);

            yield return CardHolder.RotateAsync(Vector3.up * 90f, -90f, flipDuration);

            // Invoke the callback if required
            if (invokeCallback)
                GameManager.OnCardFlippedCallback?.Invoke(this);
        }
        internal void HideCard()
        {
            StartCoroutine(HideCardCoroutine());
        }


        private IEnumerator HideCardCoroutine()
        {
            const float flipDuration = 0.3f;
            Vector3 originalRotation = CardHolder.localEulerAngles;

            yield return CardHolder.RotateAsync(originalRotation, 90f, flipDuration);

            BackCardImage.gameObject.SetActive(true);
            FaceCardImage.gameObject.SetActive(false);

            yield return CardHolder.RotateAsync(Vector3.up * 90f, -90f, flipDuration);

            CardState = false;
        }
        internal void CollectCard(float delay = 0f)
        {
            StartCoroutine(CollectCardCoroutine(delay));
        }

        private IEnumerator CollectCardCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            CardHolder.SetParent(DiscardPile);

            StartCoroutine(CardHolder.RotateAsync(360f, 0.5f));
            yield return CardHolder.MoveAsync(Vector2.zero, 0.5f);

            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            HideCard();

            m_IsCollected = true;
        }
        internal void Hint()
        {
            if (CardState) return;
            CardHolder.Vibrate(1f);
        }
        public CardSave CreateCardSave()
        {
            return new CardSave(m_Id, m_IsCollected);
        }
    }

}
