using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Davanci
{

    [CreateAssetMenu(fileName = FileName, menuName = "Match Card Game/Data")]
    public class Data : ScriptableObject
    {
        private const string FileName = "Data";

        private static Data Instance;
        public static Data m_Instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = Resources.Load<Data>(FileName);
                }
                return Instance;
            }
        }
        public Sprite[] Sprites;
    }
}

