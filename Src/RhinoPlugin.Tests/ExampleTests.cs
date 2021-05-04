using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace RhinoPluginTests
{
    /// <summary>
    /// Example test methods that invoke RhinoCommon and Grasshopper
    /// </summary>
    [TestClass]
    public class ExampleTests
    {
        /// <summary>
        /// Transform a brep using a translation
        /// </summary>
        [TestMethod]
        public void Brep_Translation()
        {
            // Arrange
            var bb = new BoundingBox(new Point3d(0, 0, 0), new Point3d(100, 100, 100));
            var brep = bb.ToBrep();
            var t = Transform.Translation(new Vector3d(30, 40, 50));

            // Act
            brep.Transform(t);

            // Assert
            Assert.AreEqual(brep.GetBoundingBox(true).Center, new Point3d(80, 90, 100));
        }

        /// <summary>
        /// Intersect sphere with a plane to generate a circle
        /// </summary>
        [TestMethod]
        public void Brep_Intersection()
        {
            // Arrange
            var radius = 4.0;
            var brep = Brep.CreateFromSphere(new Sphere(new Point3d(), radius));
            var cuttingPlane = Plane.WorldXY;

            // Act
            Rhino.Geometry.Intersect.Intersection.BrepPlane(brep, cuttingPlane, 0.001, out var curves, out var points);

            // Assert
            Assert.AreEqual(1, curves.Length, "Wrong curve count");
            Assert.AreEqual(2 * Math.PI * radius, curves[0].GetLength(), "Wrong curve length");
        }

        /// <summary>
        /// Xunit Test to ensure Centroid of GH_Box outputs a GH_Point
        /// </summary>
        [TestMethod]
        public void GHBox_Centroid_ReturnsGHPoint()
        {
            // Arrange
            var myBox = new GH_Box(new Box());

            // Act 
            var result = myBox.Boundingbox.Center;

            // Assert
            Assert.IsInstanceOfType(result, typeof(Point3d));
        }
    }
}
