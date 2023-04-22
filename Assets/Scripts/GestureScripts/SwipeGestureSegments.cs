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

namespace LightBuzz.Vitruvius.Gestures
{
    /// <summary>
    /// The first part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z)
            {
                // Debug.Log("GesturePart 1 - left hand in front of left shoulder - PASS");
                // left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.Log("GesturePart 1 - left hand below head height but above hip height - PASS");
                    // left hand left of left Shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        // Debug.Log("GesturePart 1 - left hand left of left shoulder - PASS - NEXT");
                        return GesturePartResult.Succeeded;
                    }
                }

                // Debug.Log("GesturePart 1 - left hand below head height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.Log("GesturePart 1 - left hand in front of left shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z) // && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.Log("GesturePart 2 - left hand in front of left shoulder - PASS");
                // Left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.Log("GesturePart 2 - left hand below head height but above hip height - PASS");
                    // Left hand between shoulders.
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        // Debug.Log("GesturePart 2 - left hand between shoulders - PASS - NEXT");
                        return GesturePartResult.Succeeded;
                    }

                    // Debug.Log("GesturePart 2 - left hand between shoulders - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }

                // Debug.Log("GesturePart 2 - left hand below head height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.Log("GesturePart 2 - left hand in front of left shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The third part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z) // && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.Log("GesturePart 3 - left hand in front of left shoulder - PASS");
                // left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.Log("GesturePart 3 - left hand below head height but above hip height - PASS");
                    // left hand left of left Shoulder
                    if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Debug.Log("GesturePart 3 - left hand beyond right shoulder - PASS - DONE");
                        return GesturePartResult.Succeeded;
                    }
                    
                    if(body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Failed;
                    }

                    // Debug.Log("GesturePart 3 - left hand beyond right shoulder - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }
                // Debug.Log("GesturePart 3 - left hand below head height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }
            //  Debug.Log("GesturePart 3 - left hand in front of left shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }
}