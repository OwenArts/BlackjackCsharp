// See https://aka.ms/new-console-template for more information

using Common;
using Server;

var socket = new ServerSocket();

Log.Send().Information($"Started a server");

await Task.Delay(-1);