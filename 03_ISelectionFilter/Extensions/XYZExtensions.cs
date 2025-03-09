using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions
{
    public static class XYZExtensions
    {

        /// <summary>
        /// This method is used to visualize XYZ in a document
        /// </summary>
        /// <param name="point"></param>
        /// <param name="document"></param>
        public static void Visualize(
            this XYZ point, Document document)
        {
            document.CreateDirectShape(new List<GeometryObject>() { Point.Create(point)});
        }

        /// <summary>
        /// This method is used to get a curve from vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="document"></param>
        /// <param name="origin"></param>
        public static Curve AsCurve(
            this XYZ vector, XYZ origin = null, double? lenght = null)
        {
            if (origin == null ) origin = XYZ.Zero;

            if (lenght == null) lenght = vector.GetLength();

            Line line = Line.CreateBound(origin, origin.MoveAlongVector(vector.Normalize(), lenght.GetValueOrDefault()));

            return line;
        }

        /// <summary>
        /// This method is used to move the point along a given vector and distance
        /// </summary>
        /// <param name="pointToMove"></param>
        /// <param name="vector"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static XYZ MoveAlongVector(
            this XYZ pointToMove, XYZ vector, double distance) => pointToMove.Add(vector * distance);

        /// <summary>
        /// This method is used to move the point along a given vector
        /// </summary>
        /// <param name="pointToMove"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static XYZ MoveAlongVector(
            this XYZ pointToMove, XYZ vector) => pointToMove.Add(vector);

        /// <summary>
        /// This method is to get normalized vector by curve
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static XYZ ToNormalizedVector(this Curve curve)
        {
            return (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();
        }

        public static XYZ ToVector(
            this XYZ fromPoint, XYZ toPoint)
        {
            return toPoint - fromPoint;
        }
        
        public static XYZ ToNormalizedVector(
            this XYZ fromPoint, XYZ toPoint)
        {
            return (toPoint - fromPoint).Normalize();
        }

        public static double DistanceToAlongVector(
            this XYZ fromPoint, XYZ toPoint, XYZ vectorToMeasureBy)
        {
            XYZ vector = fromPoint.ToVector(toPoint);
            double distance = vector.DotProduct(vectorToMeasureBy);
            return Math.Abs(distance);
        }
    }
}
