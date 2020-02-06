using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolverEngine;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Go?");
            Console.ReadLine();
            SudokuSolverEngine.Solver.Solver SuDoKu = new SudokuSolverEngine.Solver.Solver();
            SuDoKu.Solve();
            Console.ReadLine();
        }
    }
}
