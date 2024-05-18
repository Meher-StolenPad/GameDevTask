using DG.Tweening;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class Card : MonoBehaviour, IComparable<Card>
    {
        public bool m_IsCollected;
            
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform CardHolder;
        [SerializeField] private Image BackCardImage;
        [SerializeField] private Image FaceCardImage;

        private int Id;
        private bool CardState;
        private RectTransform DiscardPile;  

        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;
            return Id.CompareTo(other.Id);
        }
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
        //function called from UI
        public void OnCardClicked()
        {
            if (CardState) return;
            ShowCard();
        }
        public void ShowCard()
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
