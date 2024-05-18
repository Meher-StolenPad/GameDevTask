using UnityEngine;
namespace Davanci
{
    [CreateAssetMenu(fileName = FileName, menuName = "Match Card Game/Levels")]
    public class Levels : ScriptableObject
    {
        private const string FileName = "Levels";

        private static Levels Instance;
        public static Levels m_Instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = Resources.Load<Levels>(FileName);
                }
                return Instance;
            }
        }
        public Vector2Int[] LevelsDimension;
    }
}