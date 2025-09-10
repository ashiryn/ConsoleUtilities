using System;

namespace FluffyVoid.ConsoleUtilities
{
    /// <summary>
    ///     Event arg class for key pressed events
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        /// <summary>
        ///     The key that was pressed in the console window
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ConsoleKey KeyPressed { get; }

        /// <summary>
        ///     Constructor used to initialize the event args
        /// </summary>
        public InputEventArgs(ConsoleKey keyPressed)
        {
            KeyPressed = keyPressed;
        }
    }
}
