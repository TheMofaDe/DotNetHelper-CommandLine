# DotNetHelper-CommandLine

#### *A cross-platform command-line helper class that makes it easy to run commands. This library also comes with commonly used command see api docs for examples.* 

|| [**View on Github**][Github] || 


## Features
+ Runs terminal commands
+ Works on linux & windows


## How to use
```csharp
        static void Main(string[] args)
        {
                var cmd = new CommandPrompt();
                var exitCode = cmd.RunCommand("ping www.google.com"), null,Exited, OnDataReceived, ErrorDataReceived); // RETURNS 0
                var exitCode1 = cmd.RunCommand("ping This is not a valid command"), null, OnDataReceived, ErrorDataReceived, Exited); // RETURN 1
                Console.ReadLine();
        }
        private static void Exited(object sender, EventArgs e)
        {
            Console.WriteLine("command has finished.");
        }
        private static void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("error : " + e.Data);
        }
        private static void OnDataReceived(object sender, DataReceivedEventArgs args)
        {
            Console.WriteLine("received data : " + args.Data);
        }
    }
```
## Results from sample code
 
![alt text][logo]

[logo]: images/codesnippet1.PNG "Code Snippet Output"


<!-- Links. -->

[1]:  https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[2]: http://themofade.github.io/DotNetHelper-CommandLine

[Cake]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Azure DevOps]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[AppVeyor]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[GitVersion]: https://gitversion.readthedocs.io/en/latest/
[Nuget]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Chocolately]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[WiX]: http://wixtoolset.org/
[DocFx]: https://dotnet.github.io/docfx/
[Github]: https://github.com/TheMofaDe/DotNetHelper-CommandLine


<!-- Documentation Links. -->
[Docs]: https://themofade.github.io/DotNetHelper-CommandLine/index.html
[Docs-API]: https://themofade.github.io/DotNetHelper-CommandLine/api/DotNetHelper-CommandLine.Attribute.html
[Docs-Tutorials]: https://themofade.github.io/DotNetHelper-CommandLine/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/