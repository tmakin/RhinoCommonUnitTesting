//#define RHINOINSIDE

using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Xunit;



namespace RhinoPlugin.Tests.Xunit
{
    /// <summary>
    /// Shared Xunit test context using a manual load of assemblies WIP using Rhino.Inside
    /// </summary>
    public class XunitTestInitFixture : IDisposable
    {
        static bool initialized = false;
        static string rhinoDir;
#if RHINOINSIDE
            Rhino.Runtime.InProcess.RhinoCore _rhinoCore;
#endif

        
        public XunitTestInitFixture()
        {
            rhinoDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install", "Path", null) as string ?? string.Empty;

            Assert.True(System.IO.Directory.Exists(rhinoDir), String.Format("Rhino system dir not found: {0}", rhinoDir));

            //Check if the fixture is already initialised
            if (initialized)
            {
                throw new InvalidOperationException("AssemblyInitialize should only be called once");
            }
#if RHINOINSIDE
            RhinoInside.Resolver.Initialize();
#endif
            initialized = true;

            // Make sure we are running the tests as 64x
            Assert.True(Environment.Is64BitProcess, "Tests must be run as x64");

            // Add rhino system directory to path
            string envPath = Environment.GetEnvironmentVariable("path");
            Environment.SetEnvironmentVariable("path", envPath + ";" + rhinoDir);
#if !RHINOINSIDE
            // Add hook for .Net assembly resolve (for RhinoCommmon.dll and Grasshopper3d.dll)
            AppDomain.CurrentDomain.AssemblyResolve += ResolveRhinoAssemblies;


#endif
            // Start headless Rhino process
#if RHINOINSIDE
            _rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(null, Rhino.Runtime.InProcess.WindowStyle.Hidden);
#elif !RHINOINSIDE
            LaunchInProcess(0, 0);
#endif
        }

        public void Dispose()
        {
#if RHINOINSIDE
            _rhinoCore?.Dispose();
            _rhinoCore = null;
#elif !RHINOINSIDE
            ExitInProcess();
#endif

        }

        private static Assembly ResolveRhinoAssemblies(object sender, ResolveEventArgs args)
        {
            var name = args.Name;

            if (name.StartsWith("RhinoCommon"))
            {
                var path = System.IO.Path.Combine(rhinoDir, "RhinoCommon.dll");
                return Assembly.LoadFrom(path);
            }
           else if (name.StartsWith("Grasshopper"))
            {
                var path = System.IO.Path.Combine(Path.GetFullPath(Path.Combine(rhinoDir, @"..\")), "Plug-ins\\Grasshopper\\Grasshopper.dll");
                return Assembly.LoadFrom(path);
            }
            else
            {
                return null;
            }

        }

        [DllImport("RhinoLibrary.dll")]
        internal static extern int LaunchInProcess(int reserved1, int reserved2);

        [DllImport("RhinoLibrary.dll")]
        internal static extern int ExitInProcess();

    }


    /// <summary>
    /// Collection Fixture
    /// </summary>
    [CollectionDefinition("Rhino Testing Collection")]
    public class RhinoCollection : ICollectionFixture<XunitTestInitFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}

