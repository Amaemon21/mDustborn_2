using System;
using System.Collections.Generic;

namespace GoogleImporter
{
    public class ItemsSettingsParser : IGoogleSheetParser
    {
        private readonly GameSettings _gameSettings;
        
        private ItemsSettings _currentItemSettings;

        public ItemsSettingsParser(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _gameSettings.Items = new List<ItemsSettings>();
        }
        
        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "ID":
                    _currentItemSettings = new ItemsSettings
                    {
                        Id = token,
                    };
                    _gameSettings.Items.Add(_currentItemSettings);
                    break;
                case "Type":
                    _currentItemSettings.Type = Enum.Parse<ItemType>(token);
                    break;
                case "Rarity":
                    _currentItemSettings.Rarity = Enum.Parse<RarityType>(token);
                    break;
                case "Name":
                    _currentItemSettings.Name = token;
                    break;
                case "Description":
                    _currentItemSettings.Description = token;
                    break;
                case "IconName":
                    _currentItemSettings.IconName = token;
                    break;
                case "Amount":
                    _currentItemSettings.Amount = Convert.ToInt32(token);
                    break;
                case "CellCapacity":
                    _currentItemSettings.CellCapacity = Convert.ToInt32(token);
                    break;
                case "PrefabName":
                    _currentItemSettings.PrefabName = token;
                    break;
                default:
                    throw new Exception($"Invalid header: {header}");
            }
        }
    }
}