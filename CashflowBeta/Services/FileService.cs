﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CashflowBeta.Models;
using CashflowBeta.Services.StatementProcessing;

namespace CashflowBeta.Services;

public class FileService
{
    private readonly Window _target;

    public FileService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFile?> OpenFileAsync()
    {
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Statement File",
            AllowMultiple = false
        });

        return files.Count >= 1 ? files[0] : null;
    }

    public static Dictionary<TransactionPartner, List<string>> LoadPartnerKeywords()
    {
        //DEMODATA
        //SHOULD BE MADE CUSTOM WITH DISPLAYS TO CONFIGURE WITH OWN KEYWORDS
        // Create two transaction partners

        var cashWithdrawalPartner = TransactionPartnerService.GetSystemPartners()[0];
        var ownTransferPartner = TransactionPartnerService.GetSystemPartners()[1];

        // Create search term lists separately
        var cashWithdrawalTerms = new List<string>
        {
            "atm",
            "automat",
            "bargeld",
            "cash",
            "withdrawal",
            "geldautomat",
            "cashpoint"
        };
        var ownTransferTerms = new List<string>
        {
            "saving",
            "save",
            "purpose",
            "own",
            "transfer",
            "deposit",
            "internal"
        };

        //Create dictionary
        Dictionary<TransactionPartner, List<string>> partnerKeywords = new()
        {
            { cashWithdrawalPartner, cashWithdrawalTerms },
            { ownTransferPartner, ownTransferTerms }
        };

        return partnerKeywords;
    }

    public static void SaveSpecialPartnerAssociatedStrings()
    {
        Dictionary<TransactionPartner, List<string>> specialPartnerStrings = new();
    }

    //Load a map for a currency transaction csv statement for specific account
    public static CurrencyTransactionCsvMap LoadMapForAccount(int accId)
    {
        var path = GenerateFilePath(accId);
        CurrencyTransactionCsvMap map = new();
        if (File.Exists(path))
        {
            var jsonString = File.ReadAllText(path);
            map = JsonSerializer.Deserialize<CurrencyTransactionCsvMap>(jsonString) ?? new CurrencyTransactionCsvMap();
        }

        return map;
    }

    //Create or save a map for a currency transaction csv statement for specific account
    public static void SaveMapForAccount(int accId, CurrencyTransactionCsvMap map)
    {
        var path = GenerateFilePath(accId);
        var jsonString = JsonSerializer.Serialize(map);
        if (File.Exists(path))
            File.WriteAllText(path, jsonString);
        else
            File.WriteAllText(path, jsonString);
    }

    //Method to generate the filepath for the selected account currency transaction csv statement map
    private static string GenerateFilePath(int accId)
    {
        var path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(),
            "CashFlow",
            "Maps");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(),
            "CashFlow",
            "Maps",
            $"account{accId.ToString()}map.json");
        return path;
    }
}