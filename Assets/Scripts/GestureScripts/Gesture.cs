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

using System;
using System.Diagnostics;
using UnityEngine;
using Windows.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a Kinect gesture.
    /// </summary>
    public class Gesture
    {
        #region Constants

        /// <summary>
        /// The window size.
        /// This *should* be the number of frames for a segment of a gesture.
        /// </summary>
        readonly int WINDOW_SIZE = Mathf.RoundToInt(Application.targetFrameRate * 0.5f);

        #endregion

        #region Members

        /// <summary>
        /// The current gesture segment we are matching against.
        /// </summary>
        int _currentSegment = 0;

        /// <summary>
        /// The current frame.
        /// </summary>
        int _frameCount = 0;

        readonly Stopwatch _stopwatch = new();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/>.
        /// </summary>
        public Gesture()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/> with the specified type and segments.
        /// </summary>
        /// <param name="type">The type of gesture.</param>
        /// <param name="segments">The segments of the gesture.</param>
        public Gesture(GestureType type, IGestureSegment[] segments)
        {
            GestureType = type;
            Segments = segments;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognised.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Properties

        /// <summary>
        /// The type of the current gesture.
        /// </summary>
        public GestureType GestureType { get; set; }

        /// <summary>
        /// The segments which form the current gesture.
        /// </summary>
        public IGestureSegment[] Segments { get; set; }

        public long TimeElapsed { get; private set; } = -1;

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body data.</param>
        public void Update(Body body)
        {
            GesturePartResult result = Segments[_currentSegment].Update(body);

            if (result == GesturePartResult.Succeeded)
            {
                if(_currentSegment == Segments.Length - 1)
                {
                    // The gesture has been recognised.
                    if (GestureRecognized != null)
                    {
                        TimeElapsed = _stopwatch.ElapsedMilliseconds;
                        _stopwatch.Reset();
                        GestureRecognized(this, new GestureEventArgs(GestureType, body.TrackingId));
                    }
                }
                else
                {
                    // If the next segment is the last segment.
                    if (_currentSegment == Segments.Length - 2)
                    {
                        _stopwatch.Restart();
                    }

                    _currentSegment++;
                    _frameCount = 0;
                }
            }
            else if (result == GesturePartResult.Failed || _frameCount == WINDOW_SIZE)
            {
                Reset();
            }
            // Undetermined result found within the alloted window time.
            else
            {
                _frameCount++;
            }
        }

        #endregion
    }
}