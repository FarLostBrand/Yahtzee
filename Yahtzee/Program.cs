using System;
using System.Runtime.InteropServices;

namespace Yahtzee
{
    //==================================
    // Brandon Boros
    // 2377215
    // Programming 2
    // Final Project
    //
    // TESTERS: Jase, Salvador and Émile
    //==================================
    class Program
    {
        static void Main(string[] args)
        {
            //color constants used in the main
            const ConsoleColor ErrorColor = ConsoleColor.Red;
            const ConsoleColor GameOverColor = ConsoleColor.Red;
            const ConsoleColor ScoreColor = ConsoleColor.Green;

            //combination related
            bool previousYahtzee = false;
            const int numberOfCombinations = 14;
            bool[] combinationIsDone = new bool[numberOfCombinations]; //whether or not a combination has been done before. Replaces the backing field.
            int[] score = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            //not combination related
            Turn[] turns = [new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn(), new Turn()];
            int totalScore = 0;

            //start of game    
            for (int i = 0; i < turns.Length; i++) //uses a for loop since there is a limited amount of rounds that it know.
            {
                bool roundIsRunning = true; //when this swithces to false a new round begins
                ConsoleKeyInfo key; //this makes it possible to record what keys are pressed.

                try
                {
                    turns[i].RollDice(); //rerolls the dice at the start of the turn to get a fresh batch of die.
                }
                catch(Exception e)
                {
                    Text.WriteLine($"{e.Message}", ErrorColor);
                }

                do //this is the start of round x
                {
                    Console.Clear();

                    int[] possibleScore = RunCombinations(turns[i], combinationIsDone, ref previousYahtzee); //gets all the possible scores

                    //prints the board, split into different functions for efficiency
                    PrintScoreBoard(turns, possibleScore, combinationIsDone, score, i+1, ref totalScore);
                    PrintDice();
                    PrintOptions(turns[i].TotalRolls);

                    key = Console.ReadKey(); //your choice to the prompt
                    if (key.Key != ConsoleKey.Escape) //if the secret end game key is not clicked
                    {
                        if(key.Key == ConsoleKey.H) //if the hold/unhold uption was clicked
                            HoldAndUnhold();
                        else if(key.Key == ConsoleKey.R) //if the reroll is chosen
                            Reroll(turns[i]);
                        else if(key.Key == ConsoleKey.P) //if the submit  
                            roundIsRunning = EndingRound(combinationIsDone, possibleScore, score, ref previousYahtzee);
                    }
                    else
                    {
                        i = turns.Length-1; //just ends the game and skips the remaining rounds
                        roundIsRunning = false;
                    }

                }while (roundIsRunning);
            }
            
            //end scene
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth/2-9,Console.WindowHeight/2-1); //this is just math to center the text below
            Text.WriteLine("Game Over", GameOverColor);
            Console.SetCursorPosition(Console.WindowWidth/2-9,Console.WindowHeight/2); //this is just math to center the text below
            Text.WriteLine($"Score: {totalScore}", ScoreColor);
            Console.ReadKey();
        }

