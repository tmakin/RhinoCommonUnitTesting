using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace RhinoPlugin.Tests.RHInside
{

    /// <summary>
    /// Shared test context across unit tests that loads rhinocommon.dll and grasshopper.dll
    /// </summary>
    public class XunitTestFixtureRI : IDisposable
    {
 
        private Rhino.Runtime.InProcess.RhinoCore _rhinoCore;

        /// <summary>
        /// Empty Constuctor
        /// </summary>
        public XunitTestFixtureRI()
        {
            // Use the latest version of RH7 installed in your system
            RhinoInside.Resolver.UseLatest = true;

            // Make sure we are running the tests as 64x
            Assert.True(Environment.Is64BitProcess, "Tests must be run as x64");

            //Initialize Rhino.Inside
            RhinoInside.Resolver.Initialize();

            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            Environment.SetEnvironmentVariable("path", envPath + ";" + RhinoInside.Resolver.RhinoSystemDirectory);

            // Start Rhino and load all libraries
            StartRhino();

        }

        /// <summary>
        /// Starting Rhino - loading the relevant libraries
        /// </summary>
        [STAThread]
        public void StartRhino()
        {
            _rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(null, Rhino.Runtime.InProcess.WindowStyle.NoWindow);
        }

        /// <summary>
        /// Disposing the context after running all the tests
        /// </summary>
        public void Dispose()
        {
            // do nothing or...
            _rhinoCore?.Dispose();
            _rhinoCore = null;
        }
    }


    /// <summary>
    /// Collection Fixture - shared context across test classes
    /// </summary>
    [CollectionDefinition("RhinoTestingCollection")]
    public class RhinoCollection : ICollectionFixture<XunitTestFixtureRI>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}

