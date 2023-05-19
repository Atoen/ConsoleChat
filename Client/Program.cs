﻿using Server;

var client = new Client();

AppDomain.CurrentDomain.ProcessExit += async (_, _) => await client.CloseAsync();
Console.CancelKeyPress += async (_, _) => await client.CloseAsync();

Console.Write("Enter username: ");

var username = Console.ReadLine();
if (string.IsNullOrWhiteSpace(username)) username = "User";

Console.WriteLine("Connecting...");
var result = await client.ConnectToServerAsync(username);
result.Switch(
    success => Console.Clear(),
    error =>
    {
        Console.WriteLine(error.Value);
        Console.Read();

        Environment.Exit(1);
    });

client.MessageReceived += delegate(object? _, Message message)
{
    Console.WriteLine(message);
};

client.Listen();

string? message;
do
{
    message = Console.ReadLine() ?? string.Empty;

    if (!string.IsNullOrWhiteSpace(message))
    {
        await client.SendMessageAsync(message);
    }
} while (message != "/exit");

await client.CloseAsync();

