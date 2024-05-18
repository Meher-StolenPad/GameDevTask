using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Davanci
{

    [CreateAssetMenu(fileName = FileName, menuName = "Match Card Game/Data")]
    public class Data : ScriptableObject
    {
        private const string FileName = "Data";

        private static Data instance;
        public static Data Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<Data>(FileName);
                }
                return instance;
            }
        }
        public Sprite[] Sprites;
    }
}

