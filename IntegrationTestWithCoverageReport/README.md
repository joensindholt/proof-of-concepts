# IntegrationTestWithCoverageReport

## Test

Run the test suite using the commands:

```
Push-Location IntegrationTest;
Remove-Item TestResults -Force -Recurse;
dotnet test --collect:"XPlat Code Coverage";
reportgenerator.exe "-reports:TestResults\*\coverage.cobertura.xml" "-targetdir:TestResults\CoverageReport" -reporttypes:Html;
Pop-Location;
Invoke-Expression .\IntegrationTest\TestResults\CoverageReport\index.htm;
```

