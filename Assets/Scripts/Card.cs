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
        [SerializeField] private Image BackCard;
        [SerializeField] private Image FaceCard;

        private int Id;

        internal void Init(int _id, Sprite _face)
        {
            Id = _id;
            FaceCard.sprite = _face;
        }
    }

}
