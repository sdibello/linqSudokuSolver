using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace linqSudokuSolverEngine.sudoku
{
    public class sudokuItem
    {
        public (byte row, byte col, byte box) Key { get; set; }
        public byte Value { get; set; }
        public List<byte> Possibles { get; set; }

        public sudokuItem(byte row, byte col, byte box)
        {
            Key = (row, col, box);
            Value = 0;
            Possibles = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        }

    }
}
