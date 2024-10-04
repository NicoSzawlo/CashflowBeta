using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CashflowBeta.Models;
using CashflowBeta.Services.StatementProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class FileService
    {
        private readonly Window _target;

        public FileService(Window target)
        {
            _target = target;
        }

        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select Statement File",
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public static Dictionary<TransactionPartner,List<string>> LoadSpecialPartnerAssociatedStrings()
        {
            Dictionary<TransactionPartner, List<string>> specialPartnerStrings = new();
            
            
            return specialPartnerStrings;
        }
        public static void SaveSpecialPartnerAssociatedStrings()
        {
            Dictionary<TransactionPartner, List<string>> specialPartnerStrings = new();

        }

        //Load a map for a currency transaction csv statement for specific account
        public static CurrencyTransactionCsvMap LoadMapForAccount(int accId)
        {
            string path = GenerateFilePath(accId);
            CurrencyTransactionCsvMap map = new();
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                map = JsonSerializer.Deserialize<CurrencyTransactionCsvMap>(jsonString) ?? new CurrencyTransactionCsvMap();
            }
            return map;
        }
        //Create or save a map for a currency transaction csv statement for specific account
        public static void SaveMapForAccount(int accId, CurrencyTransactionCsvMap map)
        {
            string path = GenerateFilePath(accId);
            string jsonString = JsonSerializer.Serialize(map);
            GenerateFilePath(accId);
            if (File.Exists(path))
            {
                File.WriteAllText(path, jsonString);
            }
            else
            {
                File.WriteAllText(path, jsonString);
            }
        }
        //Method to generate the filepath for the selected account currency transaction csv statement map
        private static string GenerateFilePath(int accId)
        {
            string path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(),
                                       "CashFlow",
                                       "Maps",
                                       $"account{accId.ToString()}map.json");
            return path;
        }
    }
}
