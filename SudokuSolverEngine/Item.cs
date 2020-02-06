using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SudokuSolverEngine.Objects
{

    public struct itemCount
    {
        public int value { get; set; }
        public int count { get; set; }
    }

    public struct MatrixItem
    {
        public List<int> ControlPossibles;  
        public List<item> items;
        public List<int> UnquePossibles;
    }

    public class item
    {
        private int? _value;

        public int? Value {
            get{ return _value; }
            set{ 
                this._value = value;
                this.possibles = null;
            } }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Block { get; set; }
        public int unique { get; set; }
        public List<int> possibles { get; set; }
        public bool dirty { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">The Row value for this item</param>
        /// <param name="column">The Column value for this item</param>
        /// <param name="block">The Block value for this item</param>
        public item (int row, int column, int block )
        {
            this.Row = row;
            this.Column = column;
            this.Block = block;
            this.Value = null;
            this.unique = int.Parse(string.Format("{0}{1}", row, column));
            this.possibles = new List<int>();
            this.dirty = false;

            for (int i = 1; i < 11; i++)
			{
			 possibles.Add(i);
			}
        }

        public item(int row, int column, int block, int? value)
        {
            if (value != null)
                if ((value > 9) || (value < 0))
                    throw new ApplicationException("invalid value of item");

            this.Row = row;
            this.Column = column;
            this.Block = block;
            this.Value = value;
            this.unique = int.Parse(string.Format("{0}{1}", row, column));
            this.dirty = false;
            if (value == null)
            {
                this.dirty = true;
                this.possibles = new List<int>();
                for (int i = 1; i <= 9; i++)
                    possibles.Add(i);
            }
        }

        public string getPossiblesString() {
            StringBuilder sb = new StringBuilder();

            foreach (var item in this.possibles) {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
    }

    public static class MyExtensions {
        /// <summary>
        /// Removes the set values from Rows, Columns and Blocks
        /// </summary>
        /// <param name="square">item being extended</param>
        /// <param name="puzzle">complete list of items</param>
        public static void CleanUp(this item square, List<item> puzzle)
        {
            int row = square.Row;
            int col = square.Column;
            int block = square.Block;

            if (square.Value == null)
                throw new ApplicationException("CleanUp: Error with cleanup value;");

            int val = (int)square.Value;

            Trace.WriteLine(string.Format("CleanUp - Set Value {0} in ({1}/{2}/{3})", square.Value.ToString(), row, col, block));

            List<item> rows = puzzle.Where(x => x.Row == row).Where(a => a.Value == null).ToList();
            List<item> cols = puzzle.Where(y => y.Column == col).Where(b => b.Value == null).ToList();
            List<item> blocks = puzzle.Where(z => z.Block == block).Where(c => c.Value == null).ToList();

            rows.RemovePossibilities(val);
            cols.RemovePossibilities(val);
            blocks.RemovePossibilities(val);
        }

        /// <summary>
        /// Removes the set values from Rows, Columns and Blocks
        /// Allow to cleanup over a list
        /// </summary>
        /// <param name="list">List of items to be cleaned up</param>
        /// <param name="puzzle">Complete list of items</param>
        public static void CleanUp(this List<item> list, List<item> puzzle)
        {
            foreach (var item in list)
            {
                item.CleanUp(puzzle);
            }
        }


        /// <summary>
        /// extends a list of item, and allows you to remove a possible for the whole list
        /// </summary>
        /// <param name="squares">the extended item</param>
        /// <param name="removeMe">the integer to be removed</param>
        public static void RemovePossibilities(this List<item> squares, int removeMe)
        {
            foreach (var item in squares)
            {
                //Ignore the already solved in remove possibilities
                if (item.possibles != null)
                { 
                    if (item.possibles.Contains(removeMe))
                        Trace.WriteLine(string.Format("Removing {0} from ({1}/{2}/{3})", removeMe, item.Row.ToString(), item.Column.ToString(), item.Block.ToString()));
                        item.possibles.Remove(removeMe);
                }
            }

        }

        public static void RemovePossibilities(this List<item> squares, int removeMe, out int count)
        {
            count = 0;
            foreach (var item in squares)
            {
                //Ignore the already solved in remove possibilities
                if (item.possibles != null)
                {
                    if (item.possibles.Contains(removeMe))
                    {
                        Trace.WriteLine(string.Format("Removing {0} from ({1}/{2}/{3})", removeMe, item.Row.ToString(), item.Column.ToString(), item.Block.ToString()));
                        item.possibles.Remove(removeMe);
                        count++;
                    }
                }
            }

        }

        public static void SetValue(this item square, int value, List<item> puzzle)
        {
            square.Value = value;
            square.dirty = true;
            square.possibles = null;
            square.CleanUp(puzzle);
        }

        public static void SetValuePossible(this List<item> squares, List<item> puzzle)
        {
            foreach (var item in squares)
            {
                if (item.possibles.Count > 1)
                    throw new ApplicationException("Error in SetValuePossible, too many possible");

                int leftVal = item.possibles.First();
                item.Value = leftVal;
                item.dirty = true;
                item.possibles = null;
                item.CleanUp(puzzle);
            }
        }

        public static List<item> GetRow(this List<item> squares, int index)
        {
            return squares.Where(x => x.Row == index).ToList();
        }

        public static List<item> GetCol(this List<item> squares, int index)
        {
            return squares.Where(x => x.Column == index).ToList();
        }

        public static List<item> GetBlock(this List<item> squares, int index)
        {
            return squares.Where(x => x.Block == index).ToList();
        }

        public static List<int> getPossibleUnique(this List<item> squares)
        {
            List<int> returnMe = new List<int>();
            foreach (var item in squares)
            {
                if (item == null)
                    continue;

                if (item.possibles == null)
                    continue;

                foreach (var loop in item.possibles)
                {
                    if (!returnMe.Contains(loop))
                        returnMe.Add(loop);
                }
            }
            return returnMe;
        }

        public static List<int> getUnqiues(this List<MatrixItem> poss)
        {
            List<int> returnMe = new List<int>();
            foreach (var item in poss) {
                foreach (var square in item.items) {
                    returnMe.Add(square.unique);
	            }
            }
            return returnMe;
        }

        public static string getPossibleUniqueString(this List<item> squares)
        {
            StringBuilder digits = new StringBuilder();
            foreach (var item in squares)
            {
                if (item == null)
                    continue;

                if (item.possibles == null)
                    continue;

                foreach (var loop in item.possibles)
                {
                    string currStr = digits.ToString();
                    string currInt = loop.ToString();
                    if (currStr.IndexOfAny(currInt.ToCharArray()) == -1){
                        digits.Append(loop);
                    }


                }
            }
            return digits.ToString();
        }

        public static List<MatrixItem> generatePossiblityMatrix(this List<item> squares, item controlPossible)
        {
            List<MatrixItem> returnMe = new List<MatrixItem>();
            //Trace.WriteLine(string.Format("Generating possibility matrix for {0} items", squares.Count.ToString()));

            if (controlPossible.possibles.Count < 3)
                return null;

                //generate the items for the list
            for (int outer = 0; outer < controlPossible.possibles.Count - 1; outer++)
                {
                    for (int inner = outer + 1; inner < controlPossible.possibles.Count; inner++)
                    {
                        //Trace.WriteLine(string.Format("Creating Matrix items {0}-{1}", outer.ToString(), inner.ToString()));
                        MatrixItem newGuy = new MatrixItem();
                        newGuy.ControlPossibles = new List<int>();
                        newGuy.ControlPossibles.Add(controlPossible.possibles[outer]);
                        newGuy.ControlPossibles.Add(controlPossible.possibles[inner]);
                        newGuy.items = squares.Where(x => x.possibles.Contains(controlPossible.possibles[outer])).Where(y => y.possibles.Contains(controlPossible.possibles[inner])).Where(z => z.unique != controlPossible.unique).ToList();
                        newGuy.UnquePossibles = newGuy.items.getPossibleUnique();
                        returnMe.Add(newGuy);
                    }
                }
            return returnMe;
        }

        public static List<MatrixItem> generatePossiblityMatrix(this List<item> squares)
        {
            List<int> UniquePossibles = squares.getPossibleUnique();
            List<MatrixItem> returnMe = new List<MatrixItem>();
            //Trace.WriteLine(string.Format("Generating possibility matrix for {0} items", squares.Count.ToString()));

            if (squares.Count == 0)
            {
                return null;
            }

            if (squares.Count == 2)
            {
                MatrixItem newGuy = new MatrixItem();
                newGuy.ControlPossibles = new List<int>();
                newGuy.ControlPossibles.Add(UniquePossibles[0]);
                newGuy.ControlPossibles.Add(UniquePossibles[1]);
                newGuy.items = squares.Where(x => x.possibles.Contains(UniquePossibles[0])).Where(y => y.possibles.Contains(UniquePossibles[1])).ToList();
                newGuy.UnquePossibles = newGuy.items.getPossibleUnique();
                returnMe.Add(newGuy);
            } else {
                //generate the items for the list
                for (int outer = 0; outer < UniquePossibles.Count - 1; outer++)
                {
                    for (int inner = outer + 1; inner < UniquePossibles.Count; inner++)
                    {
                        //Trace.WriteLine(string.Format("Creating Matrix items {0}-{1}", outer.ToString(), inner.ToString()));
                        MatrixItem newGuy = new MatrixItem();
                        newGuy.ControlPossibles = new List<int>();
                        newGuy.ControlPossibles.Add(UniquePossibles[outer]);
                        newGuy.ControlPossibles.Add(UniquePossibles[inner]);
                        newGuy.items = squares.Where(x => x.possibles.Contains(UniquePossibles[outer])).Where(y => y.possibles.Contains(UniquePossibles[inner])).ToList();
                        newGuy.UnquePossibles = newGuy.items.getPossibleUnique();
                        returnMe.Add(newGuy);
                    }
                }
            }

            return returnMe;
        
        }

        /// <summary>
        /// returns a strong representation of the possibles for the group of items
        /// </summary>
        /// <param name="squares"></param>
        /// <returns></returns>
        public static string getPossiblesString(this List<item> squares)
        {
            StringBuilder digits = new StringBuilder();
            foreach (var item in squares)
            {
                if (item == null)
                    continue;

                if (item.possibles == null)
                    continue;

                foreach (var loop in item.possibles)
                    digits.Append(loop);
            }
            return digits.ToString();
        }

        public static List<MatrixItem> generateSimpleMatrix(this List<item> squares)
        {
            List<MatrixItem> returnMe = new List<MatrixItem>();
            //Trace.WriteLine(string.Format("Generating possibility matrix for {0} items", squares.Count.ToString()));

            if (squares.Count == 0) {
                return null;
            }

            for (int i = 1; i < 10; i++) {
                MatrixItem newGuy = new MatrixItem();
                newGuy.ControlPossibles = new List<int>();
                newGuy.ControlPossibles.Add(i);
                newGuy.items = squares.Where(x => x.possibles.Contains(i)).ToList();
                newGuy.UnquePossibles = newGuy.items.getPossibleUnique();
                returnMe.Add(newGuy);
            }

            return returnMe;
        }

    }

}
