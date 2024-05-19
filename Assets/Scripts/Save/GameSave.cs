using System;
using System.Collections.Generic;
using UnityEngine;

namespace Davanci
{
    [Serializable]
    public class CardSave
    {
        public int Id;
        public bool Collected;

        public CardSave(int id, bool collected)
        {
            Id = id;
            Collected = collected;
        }
    }
    [Serializable]
    public class GameSaveHolder
    {
        public int Level;
        public List<CardSave> cardSaves = new();
        public int MovesCount;
        public int MatchCount;
        public int ComboCount;
        public int MaxComboCount;   
        public int Time;
        public void Clear()
        {
            cardSaves = new();
            MovesCount = 0;
            MatchCount = 0;
            ComboCount = 0;
            MaxComboCount = 0;
            Time = 0;
            Level = 0;
        }
    }
    [CreateAssetMenu(fileName = FileName, menuName = "Match Card Game/Game Save")]
    public class GameSave : ScriptableObject
    {
        private const string FileName = "GameSave";

        public static GameSaveHolder GameSaveHolder = new GameSaveHolder();

        public static void OnCardsCreated(List<Card> cards, int level)
        {
            GameSaveHolder.Level = level;

            for (int i = 0; i < cards.Count; i++)
            {
                CardSave cs = cards[i].CreateCardSave();
                GameSaveHolder.cardSaves.Add(cs);
            }
            Save();
        }
        public static void OnCardsUpdated(int card1Index, int card2Index)   
        {
            if (GameSaveHolder.cardSaves.Count <= card1Index || GameSaveHolder.cardSaves.Count <= card2Index) return;

            Debug.Log(card1Index + "% " + card2Index);

            GameSaveHolder.cardSaves[card1Index].Collected = true;
            GameSaveHolder.cardSaves[card2Index].Collected = true;

            Save();
        }
        public static void OnDataChanged(int Matchcount, int tryCount, int time, int comboCount, int maxComboCount)
        {
            GameSaveHolder.MovesCount = tryCount;
            GameSaveHolder.MatchCount = Matchcount;
            GameSaveHolder.Time = time;
            GameSaveHolder.ComboCount = comboCount;
            GameSaveHolder.MaxComboCount = maxComboCount;
            Save();
        }

        public static bool HasSave(out GameSaveHolder returnValue)
        {
            bool hasSave = false;
            hasSave = SaveSystem.Load(FileName, GameSaveHolder, out returnValue);
            GameSaveHolder = returnValue;
            return hasSave;
        }
        private static void Save()
        {
            SaveSystem.Save(FileName, GameSaveHolder);
        }
        public static void DeleteSave()
        {
            GameSaveHolder.Clear();
            SaveSystem.DeleteSave(nameof(GameSave));
        }
    }

}
