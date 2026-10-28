using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

var salesSummary = GenerateSalesSummary(salesFiles);
File.WriteAllText(Path.Combine(salesTotalDir, "sales-summary.txt"), salesSummary);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);


    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);

        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }


    return salesFiles;
}


double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop through each JSON file
    foreach (var file in salesFiles)
    {
        // Read the file contents
        string salesJson = File.ReadAllText(file);

        // Convert JSON into a SalesData record
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the sales amount to the total
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

string GenerateSalesSummary(IEnumerable<string> salesFiles)
{
    double totalSales = 0;
    var details = new List<string>();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        if (Path.GetFileName(file) != "sales.json")
        {
            continue;
        }
        double fileTotal = data?.Total ?? 0;
        totalSales += fileTotal;

        string fileName = Path.GetFileName(file);
        details.Add($"  {fileName}: {fileTotal:C2}");
    }

    string summary = "Sales Summary" + Environment.NewLine;
    summary += "----------------------------" + Environment.NewLine;
    summary += $"Total Sales: {totalSales:C2}" + Environment.NewLine;
    summary += Environment.NewLine + "Details:" + Environment.NewLine;
    summary += string.Join(Environment.NewLine, details);

    return summary;
}

record SalesData(double Total);
