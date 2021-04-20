using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Microsoft.Win32;

namespace RhinoPluginTests
{
    /// <summary>
    /// Shared VS test assembly class using Rhino.Inside
    /// </summary>
    [TestClass]
    public static class TestInit
    {
        static bool initialized = false;
        static string rhinoDir;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //get the correct rhino 7 installation directory
            rhinoDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install", "Path", null) as string ?? string.Empty;
            Assert.IsTrue(System.IO.Directory.Exists(rhinoDir), "Rhino system dir not found: {0}", rhinoDir);
            context.WriteLine(" The current Rhino 7 installation is " + rhinoDir);

            if (initialized)
            {
                throw new InvalidOperationException("Initialize Rhino.Inside once");
            }
            else
            {
                RhinoInside.Resolver.Initialize();
                initialized = true;
                context.WriteLine("Rhino.Inside init has started");
            }
            
            // Ensure we are running the tests in x64
            Assert.IsTrue(Environment.Is64BitProcess, "Tests must be run as x64");

            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            Environment.SetEnvironmentVariable("path", envPath + ";" + rhinoDir);

            // Start a headless rhino instance using Rhino.Inside
            StartRhino();
        }

        /// <summary>
        /// Starting Rhino - loading the relevant libraries
        /// </summary>
        [STAThread]
        public static void StartRhino()
        {
            var _rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(null, Rhino.Runtime.InProcess.WindowStyle.NoWindow);
        }
    }
}
