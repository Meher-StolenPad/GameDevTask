using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Davanci
{
    public class SceneLoader : SingletonMB<SceneLoader>
    {
        [SerializeField] private CanvasGroup CanvasGroup;

        private bool IsLoadingScene;

        void Start()
        {
            FadeOut(0.5f);
        }
        internal void LoadGameScene()
        {
            if (IsLoadingScene) return;

            StartCoroutine(LoadGameSceneCoroutine());
        }
        internal IEnumerator LoadGameSceneCoroutine()
        {
            IsLoadingScene = true;

            yield return CanvasGroup.DOFade(1f, 0.5f).WaitForCompletion();

            DOTween.KillAll();

            SceneManager.LoadSceneAsync(Database.GameSceneIndex).completed += OnGameSceneLoaded;
        }
        internal void LoadLevelsMenuScene()
        {
            if (IsLoadingScene) return;

            StartCoroutine(LoadLevelSelectionSceneCoroutine());
        }
        internal IEnumerator LoadLevelSelectionSceneCoroutine()
        {   
            IsLoadingScene = true;

            yield return CanvasGroup.DOFade(1f, 0.5f).WaitForCompletion();

            DOTween.KillAll();

            SceneManager.LoadSceneAsync(Database.LevelsMenuSceneIndex).completed += OnLevelSelectionSceneLoaded;
        }
        private void OnGameSceneLoaded(AsyncOperation operation)
        {
            if(operation.isDone)
            {
                FadeOut();
                IsLoadingScene = false;
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
