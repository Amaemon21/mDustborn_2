using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace GoogleImporter
{
    public class GoogleSheetsImporter 
    {
        private readonly List<string> _headers = new();
        private readonly string _sheetId;
        private readonly SheetsService _service;

        public GoogleSheetsImporter(string credentialsPaths, string sheetId)
        {
            _sheetId = sheetId;
            
            GoogleCredential credential;
            using (var stream = new System.IO.FileStream(credentialsPaths, System.IO.FileMode.Open))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public async Task DownloadAndParseSheet(string sheetName, IGoogleSheetParser parser)
        {
            Debug.Log($"Starting downloading sheet: ({sheetName})...");
            
            var range = $"{sheetName}!A1:Z";
            var request = _service.Spreadsheets.Values.Get(_sheetId, range); 
            
            ValueRange respone;

            try
            {
                respone = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving Google Sheets data: {e.Message}");
                throw;
            }

            if (respone != null && respone.Values != null)
            {
                var tableArray = respone.Values;
                Debug.Log($"Sheet downloaded successfully: {sheetName}");
                
                var fistRow = tableArray[0];

                foreach (var cell in fistRow)
                {
                    _headers.Add(cell.ToString());
                }
                
                var rowsCount = tableArray.Count;

                for (int i = 1; i < rowsCount; i++)
                {
                    var row = tableArray[i];    
                    var rowLenght = row.Count;

                    for (var j = 0; j < rowLenght; j++)
                    {
                        var cell = row[j];  
                        var header = _headers[j];
                        
                        parser.Parse(header, cell.ToString());
                        
                        Debug.Log($"Header: {header}, value: {cell}");
                    }
                }
                
                Debug.Log("Sheet parsed successfully.");
            }
            else
            {
                Debug.Log("No data found in Google Sheets.");
            }
        }
    }
}