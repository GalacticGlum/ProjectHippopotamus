using System;
using System.Collections.Generic;
using System.Linq;

namespace Hippopotamus.Engine.Core
{
    public class GameTimer
    {
        public int MaximumBufferSamples { get; set; } = 100;
        public DateTime StartTime { get; }

        public float FramesPerSecond { get; private set; }
        public float UpdatesPerSecond { get; private set; }

        /// <summary>
        /// Amount of frames since start.
        /// </summary>
        public long FrameCount { get; private set; }
        /// <summary>
        /// Amount of time spent rendering (in seconds) since start.
        /// </summary>
        public float ElapsedFrameTime { get; private set; }

        /// <summary>
        /// Amount of updates since start.
        /// </summary>
        public long UpdateCount { get; private set; }
        /// <summary>
        /// Amount of time spent updating (in seconds) since start.
        /// </summary>
        public float ElapsedUpdateTime { get; private set; }

        private float currentFramesPerSecond;
        private readonly Queue<float> frameSampleBuffer;

        private float currentUpdatesPerSeconds;
        private readonly Queue<float> updateSampleBuffer;

        internal GameTimer()
        {
            frameSampleBuffer = new Queue<float>();
            updateSampleBuffer = new Queue<float>();

            StartTime = DateTime.Now;
        }

        internal void ProcessFrameCalculation(float deltaTime)
        {
            currentFramesPerSecond = 1.0f / deltaTime;
            frameSampleBuffer.Enqueue(currentFramesPerSecond);

            if (frameSampleBuffer.Count > MaximumBufferSamples)
            {
                frameSampleBuffer.Dequeue();
                FramesPerSecond = frameSampleBuffer.Average(fps => fps);
            }
            else
            {
                FramesPerSecond = currentFramesPerSecond;
            }

            FrameCount++;
            ElapsedFrameTime += deltaTime;
        }

        internal void ProcessUpdateCalculation(float deltaTime)
        {
            currentUpdatesPerSeconds = 1.0f / deltaTime;
            updateSampleBuffer.Enqueue(currentUpdatesPerSeconds);

            if (updateSampleBuffer.Count > MaximumBufferSamples)
            {
                updateSampleBuffer.Dequeue();
                UpdatesPerSecond = updateSampleBuffer.Average(ups => ups);
            }
            else
            {
                UpdatesPerSecond = currentUpdatesPerSeconds;
            }

            UpdateCount++;
            ElapsedUpdateTime += deltaTime;
        }
    }
}
