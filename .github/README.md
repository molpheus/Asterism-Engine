# GitHub Actions CI/CD Setup

This repository now includes automated GitHub Actions workflows for continuous integration.

## Workflow: CI/CD - Code Quality & Unit Tests

**File:** `.github/workflows/ci-comprehensive.yml`

### Purpose
Automatically performs:
- **コードエラー検査** (Code Error Checking) - Builds the solution to detect compilation errors
- **UnitTestの自動化** (Unit Test Automation) - Runs all unit tests automatically

### Triggers
- Push to `main`, `master`, or `develop` branches
- Pull requests to `main` or `master` branches

### What it does
1. **Environment Setup**
   - Uses Windows runner (required for .NET Framework 4.8)
   - Installs .NET SDK 8.0.x
   - Installs .NET Framework 4.8 Developer Pack

2. **Code Quality Check**
   - Restores NuGet dependencies
   - Builds the solution (`Asterism/Common/Common.sln`)
   - Fails if compilation errors are found

3. **Unit Test Automation**
   - Runs all unit tests in the `UnitTest` project
   - Generates test results in TRX format
   - Collects code coverage data

4. **Reporting**
   - Publishes test results with detailed reporting
   - Uploads test artifacts
   - Generates code coverage summary
   - Provides success/failure summary

### Project Structure
```
Asterism/
├── Common/
│   ├── Common.sln          # Main solution file
│   ├── Common.csproj       # Main library project (.NET Framework 4.8)
│   └── UnitTest/
│       └── UnitTest.csproj # Unit test project (MSTest)
```

### Test Framework
- **MSTest** framework for unit testing
- Tests are located in `Asterism/Common/UnitTest/`
- Existing test files:
  - `CronTest.cs`
  - `PomodoroTest.cs` 
  - `ReminderTest.cs`
  - `AESTest.cs`

The workflow will automatically run on every push and pull request, ensuring code quality and test coverage are maintained.