        /// <summary>
        /// Function to hold and unhold dice.
        /// </summary>
        static void HoldAndUnhold()
        {
            const ConsoleColor OptionCaptionColor = ConsoleColor.DarkMagenta;
            const ConsoleColor OptionColor = ConsoleColor.Magenta;

            ConsoleKeyInfo key; //this makes it possible to record what keys are pressed.
            Text.WriteLine("\n\nEnter the die numbers you wish to hold (1-5) or press enter to exit.", OptionCaptionColor);

            do
            {
                key = Console.ReadKey(); //read all the possible keys for holding/unholding (1-5)
                if(key.Key != ConsoleKey.Enter)
                {
                    switch(key.Key) //all of these toggle between hold and unhold
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            if(Turn.DiceArray[0].IsHeld)
                                Turn.DiceArray[0].IsHeld = false;
                            else
                                Turn.DiceArray[0].IsHeld = true;

                            Text.WriteLine($"\nDie 1 hold has been set to {Turn.DiceArray[0].IsHeld}", OptionColor);
                        break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            if(Turn.DiceArray[1].IsHeld)
                                Turn.DiceArray[1].IsHeld = false;
                            else
                                Turn.DiceArray[1].IsHeld = true;

                            Text.WriteLine($"\nDie 2 hold has been set to {Turn.DiceArray[1].IsHeld}", OptionColor);
                        break;

                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            if(Turn.DiceArray[2].IsHeld)
                                Turn.DiceArray[2].IsHeld = false;
                            else
                                Turn.DiceArray[2].IsHeld = true;

                            Text.WriteLine($"\nDie 3 hold has been set to {Turn.DiceArray[2].IsHeld}", OptionColor);
                        break;

                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4:
                            if(Turn.DiceArray[3].IsHeld)
                                Turn.DiceArray[3].IsHeld = false;
                            else
                                Turn.DiceArray[3].IsHeld = true;

                            Text.WriteLine($"\nDie 4 hold has been set to {Turn.DiceArray[3].IsHeld}", OptionColor);
                        break;

                        case ConsoleKey.D5:
                        case ConsoleKey.NumPad5:
                            if(Turn.DiceArray[4].IsHeld)
                                Turn.DiceArray[4].IsHeld = false;
                            else
                                Turn.DiceArray[4].IsHeld = true;

                            Text.WriteLine($"\nDie 5 hold has been set to {Turn.DiceArray[4].IsHeld}", OptionColor);
                        break;
                    }
                }
                
            }while (key.Key != ConsoleKey.Enter); //if they click enter, they are done configurating the hold
        }

        /// <summary>
        /// Function to reroll the dice.
        /// </summary>
        /// <param name="turn">The current turn the game is on.</param>
        static void Reroll(Turn turn)
        {
            const ConsoleColor ErrorColor = ConsoleColor.Red;

            try
            {
                if(turn.TotalRolls > 0) //and theres enough rolls
                {
                    try
                    {
                        turn.RollDice(); //rerolls the dice at the start of the turn to get a fresh batch of die.
                    }
                    catch(Exception e)
                    {
                        Text.WriteLine($"{e.Message}", ErrorColor);
                    }
                }
                else
                {
                    const int SleepTime = 50;
                    string continueMessage = "Press any key to continue...";

                    Text.WriteLine("\nYou have 0 Rolls Remaining", ErrorColor);
                    WritingEffect(continueMessage, SleepTime);
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Text.WriteLine($"{e.Message}", ErrorColor);
            }
        }

        /// <summary>
        /// Submits and finalizes this turn's score. Prepares for next turn
        /// </summary>
        /// <param name="combinationIsDone">Has the combination been done.</param>
        /// <param name="possibleScore">Array of all the possible scores.</param>
        /// <param name="score">Array of all the finalized scores.</param>
        /// <param name="previousYahtzee">Has there been a previous Yahtzee? yes or no?</param>
        /// <returns>A booleen deciding if the game has ended or not.</returns>
        static bool EndingRound(bool[] combinationIsDone, int[] possibleScore, int[] score, ref bool previousYahtzee)
        {
            const int min = 1;
            ConsoleKeyInfo key; //this makes it possible to record what keys are pressed.
            int max;
            string message = "\n\nEnter the number of the option you want to select (1-13). \n - 14 is reserved for a second Yahtzee.\n - Select something with nothing if you can't do anything!\nOption";
            string errorMessage = "Invalid option. Option may have been chosen previously. Bonus Yahzee cannot be set to 0.";

            if(combinationIsDone[(int)Combination.Yahtzee] && possibleScore[(int)Combination.BonusYahtzee] > 0)
                max = 14; //allows user to now use the bonus Yahtzee
            else
                max = 13; //this is because the user cannot use the bonus Yahtzee as a free 0 space

            const int SleepTime = 25;
            string confirmationMessage = "\nAre you sure you want to proceed? Press space to proceed...";

            WritingEffect(confirmationMessage, SleepTime);
            key = Console.ReadKey();

            if(key.Key == ConsoleKey.Spacebar)
            {
                int option = InputValidation(message, errorMessage, min, max, combinationIsDone);

                combinationIsDone[option-1] = true;
                score[option-1] = possibleScore[option-1];

                foreach(Dice die in Turn.DiceArray)
                {
                    die.IsHeld = false;
                }

                if(option-1 == (int)Combination.Yahtzee)
                    previousYahtzee = true;

                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Makes sure input is an integer.
        /// </summary>
        /// <param name="message">Opening message</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="combinationIsDone">Has the combination been done? Is an array.</param>
        /// <returns>The input value as an integer.</returns>
        static int InputValidation(string message, string errorMessage, int min, int max, bool[] combinationIsDone)
        {
            const ConsoleColor MessageColor = ConsoleColor.DarkCyan;
            const ConsoleColor ErrorColor = ConsoleColor.Red;
            const ConsoleColor InputColor = ConsoleColor.Green;

            Text.Write($"{message}: ", MessageColor);
            bool isValid = int.TryParse(Text.ReadLine(InputColor), out int value);

            while(!isValid || value > max || value < min || combinationIsDone[value-1] == true)
            {
                Text.WriteLine($"{errorMessage}", ErrorColor);
                Text.Write($"{message}: ", MessageColor);
                isValid = int.TryParse(Text.ReadLine(InputColor), out value);

                if (value == 0)
                    return value;
            }

            return value;
        }

        /// <summary>
        /// Fun 'Press any key to continue effect'
        /// </summary>
        static void WritingEffect(string message, int SleepTime)
        {
            const ConsoleColor WritingEffectColor = ConsoleColor.Gray;

            foreach(char c in message) //for every character
            {
                Text.Write($"{c}", WritingEffectColor); //print it
                Thread.Sleep(SleepTime); //then wait
            }
        }

        /// <summary>
        /// Runs each check and adds their scores to the possible score array.
        /// </summary>
        /// <param name="turn">The object of Turn</param>
        /// <param name="combinationIsDone"><Has the combination been done? Is an array./param>
        /// <returns>An integer array of the possible scores.</returns>
        static int[] RunCombinations(Turn turn, bool[] combinationIsDone, ref bool previousYahtzee)
        {
            const int ScoreArraySize = 14;

            int[] possibleScore = new int[ScoreArraySize];
            
            if(!combinationIsDone[(int)Combination.Ones])
                possibleScore[(int)Combination.Ones] = turn.Ones();

            if(!combinationIsDone[(int)Combination.Twos])
                possibleScore[(int)Combination.Twos] = turn.Twos();

            if(!combinationIsDone[(int)Combination.Threes])
                possibleScore[(int)Combination.Threes] = turn.Threes();

            if(!combinationIsDone[(int)Combination.Fours])
                possibleScore[(int)Combination.Fours] = turn.Fours();

            if(!combinationIsDone[(int)Combination.Fives])
                possibleScore[(int)Combination.Fives] = turn.Fives();

            if(!combinationIsDone[(int)Combination.Sixes])
                possibleScore[(int)Combination.Sixes] = turn.Sixes();

            if(!combinationIsDone[(int)Combination.ThreeOfKind])
                possibleScore[(int)Combination.ThreeOfKind] = turn.ThreeOfKind();

            if(!combinationIsDone[(int)Combination.FourOfKind])
                possibleScore[(int)Combination.FourOfKind] = turn.FourOfKind();

            if(!combinationIsDone[(int)Combination.SmallStraight])
                possibleScore[(int)Combination.SmallStraight] = turn.SmallStraight();

            if(!combinationIsDone[(int)Combination.LargeStraight])
                possibleScore[(int)Combination.LargeStraight] = turn.LargeStraight();

            if(!combinationIsDone[(int)Combination.FullHouse])
                possibleScore[(int)Combination.FullHouse] = turn.FullHouse();

            if(!combinationIsDone[(int)Combination.Chance])
                possibleScore[(int)Combination.Chance] = Turn.TotalValueOfDice;
            
            int yahtzeeScore = turn.Yahtzee(previousYahtzee);

            if (!previousYahtzee && !combinationIsDone[(int)Combination.Yahtzee])
            {
                possibleScore[(int)Combination.Yahtzee] = yahtzeeScore;
            }             
            else if(previousYahtzee && !combinationIsDone[(int)Combination.BonusYahtzee])
                possibleScore[(int)Combination.BonusYahtzee] = yahtzeeScore;

            return possibleScore;
        }

        /// <summary>
        /// Prints the Score Board.
        /// </summary>
        /// <param name="turns">The array of all the turns.</param>
        /// <param name="possibleScore">The possible score array.</param>
        /// <param name="combinationIsDone">Has the combination been done? Is an array.</param>
        /// <param name="score">The finalized array of scores</param>
        /// <param name="roundNumber">What round we are.</param>
        /// <param name="totalScore">The total score, is ref because used in end scene and returning this would be useless.</param>
        static void PrintScoreBoard(Turn[] turns, int[] possibleScore, bool[] combinationIsDone, int[] score, int roundNumber, ref int totalScore)
        {
            const ConsoleColor TitleColor = ConsoleColor.Blue;
            const ConsoleColor FrameColor = ConsoleColor.DarkYellow;
            const ConsoleColor FrameContentColor = ConsoleColor.Yellow;
            const ConsoleColor ScoreColor = ConsoleColor.Blue;
            const ConsoleColor BestPossibleScoreColor = ConsoleColor.Green;

            const int NumberDisplacement = -3;
            const int TypeDisplacement = -16;
            const int DescriptionDisplacement = -56;
            const int PossibleScoreDisplacement = -15;
            const int ScoreDisplacement = -6;

            totalScore = 0;

            string[] types = ["1s", "2s", "3s", "4s", "5s", "6s", "Three of a Kind", "Four of a Kind", "Small Straight", "Large Straight", "Full House", "Chance", "Yahtzee!", "Bonus Yahtzee!"];

            string[] descriptions = ["Sum of all 1s.", "Sum of all 2s.", "Sum of all 3s.", "Sum of all 4s.", "Sum of all 5s.", "Sum of all 6s.",
            "Sum of all numbers if there's 3 of a kind.", "Sum of all numbers if there's 4 of a kind.", "Score of 30 if there's 4 dice in sequential order.",
            "Score of 40 if there's 5 dice in sequential order.", "Score of 25 if there's 3 of a number, and 2 of another.", "Sum of all dice.", 
            "Score of 50 if all dice are the same number.", "Score of 100 only if there was a preivous Yahtzee."];
            
            int bestScoreIndex = GetHighestScore(possibleScore);

            //HEADER
            Text.Write($"{"Game Score Board | Round ", 65}", TitleColor);
            Text.WriteLine($"{roundNumber}", TitleColor);
            Text.WriteLine("╔════╦═════════════════╦═════════════════════════════════════════════════════════╦════════════════╦═══════╗", FrameColor);
            Text.Write($"║ ", FrameColor);
            Text.Write($"{"N", NumberDisplacement}", FrameContentColor);
            Text.Write($"║ ", FrameColor);
            Text.Write($"{"Type", TypeDisplacement}", FrameContentColor);
            Text.Write($"║ ", FrameColor);
            Text.Write($"{"Description", DescriptionDisplacement}", FrameContentColor);
            Text.Write($"║ ", FrameColor);
            Text.Write($"{"Possible Score", PossibleScoreDisplacement}", FrameContentColor);
            Text.Write($"║ ", FrameColor);
            Text.Write($"{"Score", ScoreDisplacement}", FrameContentColor);
            Text.WriteLine($"║", FrameColor);

            //EVERY OTHER ROW
            for(int i = 0; i < types.Length; i++)
            {
                Text.WriteLine("╠════╬═════════════════╬═════════════════════════════════════════════════════════╬════════════════╬═══════╣", FrameColor);
                Text.Write($"║ ", FrameColor);
                Text.Write($"{i+1, NumberDisplacement}", FrameContentColor);
                Text.Write($"║ ", FrameColor);
                Text.Write($"{types[i], TypeDisplacement}", FrameContentColor);
                Text.Write($"║ ", FrameColor);
                Text.Write($"{descriptions[i], DescriptionDisplacement}", FrameContentColor);
                Text.Write($"║ ", FrameColor);

                if(combinationIsDone[i])
                {
                    Console.Write($"{" ", PossibleScoreDisplacement}");
                    Text.Write($"║ ", FrameColor);
                    Text.Write($"{score[i], ScoreDisplacement}", ScoreColor);
                }
                else
                {
                    if(i == bestScoreIndex)
                        Text.Write($"{possibleScore[i], PossibleScoreDisplacement}", BestPossibleScoreColor);
                    else if (possibleScore[i] == 0)
                        Console.Write($"{" ", PossibleScoreDisplacement}");
                    else
                        Text.Write($"{possibleScore[i], PossibleScoreDisplacement}", FrameContentColor);
                    Text.Write($"║ ", FrameColor);
                    Console.Write($"{" ", ScoreDisplacement}"); //check to make sure 0 works
                }

                Text.WriteLine($"║", FrameColor);

                totalScore += score[i];
            }
            Text.WriteLine("╚════╩═════════════════╩═════════════════════════════════════════════════════════╩════════════════╩═══════╝", FrameColor);
            Text.Write($"Total: {totalScore}\n\n ", TitleColor); //yes this is weird but it's the best way to get 1 extra space on the dice frame line
        }
        
        /// <summary>
        /// Gets the index of the highest possible score so the board can highlight it green.
        /// </summary>
        /// <param name="possibleScore"></param>
        /// <returns>The index of the best possible score</returns>
        static int GetHighestScore(int[] possibleScore)
        {
            int bestScore = 0;
            int bestScoreIndex = 0;

            for(int i = 0; i < possibleScore.Length; i++)
            {
                bestScore = Math.Max(bestScore, possibleScore[i]);

                if(bestScore == possibleScore[i])
                    bestScoreIndex = i;
            }

            if(bestScore == 0)
                return -1;
            return bestScoreIndex;
        }
       
        /// <summary>
        /// Print Dice in a fancy way.
        /// </summary>
        static void PrintDice()
        {
            const ConsoleColor DiceFrameColor = ConsoleColor.DarkMagenta;
            const ConsoleColor DiceValueColor = ConsoleColor.Magenta;

            const ConsoleColor HeldDiceFrameColor = ConsoleColor.DarkRed;
            const ConsoleColor HeldDiceValueColor = ConsoleColor.Red;

            const int DiceFrameDisplacement = 10;
            const int DiceValueDisplacement = 7;

            for(int i = 0; i < Turn.DiceArray.Length; i++)
            {
                if(!Turn.DiceArray[i].IsHeld)
                    Text.Write($"[{i+1}] ╔═══╗ ", DiceFrameColor);
                else
                    Text.Write($"[{i+1}] ╔═══╗ ", HeldDiceFrameColor);
            }

            Console.WriteLine();

            for(int i = 0; i < Turn.DiceArray.Length; i++)
            {
                if(!Turn.DiceArray[i].IsHeld)
                {
                    Text.Write($"{"║ ", DiceValueDisplacement}", DiceFrameColor);
                    Text.Write($"{Turn.DiceArray[i].Number}", DiceValueColor);
                    Text.Write(" ║", DiceFrameColor);
                }
                else
                {
                    Text.Write($"{"║ ", DiceValueDisplacement}", HeldDiceFrameColor);
                    Text.Write($"{Turn.DiceArray[i].Number}", HeldDiceValueColor);
                    Text.Write(" ║", HeldDiceFrameColor);
                }
            }
            
            Console.WriteLine();

            for(int i = 0; i < Turn.DiceArray.Length; i++)
            {
                if(!Turn.DiceArray[i].IsHeld)
                    Text.Write($"{"╚═══╝", DiceFrameDisplacement}", DiceFrameColor);
                else
                    Text.Write($"{"╚═══╝", DiceFrameDisplacement}", HeldDiceFrameColor);
            }
        }
        
        /// <summary>
        /// Prints all the options for the user.
        /// </summary>
        /// <param name="rollsLeft">How many rolls remain.</param>
        static void PrintOptions(int rollsLeft)
        {
            const ConsoleColor OptionsCaptionColor = ConsoleColor.Blue;
            const ConsoleColor OptionsChoiceColor = ConsoleColor.DarkCyan;
            const ConsoleColor ExitColor = ConsoleColor.Red;

            Text.WriteLine("\n\nEnter:", OptionsCaptionColor);
            Text.WriteLine("H to Hold/Unhold", OptionsChoiceColor);
            Text.WriteLine($"R to Roll ({rollsLeft} Rolls Remain)", OptionsChoiceColor);
            Text.WriteLine("P to Play a Type", ExitColor); 

            Text.Write("\nOption: ", OptionsCaptionColor); 
        }
    }
}