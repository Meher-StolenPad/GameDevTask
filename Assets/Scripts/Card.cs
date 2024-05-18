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
        [SerializeField] private Image BackCard;
        [SerializeField] private Image FaceCard;

        private int id;
            
        internal void Init(int _id, Sprite _face)
        {
            id = _id;
            FaceCard.sprite = _face;
        }
        internal void DisableCard()
        {
            id = -1;
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }

}
