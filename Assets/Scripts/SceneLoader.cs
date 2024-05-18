using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Davanci
{
    public class SceneLoader : SingletonMB<SceneLoader>
    {
        [SerializeField] private CanvasGroup CanvasGroup;
        void Start()
        {
            FadeOut(0.5f);
        }
        internal void LoadGameScene()
        {
            CanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                SceneManager.LoadSceneAsync(Database.GameSceneIndex).completed += OnGameSceneLoaded;
            });
        }
        internal void LoadLevelsMenuScene()
        {
            CanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                SceneManager.LoadSceneAsync(Database.LevelsMenuSceneIndex).completed += OnLevelSelectionSceneLoaded;
            });
        }
        private void OnGameSceneLoaded(AsyncOperation operation)
        {
            if(operation.isDone)
            {
                FadeOut();
            }
        }
        private void OnLevelSelectionSceneLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                FadeOut();
            }
        }

        private void FadeOut(float duration = 1f)
        {
            CanvasGroup.DOFade(0f, duration);
        }
    }
}

