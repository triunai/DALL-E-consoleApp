﻿using DallETest; //since you have no folders it takes the default root

using Microsoft.Extensions.Configuration;
using System.Reflection;

Console.WriteLine("Starting commandline for DALL-E [Open AI]");

var config = BuildConfig();

IOpenAIProxy aiClient = new OpenAIHttpService(config);

Console.WriteLine("Type your first Prompt");
var msg = Console.ReadLine();

var nImages = int.Parse(config["OpenAi:DALL-E:N"]);
var imageSize = config["OpenAi:DALL-E:Size"];
var prompt = new GenerateImageRequest(msg, nImages, imageSize);

var result = await aiClient.GenerateImages(prompt);

foreach (var item in result.Data)
{
    Console.WriteLine(item.Url);

    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}.png");
    var img = await aiClient.DownloadImage(item.Url);

    await File.WriteAllBytesAsync(fullPath, img);

    Console.WriteLine("New image saved at {0}", fullPath);
}

Console.WriteLine("Press any key to exit");
Console.ReadKey();

static IConfiguration BuildConfig()
{
    var dir = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    return configBuilder.Build();
}
