using System;

using Rhino.Geometry;
using Grasshopper.Kernel.Types;

using Xunit;


namespace RhinoPlugin.Tests.RHInside
{
    /// <summary>
    /// XUnit tests
    /// </summary>
    [Collection("RhinoTestingCollection")]
    public class XunitExampleTestsRI 
    {
        XunitTestFixtureRI _fixture;

        public XunitExampleTestsRI(XunitTestFixtureRI fixture)
        {
           fixture = _fixture;
        }
       
        [Fact]
        public void Brep_Translation()
        {

                // Arrange
                var bb = new BoundingBox(new Point3d(0, 0, 0), new Point3d(100, 100, 100));
                var brep = bb.ToBrep();
                var t = Transform.Translation(new Vector3d(30, 40, 50));

                // Act
                brep.Transform(t);

                // Assert
                Assert.Equal(brep.GetBoundingBox(true).Center, new Point3d(80, 90, 100));
  
        }

        /// <summary>
        /// Xunit Test to Intersect sphere with a plane to generate a circle
        /// </summary>
        [Fact]
        public void Brep_Intersection()
        {
            // Arrange
            var radius = 4.0;
            var brep = Brep.CreateFromSphere(new Sphere(new Point3d(), radius));
            var cuttingPlane = Plane.WorldXY;

            // Act
            Rhino.Geometry.Intersect.Intersection.BrepPlane(brep, cuttingPlane, 0.001, out var curves, out var points);

            // Assert
            Assert.Single(curves);
            Assert.Equal(2 * Math.PI * radius, curves[0].GetLength());
        }

        /// <summary>
        /// Xunit Test to ensure Centroid of GH_Box outputs a GH_Point
        /// </summary>
        [Fact]
        public void GHBox_Centroid_ReturnsGHPoint()
        {
            // Arrange
            var myBox = new GH_Box(new Box());

            // Act 
            var result = myBox.Boundingbox.Center;

            // Assert
            Assert.IsType<Point3d>(result);
        }

    }
}