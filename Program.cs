using Newtonsoft.Json;
using System.Text;

if (args.Length> 0)
{
    HttpClient client = new HttpClient();
    
    client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

    //var content = new StringContent("{\"model\":\"text-devinci-001\",\"prompt\":\""+ args[0] +"\", \"tempereture\":1, \"max_tokens\":100}",
    //    Encoding.UTF8, "application/json");

    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + args[0] + "\",\"temperature\": 1,\"max_tokens\": 100}",
        Encoding.UTF8, "application/json");

    HttpResponseMessage res = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string resString = await res.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(resString);

        string guess = GuessCommand(dyData!.choices[0].text);
        Console.ForegroundColor= ConsoleColor.Green;

        Console.WriteLine($"My Guess --> {guess}");

        Console.ResetColor();

    }catch(Exception ex)
    {
        Console.WriteLine($" JSON conversion error! --> {ex.Message}");
    }

    //Console.WriteLine(resString);
}

else
{
    Console.WriteLine("Invalid Input!");
}


static string GuessCommand(string command)
{
    Console.WriteLine($"GPT Returned Text --> ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(command);

    var lastIndex = command.LastIndexOf('\n');

    string guess = command.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);
    return guess;
}