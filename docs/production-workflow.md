# Production Release Workflow Setup

This document explains how the automated production release workflow works and how to configure it.

## Workflow Overview

The production release workflow (`production-release.yml`) automatically triggers when:
1. A pull request is merged
2. The pull request has the `production` label

## What the Workflow Does

1. **Validates Structure**: Checks that both C# and Unity projects are properly structured
2. **Sets Up Environment**: Installs .NET Framework 4.8 targeting pack and Unity
3. **Generates Version**: Creates a semantic version tag using `vYYYY.MM.DD.{build-number}` format
4. **Creates Git Tag**: Tags the repository with the generated version
5. **Builds C# Library**: Compiles the Asterism Core library in Release configuration
6. **Builds Unity Project**: Creates a Windows 64-bit build of the Unity project
7. **Uploads Artifacts**: Makes build outputs available for download
8. **Creates Release**: Publishes a GitHub release with all artifacts

## Required Repository Secrets

For Unity builds to work, configure these secrets in your repository settings:

- `UNITY_LICENSE`: Complete Unity license file content
- `UNITY_EMAIL`: Unity account email address  
- `UNITY_PASSWORD`: Unity account password

## Usage

### For Developers
1. Make your changes in a feature branch
2. Create a pull request
3. Add the `production` label to the PR
4. Merge the PR when ready

### For Repository Maintainers
1. Go to repository Settings > Secrets and variables > Actions
2. Add the required Unity secrets
3. Ensure the `production` label exists in the repository
4. The workflow will run automatically on labeled PR merges

## Build Outputs

The workflow produces these artifacts:

### C# Library Artifacts
- `AsterismCore.dll` - Main library
- Dependencies and related files
- Available in both `bin/Release/` and Unity `Plugins/x86_64/` locations

### Unity Build Artifacts  
- Complete Windows 64-bit build
- Executable and all required files
- Ready for distribution

### GitHub Release
- Tagged with semantic version
- Contains downloadable artifacts
- Includes build information and PR details

## Testing

A test workflow (`test-workflow.yml`) is also available that:
- Validates project structure
- Runs on pushes to development branches
- Helps verify setup before production releases

You can also use the local test script:
```bash
./scripts/test-build.sh
```

## Troubleshooting

### Common Issues

**Unity License Issues**
- Ensure `UNITY_LICENSE` secret contains the complete license file content
- Verify Unity credentials are correct

**Build Failures**
- Check that .NET Framework 4.8 targeting pack installation succeeded
- Verify project structure hasn't changed

**Missing Artifacts**
- Confirm build output paths match the workflow expectations
- Check that build succeeded before artifact upload

### Workflow Logs
Check the Actions tab in GitHub for detailed workflow execution logs.

## Customization

### Adding Build Platforms
To build for additional platforms, modify the Unity Builder step:
```yaml
- name: Build Unity Project
  uses: game-ci/unity-builder@v4
  with:
    projectPath: "Unity/Asterism Engine"
    targetPlatform: WebGL  # or other platforms
```

### Changing Version Format
Modify the version generation step to use different format:
```yaml
- name: Generate version tag
  run: |
    # Custom version logic here
```

### Adding Build Steps
Insert additional steps between existing ones for:
- Code analysis
- Additional testing
- Custom packaging
- Deployment to other platforms