using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class CardsGenerator : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab; // Prefab for the card object
        [Range(2, 10)]
        [SerializeField] private int rows = 2;
        [Range(2, 10)]
        [SerializeField] private int columns = 3;

        [SerializeField] private List<Card> cards;

        [SerializeField] private GridLayoutGroup gridLayout;

        private Sprite[] spritesToUse;
        private int[] IconIdexArray;
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
                foreach (Card card in cards)
                {
                    Destroy(card.gameObject);
                }
            }
            int cellCount = rows * columns;

            cards = new List<Card>();

            spritesToUse = Database.GetSprites(cellCount);
            CreateIconsArray(cellCount);
            int indexInArray = 0;

            for (int i = 0; i < cellCount; i++)
            {
                // Instantiate a new card
                Card newCard = Instantiate(cardPrefab, gridLayout.transform);
                int iconIndex = IconIdexArray[indexInArray];
                newCard.Init(iconIndex, spritesToUse[iconIndex]);
                cards.Add(newCard);
                indexInArray++;
            }
        }
        private void CreateIconsArray(int count)
        {
            IconIdexArray = new int[count / 2];// => 4 8

            for (int i = 0; i < count / 2; i++)
            {
                IconIdexArray[i] = i;
            }
            //duplicate the array
            IconIdexArray = IconIdexArray.SelectMany(x => Enumerable.Repeat(x, 2)).ToArray();
            IconIdexArray.Shuffle();
        }
        private void Start()
        {
            CreateCards(rows, columns);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateCards(rows, columns);
            }
        }
    }

}
