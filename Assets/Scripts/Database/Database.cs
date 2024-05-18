using System.Collections.Generic;
using UnityEngine;

namespace Davanci
{
    public static class Database
    {
        public static int m_LevelsMenuSceneIndex = 0;
        public static int m_GameSceneIndex = 1;

        private static Sprite[] NewSprites;
        private static readonly Dictionary<int, string> GradeThresholds = new Dictionary<int, string>
        {
            { 90, "S" },
            { 75, "A" },
            { 50, "B" },
            { 30, "C" },
            { 10, "D" },
            { 0,  "F" }
        };
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
        public static string CalculateGrade(int score)
        {
            foreach (var threshold in GradeThresholds)
            {
                if (score >= threshold.Key)
                {
                    return threshold.Value;
                }
            }
            return "F";
        }
        internal static AudioClip GetAudioClip(ClipFx clipType)
        {
            AudioClip audioClip = null;
            switch (clipType)
            {
                case ClipFx.CardFlipped:
                    audioClip = Data.m_Instance.CardFlipClip;
                    break;
                case ClipFx.CardMatch:
                    audioClip = Data.m_Instance.MatchClip;
                    break;
                case ClipFx.CardUnMatch:
                    audioClip = Data.m_Instance.UnmatchClip;
                    break;
                case ClipFx.LevelCompleted:
                    audioClip = Data.m_Instance.LevelCompletedClip;
                    break;
            }
            return audioClip;
        }
    }

}
