using System;

namespace Davanci
{
    public class LevelsManager : SingletonMB<LevelsManager>
    {
        public static Action<int> m_OnSceneLoadedCallback;
        public int m_CurrentLevel;
        private void OnEnable()
        {
            m_OnSceneLoadedCallback += OnSceneLoaded;
        }
        private void OnDisable()
        {
            m_OnSceneLoadedCallback -= OnSceneLoaded;
        }

        private void OnSceneLoaded(int level)
        {
            m_CurrentLevel = level;
        }

        internal void OnLevelButtonClicked(int level)
        {
            m_CurrentLevel = level;
            SceneLoader.Instance.LoadGameScene();
        }

    }

}
