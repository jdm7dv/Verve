using System;

namespace TFSLocalBuildAgentLib
{
    /// <summary>
    /// The event fired when output is received.
    /// </summary>
    public class OutputReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the event data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the OutputReceivedEventArgs class.
        /// </summary>
        /// <param name="outputData">The event data.</param>
        public OutputReceivedEventArgs(string outputData)
        {
            Data = outputData;
        }
    }
}
