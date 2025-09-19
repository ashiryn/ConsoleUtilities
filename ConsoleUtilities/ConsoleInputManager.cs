using System;
using System.Text;
using FluffyVoid.Text;
using FluffyVoid.Text.AutoCompletion;

namespace FluffyVoid.ConsoleUtilities
{
    /// <summary>
    ///     Input manager for use in managing a console window
    /// </summary>
    public class ConsoleInputManager
    {
        /// <summary>
        ///     Event used to indicate that a function key has been pressed
        /// </summary>
        public event Action<object, InputEventArgs>? FunctionKeyPressed;
        /// <summary>
        ///     Event used to indicate that the enter key has been pressed
        /// </summary>
        public event Action<object, InputCompleteEventArgs>? InputCompleted;
        /// <summary>
        ///     Event used to indicate that an alpha-numeric key has been pressed
        /// </summary>
        public event Action<object, InputEventArgs>? KeyPressed;

        /// <summary>
        ///     Auto complete detector to use when retrieving auto complete suggestions
        /// </summary>
        private readonly AutoComplete _autoComplete;
        /// <summary>
        ///     History manager that tracks the input text that a user has input, allowing for up/down re-entry of text
        /// </summary>
        private readonly TextHistory _history;
        /// <summary>
        ///     The current text that has been input into the console window
        /// </summary>
        private readonly StringBuilder _inputBuilder;
        /// <summary>
        ///     The cached input that has been input into the console window, allowing for the display of auto complete suggestions
        ///     without losing a reference to what the user has input
        /// </summary>
        private string _cachedInput;
        /// <summary>
        ///     Whether the console window should refresh what input is currently being displayed to the console window
        /// </summary>
        private bool _shouldRefresh;
        /// <summary>
        /// Lock object for multithreaded console access
        /// </summary>
        private readonly object _consoleLock = new object();

        /// <summary>
        /// Erases the last line of the console window to emulate a bottom-to-top console display
        /// </summary>
        public void EraseLastLine()
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
            }
        }
        /// <summary>
        ///     Constructor used to initialize the manager
        /// </summary>
        /// <param name="autoCompleteDetector">Auto complete detector to use when retrieving auto complete suggestions</param>
        /// <param name="maxHistory">The maximum number of input texts to remember</param>
        public ConsoleInputManager(IAutoCompleteDetector autoCompleteDetector, int maxHistory)
        {
            _inputBuilder = new StringBuilder();
            _history = new TextHistory(maxHistory);
            _autoComplete = new AutoComplete(autoCompleteDetector);
            _shouldRefresh = false;
            _cachedInput = string.Empty;
        }

        /// <summary>
        ///     Updates the console window after detecting each key press, firing off available events as needed
        /// </summary>
        public virtual void Update()
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch(keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        if(_inputBuilder.Length > 0)
                        {
                            InputCompleted?.Invoke(this, new InputCompleteEventArgs(keyInfo.Key, _inputBuilder.ToString()));
                            _history.AddHistory(_inputBuilder.ToString());
                            ClearInput();
                        }

                        break;
                    case ConsoleKey.Escape:
                        if(string.IsNullOrEmpty(_cachedInput))
                        {
                            ClearInput();
                        }
                        else
                        {
                            _inputBuilder.Clear();
                            _shouldRefresh = true;
                            _inputBuilder.Append(_cachedInput);
                            _cachedInput = string.Empty;
                        }

                        break;
                    case ConsoleKey.Backspace:
                        if(_inputBuilder.Length > 0)
                        {
                            _inputBuilder.Remove(_inputBuilder.Length - 1, 1);
                        }

                        QueueForRefresh();

                        break;
                    case ConsoleKey.Tab:
                        if(string.IsNullOrEmpty(_cachedInput))
                        {
                            _cachedInput = _inputBuilder.ToString();
                        }

                        _inputBuilder.Clear();
                        _shouldRefresh = true;
                        _inputBuilder.Append(_autoComplete.GetSuggestion(_cachedInput));

                        break;
                    case ConsoleKey.UpArrow:
                        ClearInput();
                        _inputBuilder.Append(_history.GetPreviousEntry());

                        break;
                    case ConsoleKey.DownArrow:
                        ClearInput();
                        _inputBuilder.Append(_history.GetNextEntry());

                        break;
                    case ConsoleKey.F1:
                    case ConsoleKey.F2:
                    case ConsoleKey.F3:
                    case ConsoleKey.F4:
                    case ConsoleKey.F5:
                    case ConsoleKey.F6:
                    case ConsoleKey.F7:
                    case ConsoleKey.F8:
                    case ConsoleKey.F9:
                    case ConsoleKey.F10:
                    case ConsoleKey.F11:
                    case ConsoleKey.F12:
                        FunctionKeyPressed?.Invoke(this, new InputEventArgs(keyInfo.Key));

                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.B:
                    case ConsoleKey.C:
                    case ConsoleKey.D:
                    case ConsoleKey.E:
                    case ConsoleKey.F:
                    case ConsoleKey.G:
                    case ConsoleKey.H:
                    case ConsoleKey.I:
                    case ConsoleKey.J:
                    case ConsoleKey.K:
                    case ConsoleKey.L:
                    case ConsoleKey.M:
                    case ConsoleKey.N:
                    case ConsoleKey.O:
                    case ConsoleKey.P:
                    case ConsoleKey.Q:
                    case ConsoleKey.R:
                    case ConsoleKey.S:
                    case ConsoleKey.T:
                    case ConsoleKey.U:
                    case ConsoleKey.V:
                    case ConsoleKey.W:
                    case ConsoleKey.X:
                    case ConsoleKey.Y:
                    case ConsoleKey.Z:
                    case ConsoleKey.D0:
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                    case ConsoleKey.D6:
                    case ConsoleKey.D7:
                    case ConsoleKey.D8:
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad0:
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.Multiply:
                    case ConsoleKey.Add:
                    case ConsoleKey.Separator:
                    case ConsoleKey.Subtract:
                    case ConsoleKey.Decimal:
                    case ConsoleKey.Divide:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.OemComma:
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.OemPeriod:
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Oem1:
                    case ConsoleKey.Oem2:
                    case ConsoleKey.Oem3:
                    case ConsoleKey.Oem4:
                    case ConsoleKey.Oem5:
                    case ConsoleKey.Oem6:
                    case ConsoleKey.Oem7:
                    case ConsoleKey.Oem8:
                        KeyPressed?.Invoke(this, new InputEventArgs(keyInfo.Key));
                        _inputBuilder.Append(keyInfo.KeyChar);
                        QueueForRefresh();

                        break;
                }
            }

            if(Console.CursorLeft != _inputBuilder.Length || _shouldRefresh)
            {
                _shouldRefresh = false;
                EraseLastLine();
                Console.Write(_inputBuilder.ToString());
            }
        }

        /// <summary>
        ///     Clears the input field to allow new text to be entered
        /// </summary>
        private void ClearInput()
        {
            _inputBuilder.Clear();
            QueueForRefresh();
        }
        /// <summary>
        ///     Signals that the console window should refresh the input field after text has been entered
        /// </summary>
        private void QueueForRefresh()
        {
            _shouldRefresh = true;
            _cachedInput = string.Empty;
        }
    }
}
