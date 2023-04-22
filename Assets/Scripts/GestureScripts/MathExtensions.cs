//
// Copyright (c) LightBuzz Software.
// All rights reserved.
//
// http://lightbuzz.com
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
// AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using Windows.Kinect;
using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common mathematical extensions for Kinect data.
    /// </summary>
    public static class MathExtensions
    {
        #region Lengths

        /// <summary>
        /// Calculates the length of the specified 3-D point.
        /// </summary>
        /// <param name="point">The specified 3-D point.</param>
        /// <returns>The corresponding length, in meters.</returns>
        public static double Length(this CameraSpacePoint point)
        {
            return Math.Sqrt(
                Math.Pow(point.X, 2) +
                Math.Pow(point.Y, 2) +
                Math.Pow(point.Z, 2)
            );
        }

        /// <summary>
        /// Returns the length of the segment defined by the specified points.
        /// </summary>
        /// <param name="point1">The first point (start of the segment).</param>
        /// <param name="point2">The second point (end of the segment).</param>
        /// <returns>The length of the segment (in meters).</returns>
        public static double Length(this CameraSpacePoint point1, CameraSpacePoint point2)
        {
            return Math.Sqrt(
                Math.Pow(point1.X - point2.X, 2) +
                Math.Pow(point1.Y - point2.Y, 2) +
                Math.Pow(point1.Z - point2.Z, 2)
            );
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified points.
        /// </summary>
        /// <param name="points">A collection of two or more points.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Length(params CameraSpacePoint[] points)
        {
            double length = 0;

            for (int index = 0; index < points.Length - 1; index++)
            {
                length += Length(points[index], points[index + 1]);
            }

            return length;
        }

        #endregion
    }
}