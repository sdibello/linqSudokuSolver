// See https://aka.ms/new-console-template for more information

using linqSudokuSolverEngine.sudoku;

Console.WriteLine("Hello, World!");

var game = new sudokuGame("020900000048000031000063020009407003003080200400105600030570000250000180000006050");

Console.WriteLine(game.SudokuItems.Count());


Console.ReadLine();
