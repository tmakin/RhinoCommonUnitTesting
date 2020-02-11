using System;

using Rhino.Geometry;

using Xunit;

namespace RhinoPlugin.Tests.Xunit
{
    [Collection("Rhino Collection")]
    public class XunitExampleTests
    {

        /// <summary>
        /// Xunit Test to Transform a brep using a translation
        /// </summary>
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

    }
}
