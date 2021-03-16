var generateDocsTask = Task("Generate-Docs")
.WithCriteria<BuildParameters>((context, parameters) => context.IsRunningOnWindows(),  "Generate-Docs will only run on windows agent.")
.WithCriteria<BuildParameters>((context, parameters) => parameters.IsLocalBuild,  "Generate-Docs will only run during a local build")
.Does<BuildParameters>((parameters) => 
{

   foreach(var project in parameters.SolutionProjects){  
       
        var isABoolean = bool.TryParse(project.GetProjectProperty("GenerateDocumentation"),out var canGenerateDocumentation);
        if(!canGenerateDocumentation) 
        break;

	    DocFxMetadata("./docs/docfx.json");
	    DocFxBuild("./docs/docfx.json");
	    CreateDirectory(parameters.Paths.Directories.Artifacts);
	    Zip("./docs/_site/", parameters.Paths.Directories.Artifacts.CombineWithFilePath("docfx.zip"));

        break;

    }
})
.ReportError(exception =>
{
    Error(exception.Dump());
})
;


