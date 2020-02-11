using System;
using System.Reflection;
using System.Runtime.InteropServices;

using Xunit;


namespace RhinoPlugin.Tests.Xunit
{
    /// <summary>
    /// Initialization process for Rhino and  shared Test Context adapted to xunit from here https://github.com/tmakin/RhinoCommonUnitTesting
    /// </summary>
    public class XunitTestInitFixture : IDisposable
    {
        static bool initialized = false;
        static string systemDir = null;
        static string systemDirOld = null;



        public XunitTestInitFixture()
        {
            ////Check if the fixture is already initialised
            if (initialized)
            {
                throw new InvalidOperationException("AssemblyInitialize should only be called once");
            }
            initialized = true;

            // Make surte we are running the tests as 64x
            Assert.True(Environment.Is64BitProcess, "Tests must be run as x64");


            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            systemDir = System.IO.Path.Combine(programFiles, "Rhino 7 WIP", "System");
            systemDirOld = System.IO.Path.Combine(programFiles, "Rhino WIP", "System");
            if (System.IO.Directory.Exists(systemDir) != true)
            {
                systemDir = systemDirOld;
            }

            Assert.True(System.IO.Directory.Exists(systemDir), string.Format("Rhino system dir not found: {0}", systemDir));
            // Add rhino system directory to path (for RhinoLibrary.dll)
            Environment.SetEnvironmentVariable("path", envPath + ";" + systemDir);

            // Add hook for .Net assmbly resolve (for RhinoCommmon.dll)
            AppDomain.CurrentDomain.AssemblyResolve += ResolveRhinoCommon;

            // Start headless Rhino process
            LaunchInProcess(0, 0);
        }

        private static Assembly ResolveRhinoCommon(object sender, ResolveEventArgs args)
        {
            var name = args.Name;

            if (!name.StartsWith("RhinoCommon"))
            {
                return null;
            }

            var path = System.IO.Path.Combine(systemDir, "RhinoCommon.dll");
            return Assembly.LoadFrom(path);
        }

        public void Dispose()
        {
            //Cleaning up
            ExitInProcess();
            //initialized = false;
        }

        [DllImport("RhinoLibrary.dll")]
        internal static extern int LaunchInProcess(int reserved1, int reserved2);

        [DllImport("RhinoLibrary.dll")]
        internal static extern int ExitInProcess();
    }

    /// <summary>
    /// Collection Fixture
    /// </summary>
    [CollectionDefinition("Rhino Collection")]
    public class RhinoCollection : ICollectionFixture<XunitTestInitFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
