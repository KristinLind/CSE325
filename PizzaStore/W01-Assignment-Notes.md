# CSE 325 – Week 01 Assignment: .NET Applications

## Component 1: Sales Summary Report

I completed the Microsoft Learn .NET file-system exercise and extended the application to generate a detailed sales summary report.

The application reads JSON sales files, calculates the total sales, and writes the results to `salesTotalDir/sales-summary.txt`.

**Sales Summary**

```text
Sales Summary
----------------------------
Total Sales: $2,012.20

Details:
  sales.json: $88.88
  sales.json: $88.88
  sales.json: $99.00
  sales.json: $1,234.22
  sales.json: $501.22
```

The five sales amounts add up to $2,012.20.

### GenerateSalesSummary Function


```csharp
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
```



## Component 2: Pizza API

I created an ASP.NET Core Web API using .NET 8. The application includes a Pizza model, PizzaService, and PizzaController.

### Initial Pizzas

| ID | Name | Gluten-Free |
|---|---|---|
| 1 | Classic Italian | False |
| 2 | Veggie | True |
| 3 | BBQ Chicken | False |

I added BBQ Chicken as an additional pizza in PizzaService.

### API Testing Results

I tested the API using Swagger at `http://localhost:5191/swagger`.

| HTTP Method | Endpoint | Status Code | Result |
|---|---|---|---|
| GET | /Pizza | 200 OK | Retrieved all three pizzas |
| POST | /Pizza | 201 Created | Created pizza ID 4 |
| PUT | /Pizza/4 | 204 No Content | Updated pizza ID 4 to Hawaiian |
| GET | /Pizza/4 | 200 OK | Confirmed the Hawaiian pizza |
| DELETE | /Pizza/4 | 204 No Content | Deleted pizza ID 4 |
| GET | /Pizza/4 | 404 Not Found | Confirmed pizza ID 4 no longer existed |

### Example POST Request

```json
{
  "id": 0,
  "name": "string",
  "isGlutenFree": true
}
```

The API returned HTTP 201 Created and assigned the new pizza ID 4.

### Example PUT Request

```json
{
  "id": 4,
  "name": "Hawaiian",
  "isGlutenFree": false
}
```

The API returned HTTP 204 No Content. A subsequent GET request confirmed the updated values.

### Conclusion

Both applications were built and tested successfully. The Sales Summary Report generated the expected total of $2,012.20, and the Pizza API successfully handled GET, POST, PUT, and DELETE operations.