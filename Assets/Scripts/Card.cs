using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class Card : MonoBehaviour, IComparable<Card>
    {
        #region Public variables Region
        public bool m_IsCollected;
        #endregion

        #region Serialzed Variable Region
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform CardHolder;
        [SerializeField] private Image BackCardImage;
        [SerializeField] private Image FaceCardImage;

        #endregion

        #region Private Variable Region
        private int Id;
        private bool CardState;
        private RectTransform DiscardPile;
        #endregion

        #region Comparer Region
        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;
            return Id.CompareTo(other.Id);
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
            CardHolder.DOLocalRotate(Vector3.forward * 5f, 0.2f, RotateMode.Fast);
        }
        public void OnUnHover()
        {
            if (CardState) return;
            CardHolder.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast);
        }
        #endregion

        internal void Init(int _id, Sprite _face, RectTransform _discardPile)
        {
            Id = _id;
            FaceCardImage.sprite = _face;
            DiscardPile = _discardPile;
        }
        internal void DisableCard()
        {
            Id = -1;
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        internal void ShowCard()
        {
            CardState = true;
            CardHolder.DOLocalRotate(Vector3.up * 90f, 0.3f, RotateMode.Fast).OnComplete(() =>
            {
                BackCardImage.gameObject.SetActive(false);
                FaceCardImage.gameObject.SetActive(true);
                CardHolder.DOLocalRotate(Vector3.up * 180f, 0.3f, RotateMode.Fast).OnComplete(() =>
                {
                    GameManager.OnCardFlippedCallback?.Invoke(this);
                });
            });
        }

        internal void HideCard()
        {
            CardHolder.DOLocalRotate(Vector3.up * 90f, 0.3f, RotateMode.Fast).OnComplete(() =>
            {
                BackCardImage.gameObject.SetActive(true);
                FaceCardImage.gameObject.SetActive(false);
                CardHolder.DOLocalRotate(Vector3.zero, 0.3f, RotateMode.Fast).OnComplete(() =>
                {
                    CardState = false;
                });
            });
        }
        internal void CollectCard(float delay = 0f)
        {
            CardHolder.transform.SetParent(DiscardPile);

            CardHolder.DOLocalRotate(Vector3.forward * 360f, 0.5f, RotateMode.LocalAxisAdd).SetDelay(delay).SetEase(Ease.InBack);
            CardHolder.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InBack).SetDelay(delay).OnComplete(() =>
            {
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
                HideCard();
            });
            m_IsCollected = true;
        }

    }

}
