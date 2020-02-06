using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolverEngine.Objects;

namespace SudokuSolverEngine.Solver
{
    public static class Loader
    {
        /// <summary>
        /// Load a test puzzle
        /// </summary>
        /// <returns></returns>
        public static List<item> Load()
        {
            List<item> puzzle = new List<item> { 
                new item(1, 1, 1, 6),
                new item(1, 2, 1, null),
                new item(1, 3, 1, null),
                new item(1, 4, 2, null),
                new item(1, 5, 2, null),
                new item(1, 6, 2, 8),
                new item(1, 7, 3, 7),
                new item(1, 8, 3, null),
                new item(1, 9, 3, null),

                new item(2, 1, 1, 8),
                new item(2, 2, 1, null),
                new item(2, 3, 1, 4),
                new item(2, 4, 2, 3),
                new item(2, 5, 2, null),
                new item(2, 6, 2, null),
                new item(2, 7, 3, null),
                new item(2, 8, 3, 9),
                new item(2, 9, 3, null),

                new item(3, 1, 1, null),
                new item(3, 2, 1, null),
                new item(3, 3, 1, null),
                new item(3, 4, 2, null),
                new item(3, 5, 2, 4),
                new item(3, 6, 2, null),
                new item(3, 7, 3, null),
                new item(3, 8, 3, 3),
                new item(3, 9, 3, null),

                new item(4, 1, 4, null),
                new item(4, 2, 4, null),
                new item(4, 3, 4, 9),
                new item(4, 4, 5, null),
                new item(4, 5, 5, null),
                new item(4, 6, 5, null),
                new item(4, 7, 6, null),
                new item(4, 8, 6, 1),
                new item(4, 9, 6, null),

                new item(5, 1, 4, null),
                new item(5, 2, 4, null),
                new item(5, 3, 4, null),
                new item(5, 4, 5, 1),
                new item(5, 5, 5, null),
                new item(5, 6, 5, 3),
                new item(5, 7, 6, null),
                new item(5, 8, 6, null),
                new item(5, 9, 6, null),

                new item(6, 1, 4, null),
                new item(6, 2, 4, null),
                new item(6, 3, 4, null),
                new item(6, 4, 5, null),
                new item(6, 5, 5, 7),
                new item(6, 6, 5, null),
                new item(6, 7, 6, null),
                new item(6, 8, 6, null),
                new item(6, 9, 6, 6),

                new item(7, 1, 7, null),
                new item(7, 2, 7, null),
                new item(7, 3, 7, null),
                new item(7, 4, 8, 5),
                new item(7, 5, 8, null),
                new item(7, 6, 8, null),
                new item(7, 7, 9, null),
                new item(7, 8, 9, null),
                new item(7, 9, 9, null),

                new item(8, 1, 7, 4),
                new item(8, 2, 7, null),
                new item(8, 3, 7, 8),
                new item(8, 4, 8, 7),
                new item(8, 5, 8, null),
                new item(8, 6, 8, null),
                new item(8, 7, 9, 9),
                new item(8, 8, 9, null),
                new item(8, 9, 9, 5),

                new item(9, 1, 7, 3),
                new item(9, 2, 7, null),
                new item(9, 3, 7, null),
                new item(9, 4, 8, 9),
                new item(9, 5, 8, 8),
                new item(9, 6, 8, 1),
                new item(9, 7, 9, null),
                new item(9, 8, 9, 6),
                new item(9, 9, 9, null)
                };
 
                return puzzle;

            }

    }
}
