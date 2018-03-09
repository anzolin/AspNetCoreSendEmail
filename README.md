# Send Email using ASP.NET Core
A simple example of a send email project using ASP.NET Core

```
This tutorial is under construction!
```


What you need to do
-------------------

First, install the following applications:
- [.NET Core SDK](https://www.microsoft.com/net/download/core)
- [Visual Studio Code](https://code.visualstudio.com/)
- [C# for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

Considering that you know a little about .Net Core, open the 'Visual Studio Code' and create a new .NET Core mvc project.

Open the file 'appsettings.json' and put yours configurations like the example below:

```json
{
  "EmailSettings": {
    "FromName": "Your name",
    "FromAddress": "you@gmail.com",
    "ToEmail": "",
    "CcEmail": "",
    "BccEmail": "",
    "ServerAddress": "smtp.gmail.com",
    "ServerPort": 587,
    "ServerUseSsl": true,
    "Username": "",
    "Password": ""
  }
}
```

Create a directory called 'Code' in the root folder of your project and in this folder create a class file called 'EmailSettings.cs' with the following content:

```csharp
namespace SendEmail.Code
{
    public class EmailSettings
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public bool ServerUseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
```


License
-------

This example application is [MIT Licensed](https://github.com/anzolin/AspNetCoreSendEmail/blob/master/LICENSE).


About the author
----------------

Hello everyone, my name is Diego Anzolin Ferreira. I'm a .NET developer from Brazil. I hope you will enjoy this simple example application as much as I enjoy developing it. If you have any problems, you can post a [GitHub issue](https://github.com/anzolin/AspNetCoreSendEmail/issues). You can reach me out at diego@anzolin.com.br.
