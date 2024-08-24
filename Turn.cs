using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yahtzee
{
    /// <summary>
    /// Each turn runs this class. It updates the 5 dice, and does turn operations.
    /// </summary>
    public class Turn
    {
        #region Fields
        //fields that belong to the class
        private static Dice[] _diceArray;
        private static int[] _diceValue;
        private static int _totalValueOfDice;

        //fields that belong to the object
        private int _totalRolls;
        #endregion

        #region Properties
        /// <summary>
        /// This is to change or get the dice inside the dice array.
        /// </summary>
        public static Dice[] DiceArray
        {
            get {return _diceArray;}
            set {_diceArray = value;}
        }

        /// <summary>
        /// This is to change or get the dice value. It's store in increasing order.
        /// </summary>
        public static int[] DiceValue
        {
            get {return _diceValue;}
            set {_diceValue = value;}
        }

        /// <summary>
        /// This is to get the sum of all dice values.
        /// </summary>
        public static int TotalValueOfDice
        {
            get {return _totalValueOfDice;}
        }

        /// <summary>
        /// This is to change or get the remaining rolls left for this turn.
        /// </summary>
        public int TotalRolls
        {
            get {return _totalRolls;}
            set 
            {
                if (value < 0)
                    throw new Exception("Remaining Rolls can't be lower than 0.");

                _totalRolls = value;
            }
        }
        #endregion

        #region Constructors
        public Turn()
        {
            TotalRolls = 3;
        }

        static Turn()
        {
            DiceArray = [new Dice(), new Dice(), new Dice(), new Dice(), new Dice()];
            GetDiceValue();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total of ones.
        /// </summary>
        public int Ones()
        {
            return Counter((int)Combination.Ones + 1);
        }

        /// <summary>
        /// Returns the total of twos multiplied by 2.
        /// </summary>
        public int Twos()
        {
            const int Multiplier = 2;
            return Counter((int)Combination.Twos + 1) * Multiplier;
        }

        /// <summary>
        /// Returns the total of threes multiplied by 3.
        /// </summary>
        public int Threes()
        {
            const int Multiplier = 3;
            return Counter((int)Combination.Threes + 1) * Multiplier;
        }

        /// <summary>
        /// Returns the total of fours multiplied by 4.
        /// </summary>
        public int Fours()
        {
            const int Multiplier = 4;
            return Counter((int)Combination.Fours + 1) * Multiplier;
        }

        /// <summary>
        /// Returns the total of fives multiplied by 5.
        /// </summary>
        public int Fives()
        {
            const int Multiplier = 5;
            return Counter((int)Combination.Fives + 1) * Multiplier;
        }

        /// <summary>
        /// Returns the total of sixes multiplied by 6.
        /// </summary>
        public int Sixes()
        {
            const int Multiplier = 6;
            return Counter((int)Combination.Sixes + 1) * Multiplier;
        }

        /// <summary>
        /// Returns the total sum of all numbers if 3 are of the same kind.
        /// </summary>
        public int ThreeOfKind()
        {
            const int NumberOfSameKind = 3; //number of times the same number is required
            return XOfKind(NumberOfSameKind);
        }

        /// <summary>
        /// Returns the total sum of all numbers if 4 are of the same kind.
        /// </summary>
        public int FourOfKind()
        {
            const int NumberOfSameKind = 4; //number of times the same number is required
            return XOfKind(NumberOfSameKind);
        }

        /// <summary>
        /// Returns a score of 30 if there is a small straight (1-2-3-4 | 2-3-4-5 | 3-4-5-6).
        /// </summary>
        public int SmallStraight()
        {
            const int SmallStraightPoints = 30;
            const int RequiredSequenceLength = 4;
            int sequenceLength = 1;

            for (int i = 1; i < DiceValue.Length; i++)
            {
                if(DiceValue[i] != DiceValue[i-1])
                {
                    if (DiceValue[i - 1] == DiceValue[i] - 1) //if the current value - 1 is not the same as the previous one...
                        sequenceLength++; //add to sequence length
                    else if (sequenceLength < RequiredSequenceLength) //if it's not and the required sequence length is not reached, then reset to 0.
                        sequenceLength = 1;
                }
            }

            if (sequenceLength >= RequiredSequenceLength)
                return SmallStraightPoints;

            return 0;
        }

        /// <summary>
        /// Returns a score of 40 if there is a large straight (1-2-3-4-5 | 2-3-4-5-6).
        /// </summary>
        public int LargeStraight()
        {
            const int LargeStraightPoints = 40;
            int sequenceLength = 1;

            for (int i = 1; i < DiceValue.Length; i++)
            {
                if (DiceValue[i - 1] == DiceValue[i] - 1) //if the current value - 1 is not the same as the previous one...
                    sequenceLength++; //add to sequence length
                else //all dice must be apart of the sequence so...
                    return 0;
            }

            return LargeStraightPoints; //if it makes it here it's good
        }

        /// <summary>
        /// Returns a score of 25 if there is 3 of x number and 2 of y number.
        /// </summary>
        public int FullHouse()
        {
            const int FullHousePoints = 25;
            
            int maxNumberOfX = 3;
            int numberOfX = 1;
            int secondNumber = 0;

            for (int i = 1; i < DiceValue.Length; i++)
            {
                if(numberOfX < maxNumberOfX)
                {
                    if (DiceValue[i] == DiceValue[i - 1]) //if the current value is the same as the previous one...
                        numberOfX++; //add to numberOfX
                    else if (numberOfX == maxNumberOfX - 1)
                    {
                        maxNumberOfX--; //make it 2 because we can have 2 of x number and 3 of y number
                        secondNumber = DiceValue[i]; //make the number that made the last check fail the required 2nd number. There can only be 2.
                    }
                    else
                        return 0; //if it failed the first check the first time then there will 100% be 3 different numbers.
                }
                else 
                {
                    if(secondNumber == 0)
                        secondNumber = DiceValue[i];
                    else if (DiceValue[i] != secondNumber) //we only care that the last 2 or 3 numbers are the same, so if not then it doesn't matter.
                        return 0;
                }
            }

            return FullHousePoints;
        }

        /// <summary>
        /// Returns a score of 50 if there is 5 of one number. Returns 100 if this has already been done.
        /// </summary>
        /// <param name="previousYahtzee">
        /// Was there another yahtzee before? Yes or No.
        /// </param>
        public int Yahtzee(bool previousYahtzee)
        {
            const int YahtzeeScore = 50;
            const int BonusYahtzeeScore = 100;
            const int RequiredNumberOfX = 5;

            if(Counter(DiceValue[0]) != RequiredNumberOfX) //there can only be 1 type of number so we can use any index.
                return 0;
            else if (previousYahtzee) //if theres already been a yahtzee than...
                return BonusYahtzeeScore;

            return YahtzeeScore;
        }

        /// <summary>
        /// Returns the total of x number.
        /// </summary>
        /// <param name="combination">x number we are looking for</param>
        /// <returns></returns>
        private int Counter(int number)
        {
            int totalOfX = 0;
            foreach (int value in DiceValue)
            {
                if (value == number)
                    totalOfX++;
            }
            return totalOfX;
        }

        /// <summary>
        /// Counts how many of the same number you have and checks if you have more than x amount.
        /// </summary>
        /// <param name="numberOfSameKind">X amount of number you need</param>
        /// <returns>Sum of all dice or 0</returns>
        private int XOfKind(int numberOfSameKind)
        {
            int ofKind = 1;

            for(int i = 1; i < DiceValue.Length; i++)
            {
                if (DiceValue[i] == DiceValue[i-1]) //if the current value is the same as the previous one then...
                    ofKind++; //add to ofKind
                else if (ofKind < numberOfSameKind) //if it's not and the required ofKind number is not reached, then reset to 0.
                    ofKind = 1;
            }

            if (ofKind >= numberOfSameKind) //if there is enough of the same number
                return TotalValueOfDice; //returns the sum

            return 0;
        }

        /// <summary>
        /// Rolls the dice. Generates new dice and replaces the unheld ones.
        /// </summary>
        public void RollDice()
        {            
            if (TotalRolls <= 0)
                throw new Exception("You cannot reroll when 0 or less rolls remain.");

            for(int i = 0; i < DiceArray.Length; i++)
            {
                if(DiceArray[i].IsHeld ==false) //if the die is not held then
                {
                    DiceArray[i] = new Dice();
                } 
            }

            GetDiceValue(); //updates the DiceValue array for the checks.

            TotalRolls--;
        }

        /// <summary>
        /// Sets the values of the dice in an array and sorts it.
        /// </summary>
        public static void GetDiceValue()
        {
            DiceValue = [DiceArray[0].Number, DiceArray[1].Number, DiceArray[2].Number, DiceArray[3].Number, DiceArray[4].Number];
            Array.Sort(DiceValue);
            GetTotalValueOfDice(); //updates the total
        }

        /// <summary>
        /// Returns the sum of the value of all dice. Since there's one dice array, this belongs to the class.
        /// This is also used for the Chance check.
        /// </summary>
        public static void GetTotalValueOfDice() //this will also be used for chance
        {
            int total = 0;

            foreach (int value in DiceValue)
            {
                total += value;
            }

            _totalValueOfDice = total;
        }
        #endregion
    }
}