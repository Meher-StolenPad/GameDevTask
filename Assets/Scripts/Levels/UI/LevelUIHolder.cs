using System;
using TMPro;
using UnityEngine;

namespace Davanci
{
    public class LevelUIHolder : MonoBehaviour
    {
        #region Serialize Variables Region
        [SerializeField] private int Level = 0;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private RectTransform LevelHolderContainer;
        #endregion

        #region Private Variable Region
        private Action<int> OnLevelButtonClickedCallback;
        #endregion

        public void SetLevel(int level, Action<int> onLevelButtonClicked)
        {
            Level = level;
            LevelText.text = (level + 1).ToString();
            OnLevelButtonClickedCallback += onLevelButtonClicked;
        }
        #region UI Functions
        public void OnLevelButtonClicked()
        {
            OnLevelButtonClickedCallback?.Invoke(Level);
        }
        public void OnHover()
        {
            LevelHolderContainer.OnHover();
        }
        public void OnUnHover()
        {
            LevelHolderContainer.OnUnHover();
        }
        #endregion
    }
}

