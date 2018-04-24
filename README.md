# RhinoCommon Unit Testing

## Intro
Example of unit testing RhinoCommon from within the Visual Studio test runner on windows.

## Prerequisites
Rhino WIP 7  
https://www.rhino3d.com/download/rhino/wip

Visual Studio 2017  
https://www.visualstudio.com/downloads/

## Test Framework
For simplicitiy this project uses the defualt MS Unit framework,
but the principles wouuld be easily transferable to other frameworks such as NUnit or XUnit.

## How to Run
- Build the solution
- Test should appear in Visual Studio Test Explorer
- Set test enviroment to x64  
  `Test > Test Settings > Default Processor Architecture > x64`
- Click `Run All` to run the tests

## Further Reading
For more info on using Rhino in a headless environment see the Rhino Compute project:
https://github.com/mcneel/compute.rhino3d

## Troubleshooting
- Note that when Rhino is in headless mode there is no document defined. 
The static property `RhinoDoc.ActiveDoc` will thus be null which may trip up your plugin code.

- If you have any problems getting this to work or you have a more complex use case, 
then please get in touch via the issue tracker.