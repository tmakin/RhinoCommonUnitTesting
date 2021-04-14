using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using RhinoInside;
using Microsoft.Win32;

namespace RhinoPluginTests
{
    [TestClass]
    public static class TestInit
    {
        static bool initialized = false;
        static string rhinoDir;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            rhinoDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install", "Path", null) as string ?? string.Empty;

            Assert.IsTrue(System.IO.Directory.Exists(rhinoDir), "Rhino system dir not found: {0}", rhinoDir);
            context.WriteLine(" The current Rhino 7 installation is " + rhinoDir);

            if (initialized)
            {
                throw new InvalidOperationException("Initialize Rhino.Inside once");
            }
            else
            {
                Resolver.Initialize();
                initialized = true;
            }
            
            context.WriteLine("Rhino.Inside init has started");

            // Ensure we are 64 bit
            Assert.IsTrue(Environment.Is64BitProcess, "Tests must be run as x64");

            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            Environment.SetEnvironmentVariable("path", envPath + ";" + rhinoDir);

            // Starta headless rhino instance using Rhino.Inside
            var _rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(null, Rhino.Runtime.InProcess.WindowStyle.NoWindow);
        }



    }
}
