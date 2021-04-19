using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Xunit;

namespace RhinoPlugin.Tests.Xunit
{


    public class XunitTestFixture : IDisposable
    {
        bool initialized = false;
        static string rhinoDir;
        static string ghDir="";
        Rhino.Runtime.InProcess.RhinoCore _rhinoCore;
        Grasshopper.Plugin.GH_RhinoScriptInterface pluginObject;


        public XunitTestFixture()
        {
            rhinoDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install", "Path", null) as string ?? string.Empty;
            ghDir = Path.Combine(Path.GetFullPath(Path.Combine(rhinoDir, @"..\")), "Plug-ins\\Grasshopper\\");

            Assert.True(System.IO.Directory.Exists(rhinoDir), String.Format("Rhino system dir not found: {0}", rhinoDir));

            //rhinoAppDomain = RhinoLoadDomain.CreateDomain();

            // Make sure we are running the tests as 64x
            Assert.True(Environment.Is64BitProcess, "Tests must be run as x64");

            if (initialized)
            {
                throw new InvalidOperationException("Initialize Rhino.Inside once");
            }
            else
            {
                RhinoInside.Resolver.Initialize();
                initialized = true;
            }

            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            Environment.SetEnvironmentVariable("path", envPath + ";" + rhinoDir);

            // Start a headless rhino instance using Rhino.Inside
            StartRhino();
            // We have to manually load the GH.dll to the AppDomain :(
            AppDomain.CurrentDomain.AssemblyResolve += ResolveGH;
        }

        /// <summary>
        /// Sart headless rhino
        /// </summary>
        /// <returns></returns>
        [STAThread]
        public void StartRhino()
        {
            _rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(null, Rhino.Runtime.InProcess.WindowStyle.NoWindow);
   
        }

        /// <summary>
        /// Start a headless GH
        /// This doesn't seem to work at the moment -hmmm hmmm... 
        /// </summary>
        [STAThread]
        public void StartGH()
        {
            pluginObject = Rhino.RhinoApp.GetPlugInObject("Grasshopper.dll") as Grasshopper.Plugin.GH_RhinoScriptInterface;
            pluginObject.RunHeadless();
        }

        private static Assembly ResolveGH(object sender, ResolveEventArgs args)
        {
            var name = args.Name;

            if (!name.StartsWith("Grasshopper"))
            {
                return null;
            }

            var path = Path.Combine(ghDir, "Grasshopper.dll");
            return Assembly.LoadFrom(path);
        }

        public void Dispose()
        {
            // do nothing or...
            _rhinoCore?.Dispose();
            _rhinoCore = null;
            AppDomain.Unload(AppDomain.CurrentDomain);
        }
    }


    /// <summary>
    /// Collection Fixture
    /// </summary>
    [CollectionDefinition("RhinoTestingCollection")]
    public class RhinoCollection : ICollectionFixture<XunitTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}

