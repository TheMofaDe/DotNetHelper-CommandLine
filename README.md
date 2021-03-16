# DotNetHelper-CommandLine


#### *A cross-platform command-line helper class that makes it easy to run commands. This library also comes with commonly used command see api docs for examples.* 

|| [**Documentation**][Docs] • [**API**][Docs-API] • [**Tutorials**][Docs-Tutorials] ||  [**Change Log**][Changelogs] • || [**View on Github**][Github]|| 

| AppVeyor | AzureDevOps |
| :-----: | :-----: |
| [![Build status](https://ci.appveyor.com/api/projects/status/DotNetHelper-CommandLine?svg=true)](https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-CommandLine)  | [![Build Status](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-CommandLine/_apis/build/status/TheMofaDe.DotNetHelper-CommandLine?branchName=master)](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_build/latest?definitionId=5&branchName=master)  

| Package  | Tests | Code Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![codecov](https://codecov.io/gh/TheMofaDe/DotNetHelper-CommandLine/branch/master/graph/badge.svg)](https://codecov.io/gh/TheMofaDe/DotNetHelper-CommandLine) |


```csharp
static void Main(string[] args)
{
	var cmd = new CommandPrompt();
	cmd.OutputDataReceived += OnDataReceived;
	cmd.Exited += Exited;
	cmd.ErrorDataReceived += ErrorDataReceived;

    var process = cmd.RunCommand("ping www.google.com");
    // Or if you need wait until the process 
    var processButExited = cmd.RunCommandAndWaitForExit("ping www.youtube.com");
}


private static void Exited(object sender, EventArgs e)
{
	Console.WriteLine("command has exited.");
}

private static void ErrorDataReceived(object sender, DataReceivedEventArgs e)
{
	Console.WriteLine("error : " + e.Data);
}

private static void OnDataReceived(object sender, DataReceivedEventArgs args)
{
	Console.WriteLine("received data : " + args.Data);
}

```
## Results from sample code
 
![alt text][logo]

[logo]: docs/images/codesnippet1.PNG "Code Snippet Output"


## Documentation
For more information, please refer to the [Officials Docs][Docs]



<!-- Links. -->

[1]:  https://gist.github.com/davidfowl/ed7564297c61fe9ab814

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
[Docs-API]: https://themofade.github.io/DotNetHelper-CommandLine/api/DotNetHelper-CommandLine.html
[Docs-Tutorials]: https://themofade.github.io/DotNetHelper-CommandLine/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/


<!-- BADGES. -->

[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper-CommandLine.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/TheMofaDe/DotNetHelper-CommandLine.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-CommandLine/_apis/build/status/TheMofaDe.DotNetHelper-CommandLine?branchName=master&jobName=Windows


