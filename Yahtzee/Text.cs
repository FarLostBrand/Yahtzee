namespace Yahtzee
{
    /// <summary>
    /// Class for making text with color easy and efficient.
    /// </summary>
    public static class Text
    {
        /// <summary>
        /// Prints text with fancy colors then goes to next line.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="foregroundColor">Color of the text.</param>
        public static void WriteLine(string text, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        
        /// <summary>
        /// Prints text with fancy colors then goes to next line.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="foregroundColor">Color of the text.</param>
        /// <param name="backgroundColor">Color behind the text.</param>
        public static void WriteLine(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    
        /// <summary>
        /// Prints text with fancy colors.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="foregroundColor">Color of the text.</param>
        /// <param name="backgroundColor">Color behind the text.</param>
        public static void Write(string text, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ResetColor();
        }
        
        /// <summary>
        /// Prints text with fancy colors.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="foregroundColor">Color of the text.</param>
        /// <param name="backgroundColor">Color behind the text.</param>
        public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ResetColor();
        }
    
        /// <summary>
        /// Gets user input with typing colors.
        /// </summary>
        /// <param name="foregroundColor">Color of the text.</param>
        /// <param name="backgroundColor">Color behind the text.</param>
        /// <returns>Text that user types.</returns>
        public static string? ReadLine(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            string? text = Console.ReadLine();
            Console.ResetColor();
            return text;
        }
        
        /// <summary>
        /// Gets user input with typing colors.
        /// </summary>
        /// <param name="foregroundColor">Color of the text.</param>
        /// <param name="backgroundColor">Color behind the text.</param>
        /// <returns>Text that user types.</returns>
        public static string? ReadLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            string? text = Console.ReadLine();
            Console.ResetColor();
            return text;
        }
    }
}