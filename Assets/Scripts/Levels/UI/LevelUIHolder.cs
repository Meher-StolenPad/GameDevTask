using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class LevelUIHolder : MonoBehaviour
    {
        #region Serialize Variables Region
        [SerializeField] private int Level = 0;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private RectTransform LevelHolderContainer;
        [SerializeField] private Image LevelUIImage;
        #endregion
            
        #region Private Variable Region
        private Action<int> OnLevelButtonClickedCallback;
        #endregion

        Action<int> OnLevelButtonClick;

        public void SetLevel(int level, Action<int> onLevelButtonClicked)
        {
            Level = level;
            LevelText.text = (level + 1).ToString();
            OnLevelButtonClick = onLevelButtonClicked;
            OnLevelButtonClickedCallback += onLevelButtonClicked;
        }
        #region UI Functions
        public void OnLevelButtonClicked()
        {
            LevelUIImage.raycastTarget = false;
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
        private void OnDestroy()
        {
            OnLevelButtonClickedCallback -= OnLevelButtonClick;
        }
        #endregion
    }
}

