using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;

namespace RhinoPluginTests
{
    /// <summary>
    /// Checking to see if offset curve works
    /// https://discourse.mcneel.com/t/curve-offset-and-mstest/66042
    /// </summary>
    [TestClass]
    public class OffsetTests
    {
        /// <summary>
        /// Offset a straight line drawn in XY plane
        /// </summary>
        [TestMethod]
        public void Rhino_Offset()
        {
            // Arrange
            var line = new LineCurve(new Point3d(), new Point3d(10, 0, 0));
            var plane = new Plane(new Point3d(), Vector3d.ZAxis, Vector3d.YAxis);

            // Act
            var offsetLines = line.Offset(plane, 2.5, 0.001, CurveOffsetCornerStyle.None);
            //var offsetLines = line.Offset(new Point3d(0,0,2.5), Vector3d.YAxis, 2.5, 0.001, CurveOffsetCornerStyle.None);

            // Assert
            Assert.IsNotNull(offsetLines, "Offset failed");
            Assert.AreEqual(1, offsetLines.Length, "Wrong number of curves returned");
            Assert.AreEqual(2.5, offsetLines[0].PointAtStart.Z, "Wrong z value");
        }
    }
}
