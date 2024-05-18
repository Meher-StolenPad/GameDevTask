using UnityEngine;

namespace Davanci
{
    public static class Database
    {
        private static Sprite[] newSprites;

        public static Sprite[] GetSprites(int cellsize)
        {
            if (cellsize == 0) return null;

            if (cellsize / 2 >= Data.Instance.Sprites.Length)
            {
                Debug.LogWarning("You need more sprites");
                return null;
            }
            newSprites = new Sprite[cellsize / 2];

            for (int i = 0; i < newSprites.Length; i++)
            {
                newSprites[i] = Data.Instance.Sprites[i];
            }
            return newSprites;
        }
    }

}
