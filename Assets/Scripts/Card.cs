using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private RectTransform CardHolder;
        [SerializeField] private Image BackCardImage;
        [SerializeField] private Image FaceCardImage;

        private int Id;
        private bool CardState;

        internal void Init(int _id, Sprite _face)
        {
            Id = _id;
            FaceCardImage.sprite = _face;
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
                    HideCard();
                });
            });
        }
        internal void HideCard()
        {
            CardHolder.DOLocalRotate(Vector3.up * 90f, 0.3f, RotateMode.Fast).SetDelay(2f).OnComplete(() =>
            {
                BackCardImage.gameObject.SetActive(true);
                FaceCardImage.gameObject.SetActive(false);
                CardHolder.DOLocalRotate(Vector3.zero, 0.3f, RotateMode.Fast).OnComplete(() =>
                {
                    CardState = false;
                });
            });
        }
    }

}
