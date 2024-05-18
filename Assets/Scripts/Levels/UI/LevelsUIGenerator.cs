using Davanci;
using UnityEngine;

public class LevelsUIGenerator : MonoBehaviour
{
    [SerializeField] private Transform LevelsHolderParent;

    [SerializeField] private LevelUIHolder LevelHolderPrefab;

    private LevelsManager LevelsManager;
    private void Start()
    {
        LevelsManager = LevelsManager.Instance;

        var levels = Database.GetLevels();
        for (int i = 0; i < levels.Length; i++)
        {
            LevelUIHolder levelHolder = Instantiate(LevelHolderPrefab, LevelsHolderParent);
            levelHolder.SetLevel(i, LevelsManager.OnLevelButtonClicked);
        }
    }
}
