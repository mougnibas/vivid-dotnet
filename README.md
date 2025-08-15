# Vivid-dotnet

A dotnet project to have fun with a familiar project.

# Usage

TODO

# Dev notes

## Requirements

### .NET 9

(.NET 9 download page)[https://dotnet.microsoft.com/en-us/download/dotnet/9.0]

### Visual Studio Code

(Visual Studio Code download page)[https://code.visualstudio.com/download]

### Report Generator

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

##  VS Code

Open `vivid.code-workspace` with Visual Studio Code.

Install workspace recommanded extensions.

## Build

```bash
dotnet build
```

## Test and coverage

```bash
dotnet test
reportgenerator -reports:TestResults/coverage.cobertura.xml -reportTypes:HtmlInline -targetdir:TestResults/
```
