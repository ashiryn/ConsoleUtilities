using System;

namespace FluffyVoid.ConsoleUtilities
{
    /// <summary>
    ///     Event arg class for holding a completed string entry
    /// </summary>
    public class InputCompleteEventArgs : InputEventArgs
    {
        /// <summary>
        ///     The full string that was entered into the console window
        /// </summary>
        public string Input { get; }

        /// <summary>
        ///     Constructor used to initialize the event args
        /// </summary>
        /// <param name="keyPressed">The last key that was pressed in the console window</param>
        /// <param name="input">The full string that was entered into the console window</param>
        public InputCompleteEventArgs(ConsoleKey keyPressed, string input)
            : base(keyPressed)
        {
            Input = input;
        }
    }
}
