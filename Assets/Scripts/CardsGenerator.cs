using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab; // Prefab for the card object
    [Range(2, 10)]
    [SerializeField] private int rows = 2;
    [Range(2, 10)]
    [SerializeField] private int columns = 3;

    [SerializeField] private List<GameObject> cards;

    [SerializeField] private GridLayoutGroup gridLayout;

    private void CalculateLayout()
    {
        RectTransform parentRect = gridLayout.transform as RectTransform;
        Vector2 availableSize = new Vector2(parentRect.rect.width, parentRect.rect.height);

        // Ensure all cards are square
        float minDimension = Mathf.Min(availableSize.x, availableSize.y);
        float cellSize = minDimension * 0.9f / Mathf.Max(rows, columns);

        // Calculate spacing
        float spacingX = (columns > 1) ? (availableSize.x - (cellSize * columns)) / columns : 0;
        float spacingY = (rows > 1) ? (availableSize.y - (cellSize * rows)) / rows : 0;

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
        gridLayout.spacing = new Vector2(spacingX, spacingY);
    }
    private void CreateCards(int rows, int columns)
    {
        CalculateLayout();

        if (cards != null)
        {
            foreach (GameObject card in cards)
            {
                Destroy(card.gameObject);
            }
        }
        int cellCount = rows * columns;

        cards = new List<GameObject>();

        for (int i = 0; i < cellCount; i++)
        {
            // Instantiate a new card
            GameObject newCard = Instantiate(cardPrefab, gridLayout.transform);
            cards.Add(newCard);
        }
    }
    private void Start()
    {
        CreateCards(rows, columns);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateCards(rows, columns);
        }
    }
}
