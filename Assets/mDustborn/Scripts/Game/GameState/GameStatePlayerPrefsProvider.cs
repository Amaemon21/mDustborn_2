using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class GameStatePlayerPrefsProvider : IGameStateProvider
{
    private const string KEY = "GAME STATE";
        
    public GameStateData GameState { get; private set; }
        
    public void Save()
    {
        var json = JsonUtility.ToJson(GameState);
        PlayerPrefs.SetString(KEY, json);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            var json = PlayerPrefs.GetString(KEY);
            GameState = JsonUtility.FromJson<GameStateData>(json);
        }
        else
        {
            GameState = InitFromSettings();
            Save();
        }
    }

    private GameStateData InitFromSettings()
    {
        var gameState = new GameStateData()
        {
            Inventories = new List<InventoryGridData>()
            {
                CreateTestInventory("MAIN"),
                CreateTestInventory("NPC")
            }
        };
            
        return gameState;
    }
        
    private InventoryGridData CreateTestInventory(string ownerId)
    {
        var size = new Vector2Int(3, 4);
        var createdInventorySlots = new List<InventorySlotData>();
        var lenght = size.x * size.y;

        for (int i = 0; i < lenght; i++)
        {
            createdInventorySlots.Add(new InventorySlotData());
        }

        var createdInventoryData = new InventoryGridData()
        {
            OwnerId = ownerId,
            Size = size,
            Slots = createdInventorySlots
        };
        
        return createdInventoryData;
    }
}