using UnityEngine;

namespace Davanci
{
    public static class Database
    {
        private static Sprite[] NewSprites;
        public static int LevelsMenuSceneIndex = 0;
        public static int GameSceneIndex = 1;

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
        public static Vector2Int[] GetLevels()
        {
            return Levels.m_Instance.LevelsDimension;
        }
        public static Vector2Int GetLevelDimension(int level)
        {
            if (level > Levels.m_Instance.LevelsDimension.Length) return Vector2Int.zero;

            return Levels.m_Instance.LevelsDimension[level];
        }
        internal static bool HasLevel(int level)
        {
            return level < Levels.m_Instance.LevelsDimension.Length;
        }
    }

}
