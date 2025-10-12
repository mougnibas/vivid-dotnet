# Vivid-dotnet

A dotnet project to have fun with a familiar project.

# Usage

TODO

# Dev notes

## Requirements

### .NET 9

[.NET 9 download page](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Visual Studio Code

[Visual Studio Code download page](https://code.visualstudio.com/download)

### Report Generator

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

## VS Code

Open `vivid.code-workspace` with Visual Studio Code.

Install workspace recommended extensions.

## Docker Desktop

TODO Link to download page

### Kubernates

"Kubeadm" setup (NOT "kind" setup).

### Ingress in Kubernates

'''bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.1/deploy/static/provider/cloud/deploy.yaml
'''

### Build

`Terminal / Run Task ... / build`

or

`Terminal / Run Build Task ...`

### Test and coverage

`Terminal / Run Task ... / test`

or

`Left bar / Testing / Run Tests with Coverage`

## CLI

### Build

```bash
dotnet build
```

### Test and coverage (unit only)

```bash
dotnet test --filter TestCategory=Unit
```

### Test and coverage (integration only)

```bash
dotnet test --filter TestCategory=Integration
```

### Test and coverage (unit and integration)

```bash
dotnet test
```

HTML report is located in `test/TestResults/`.
