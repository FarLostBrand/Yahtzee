using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yahtzee
{
    /// <summary>
    /// This is to create Dice. Each Die has it's own value and held status.
    /// </summary>
    public class Dice
    {
        #region Fields
        private int _number; //dice value
        private bool _isHeld; //if it's being held or not
        #endregion

        #region Properties

        /// <summary>
        /// This is read only. It gets the die's value.
        /// </summary>
        public int Number
        {
            get {return _number;}
        }

        /// <summary>
        /// This is read and write only. It gets if the die is being held or not. You can change whether it is or not.
        /// </summary>
        public bool IsHeld
        {
            get {return _isHeld;}
            set {_isHeld = value;}
        }
        #endregion

        #region Constructors
        public Dice()
        {
            _number = Roll();
            IsHeld = false;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This generates a new number for the die.
        /// </summary>
        public int Roll()
        {
            const int Min = 1;
            const int Max = 7;

            Random random = new Random();
            return random.Next(Min,Max);
        }
        #endregion

        #region Overriden Methods
        /// <summary>
        /// Prints the data of a die.
        /// </summary>
        public override string ToString()
        {
            return $"Dice Value: {Number}\nIs on Hold: {IsHeld}";
        }
        #endregion
    }
}