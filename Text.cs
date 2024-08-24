using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yahtzee
{
    //===================================================
    //Text
    //Class for making text with color easy and efficient
    //===================================================
    public static class Text
    {
        //====================================================
        //WriteLine
        //Prints text with fancy colors then goes to next line
        //====================================================
        public static void WriteLine(string text = "", ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        //=============================
        //Write
        //Prints text with fancy colors
        //=============================
        public static void Write(string text = "", ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ResetColor();
        }

        //=================================
        //ReadLine
        //Gets text input with fancy colors
        //=================================
        public static string? ReadLine(ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            string? text = Console.ReadLine();
            Console.ResetColor();
            return text;
        }
    }
}