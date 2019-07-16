# Changelog
All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2019-01-01
### Added
- NEW API 
~~~csharp 
Process GetProcess(string command)
// EXAMPLE USAGE
var cmd = new CommandPrompt();
cmd.GetProcess("echo myname");
~~~

### Changed
- Reordered parameters in overload methods to all be in sync so that intellisense can provide code completion 
 with delegates parameters. 
    - *This may cause your application to not compile but to fix you simply will just need to 
       reorder the paramters*

### Removed
- made the following api private
~~~csharp 
 public ProcessStartInfo CreateStartInfo(string command, bool hideWindow = true, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null)
~~~

[0.0.1]: https://github.com/olivierlacan/keep-a-changelog/releases/tag/v0.0.1

