# RhinoCommon Unit Testing

## Intro
Examples of unit testing RhinoCommon and Grasshopper3d.

## Prerequisites
Rhino 7  
https://www.rhino3d.com/download/Rhino/7.0

Visual Studio 2019
https://www.visualstudio.com/downloads/

## Test Framework
This project provides three example projects.
   - One using the MsTest Framework `RhinoPlugin.Tests`
   - A second one using Xunit `RhinoPlugin.Tests.Xunit`
   - And a thrid one using the old .csproj format without PackageReference Migration where Grasshopper3d libraries are loaded directly by Rhino.Inside `RhinoPlugin.Tests.GHAutoLoad`

but the principles would be easily transferable to other frameworks if needed.

## How to Run
- Build the solution
- Test should appear in Visual Studio Test Explorer
- Set test enviroment to x64  
  `Test > Test Settings > Default Processor Architecture > x64`
- Click `Run All` to run the tests.
- Some times you may need to run each project individualy as the testhost tends to get stuck.

## Further Reading
For more info on using Rhino in a headless environment or inside other applications please see the projects below:
- https://github.com/mcneel/compute.rhino3d
- https://github.com/mcneel/rhino.inside


## Known issues
Using the new *.csproj* format, the headless version of Rhino fails to load the GH libraries. To tackle this we are loading the library manually to the test context by resolving the specific assembly.

If you are using the old .csproj format and with a packages.config for the references then GH is loading automatically.

## Troubleshooting
- Note that when Rhino is in headless mode there is no document defined. 
The static property `RhinoDoc.ActiveDoc` will thus be null which may trip up your plugin code.

- If you have any problems getting this to work or you have a more complex use case, please get in touch via the issue tracker.