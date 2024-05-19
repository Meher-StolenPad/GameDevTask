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
        private int CurrentLevel;

        void Start()
        {
            if (GameSave.HasSave(out GameSaveHolder gameSaveHolder))
            {
                CurrentLevel = gameSaveHolder.Level;
                LoadGameScene();
                LevelsManager.m_OnSceneLoadedCallback?.Invoke(CurrentLevel);
                return;
            }
            StartCoroutine(FadeOut(0.5f));

        }
        internal void LoadGameScene()
        {
            if (IsLoadingScene) return;

            StartCoroutine(LoadGameSceneCoroutine());
        }
        internal IEnumerator LoadGameSceneCoroutine()
        {
            IsLoadingScene = true;

            yield return StartCoroutine(FadeIn(0.5f));

            DOTween.KillAll();

            SceneManager.LoadSceneAsync(Database.m_GameSceneIndex).completed += OnGameSceneLoaded;
        }
        internal void LoadLevelsMenuScene()
        {
            if (IsLoadingScene) return;

            StartCoroutine(LoadLevelSelectionSceneCoroutine());
        }
        internal IEnumerator LoadLevelSelectionSceneCoroutine()
        {
            IsLoadingScene = true;

            yield return StartCoroutine(FadeIn(0.5f));

            DOTween.KillAll();

            SceneManager.LoadSceneAsync(Database.m_LevelsMenuSceneIndex).completed += OnLevelSelectionSceneLoaded;
        }
        private void OnGameSceneLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                StartCoroutine(FadeOut(0.5f));
                IsLoadingScene = false;
            }
        }
        private void OnLevelSelectionSceneLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                StartCoroutine(FadeOut(0.5f));
                IsLoadingScene = false;
            }
        }

        private IEnumerator FadeOut(float duration)
        {
            float elapsed = 0f;
            float startAlpha = CanvasGroup.alpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                yield return null;
            }

            CanvasGroup.alpha = 0f;
        }
        private IEnumerator FadeIn(float duration)
        {
            if (CanvasGroup.alpha == 1f)
            {
                yield break; // Exit the coroutine if the alpha is already 1
            }

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                CanvasGroup.alpha = Mathf.Lerp(0, 1f, elapsed / duration);
                yield return null;
            }

            CanvasGroup.alpha = 1f;
        }
    }
}

