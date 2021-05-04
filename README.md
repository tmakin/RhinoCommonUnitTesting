# RhinoCommon Unit Testing

## Intro
Example of unit testing RhinoCommon from within the Visual Studio test runner on windows.

## Prerequisites
Rhino 7  
https://www.rhino3d.com/download/Rhino/7.0

Visual Studio 2019
https://www.visualstudio.com/downloads/

## Test Framework
This project provides examples for XUnit and MS Test,
but the principles would be easily transferable to other frameworks if needs.

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
