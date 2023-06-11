using System.Net.Http.Json;

var client = new HttpClient();
var url = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode=XAU";
var response = (await client.GetFromJsonAsync<IEnumerable<ApiResponse>>(url)).FirstOrDefault();
Console.WriteLine(response);

public class ApiResponse
{  
    public decimal Rate { get; set; }

    public string cc { get; set; }

    public string ExchangeDate { get; set; }
}