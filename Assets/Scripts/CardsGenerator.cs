using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Davanci
{
    public class CardsGenerator : MonoBehaviour
    {
        [SerializeField] private Card CardPrefab; // Prefab for the card object
        [Range(2, 10)]
        [SerializeField] private int Rows = 2;
        [Range(2, 10)]
        [SerializeField] private int Columns = 3;

        [SerializeField] private List<Card> Cards = new List<Card>();
        [SerializeField] private GridLayoutGroup GridLayout;
        [SerializeField] private RectTransform DiscardPile;


        private Sprite[] SpritesToUse;
        private int[] IconIdexArray;

        #region Cards Generation
        private void CalculateLayout()
        {
            RectTransform parentRect = GridLayout.transform as RectTransform;
            Vector2 availableSize = new Vector2(parentRect.rect.width, parentRect.rect.height);

            // Ensure all cards are square
            float minDimension = Mathf.Min(availableSize.x, availableSize.y);
            float cellSize = minDimension * 0.9f / Mathf.Max(Rows, Columns);

            // Calculate spacing
            float spacingX = (Columns > 1) ? (availableSize.x - (cellSize * Columns)) / Columns : 0;
            float spacingY = (Rows > 1) ? (availableSize.y - (cellSize * Rows)) / Rows : 0;

            GridLayout.cellSize = new Vector2(cellSize, cellSize);
            GridLayout.spacing = new Vector2(spacingX, spacingY);
        }
        private void CreateCards(int rows, int columns)
        {
            CalculateLayout();

            if (Cards != null)
            {
                foreach (Card card in Cards)
                {
                    Destroy(card.gameObject);
                }
            }
            int cellCount = rows * columns;


            SpritesToUse = Database.GetSprites(cellCount);

            CreateIconsArray(cellCount);


            int? middleCell = (cellCount) % 2 == 0 ? null : ((cellCount) / 2);

            int indexInArray = 0;

            for (int i = 0; i < cellCount; i++)
            {
                // Instantiate a new card
                Card newCard = Instantiate(CardPrefab, GridLayout.transform);
                Cards.Add(newCard);

                int iconIndex = IconIdexArray[indexInArray];

                if (!CheckMiddleCell(middleCell, i))
                {
                    newCard.Init(iconIndex, SpritesToUse[iconIndex], DiscardPile);
                    indexInArray++;
                }
                else
                {
                    newCard.DisableCard();
                }

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
        private bool CheckMiddleCell(int? middleCell, int i)
        {
            if (middleCell == null) return false;

            if (middleCell.Value == i) return true;
            else return false;
        }
        #endregion

        private void Start()
        {
            CreateCards(Rows, Columns);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateCards(Rows, Columns);
            }
        }
    }

}
