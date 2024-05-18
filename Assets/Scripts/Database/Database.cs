using UnityEngine;

namespace Davanci
{
    public static class Database
    {
        private static Sprite[] NewSprites;

        public static Sprite[] GetSprites(int cellsize)
        {
            if (cellsize == 0) return null;

            if (cellsize / 2 >= Data.m_Instance.Sprites.Length)
            {
                Debug.LogWarning("You need more sprites");
                return null;
            }
            NewSprites = new Sprite[cellsize / 2];

            for (int i = 0; i < NewSprites.Length; i++)
            {
                NewSprites[i] = Data.m_Instance.Sprites[i];
            }
            return NewSprites;
        }   
    }

}
