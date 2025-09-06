# Asterism-Engine

A Unity game engine project with C# library components.

## Project Structure

- `Asterism/Common/` - Core C# library (.NET Framework 4.8)
- `Unity/Asterism Engine/` - Unity project (Unity 6000.0.45f1)

## Automated Production Releases

This repository includes automated production release workflows that trigger when a pull request with the `production` label is merged.

### How it works

1. Create a pull request with your changes
2. Add the `production` label to the pull request
3. When the PR is merged, the workflow automatically:
   - Creates a version tag using format `vYYYY.MM.DD.{build-number}`
   - Builds the C# library in Release configuration
   - Builds the Unity project for Windows 64-bit
   - Creates downloadable artifacts
   - Publishes a GitHub release with the built components

### Setup Requirements

For the Unity build to work, the following secrets need to be configured in the repository:

- `UNITY_LICENSE` - Unity license content (for Unity builds)
- `UNITY_EMAIL` - Unity account email
- `UNITY_PASSWORD` - Unity account password

### Build Artifacts

The workflow produces:
- **C# Library**: `AsterismCore.dll` and dependencies
- **Unity Build**: Complete Windows 64-bit build
- **GitHub Release**: Tagged release with downloadable assets

### Manual Testing

A test workflow is also available that runs on push to development branches to validate the project structure and build environment.
