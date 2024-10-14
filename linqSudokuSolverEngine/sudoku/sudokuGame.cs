using System.Globalization;

namespace linqSudokuSolverEngine.sudoku
{
    public class sudokuGame
    {
        public List<sudokuItem> SudokuItems { get; set; }

        public sudokuGame(string loadgame)
        {
            if (loadgame == null) {
                ArgumentNullException.ThrowIfNull("Can't have a null game to load");
                return;
            }

            var numbers = loadgame.ToCharArray();

            // this is very code heavy - find a better way.

            this.SudokuItems = new List<sudokuItem>();
            this.SudokuItems.Add(new sudokuItem(1, 1, 1) { Value = byte.Parse(numbers[0].ToString()) });


            this.SudokuItems.Add(new sudokuItem(1, 2, 1) { Value = byte.Parse(numbers[1].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 3, 1) { Value = byte.Parse(numbers[2].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 4, 2) { Value = byte.Parse(numbers[3].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 5, 2) { Value = byte.Parse(numbers[4].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 6, 2) { Value = byte.Parse(numbers[5].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 7, 3) { Value = byte.Parse(numbers[6].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 8, 3) { Value = byte.Parse(numbers[7].ToString()) });
            this.SudokuItems.Add(new sudokuItem(1, 9, 3) { Value = byte.Parse(numbers[8].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 1, 1) { Value = byte.Parse(numbers[9].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 2, 1) { Value = byte.Parse(numbers[10].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 3, 1) { Value = byte.Parse(numbers[11].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 4, 2) { Value = byte.Parse(numbers[12].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 5, 2) { Value = byte.Parse(numbers[13].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 6, 2) { Value = byte.Parse(numbers[14].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 7, 3) { Value = byte.Parse(numbers[15].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 8, 3) { Value = byte.Parse(numbers[16].ToString()) });
            this.SudokuItems.Add(new sudokuItem(2, 9, 3) { Value = byte.Parse(numbers[17].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 1, 1) { Value = byte.Parse(numbers[18].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 2, 1) { Value = byte.Parse(numbers[19].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 3, 1) { Value = byte.Parse(numbers[20].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 4, 2) { Value = byte.Parse(numbers[21].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 5, 2) { Value = byte.Parse(numbers[22].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 6, 2) { Value = byte.Parse(numbers[23].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 7, 3) { Value = byte.Parse(numbers[24].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 8, 3) { Value = byte.Parse(numbers[25].ToString()) });
            this.SudokuItems.Add(new sudokuItem(3, 9, 3) { Value = byte.Parse(numbers[26].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 1, 4) { Value = byte.Parse(numbers[27].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 2, 4) { Value = byte.Parse(numbers[28].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 3, 4) { Value = byte.Parse(numbers[29].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 4, 5) { Value = byte.Parse(numbers[30].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 5, 5) { Value = byte.Parse(numbers[31].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 6, 5) { Value = byte.Parse(numbers[32].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 7, 6) { Value = byte.Parse(numbers[33].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 8, 6) { Value = byte.Parse(numbers[34].ToString()) });
            this.SudokuItems.Add(new sudokuItem(4, 9, 6) { Value = byte.Parse(numbers[35].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 1, 4) { Value = byte.Parse(numbers[36].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 2, 4) { Value = byte.Parse(numbers[37].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 3, 4) { Value = byte.Parse(numbers[38].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 4, 5) { Value = byte.Parse(numbers[39].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 5, 5) { Value = byte.Parse(numbers[40].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 6, 5) { Value = byte.Parse(numbers[41].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 7, 6) { Value = byte.Parse(numbers[42].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 8, 6) { Value = byte.Parse(numbers[43].ToString()) });
            this.SudokuItems.Add(new sudokuItem(5, 9, 6) { Value = byte.Parse(numbers[44].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 1, 4) { Value = byte.Parse(numbers[45].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 2, 4) { Value = byte.Parse(numbers[46].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 3, 4) { Value = byte.Parse(numbers[47].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 4, 5) { Value = byte.Parse(numbers[48].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 5, 5) { Value = byte.Parse(numbers[49].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 6, 5) { Value = byte.Parse(numbers[50].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 7, 6) { Value = byte.Parse(numbers[51].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 8, 6) { Value = byte.Parse(numbers[52].ToString()) });
            this.SudokuItems.Add(new sudokuItem(6, 9, 6) { Value = byte.Parse(numbers[53].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 1, 7) { Value = byte.Parse(numbers[54].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 2, 7) { Value = byte.Parse(numbers[55].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 3, 7) { Value = byte.Parse(numbers[56].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 4, 8) { Value = byte.Parse(numbers[57].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 5, 8) { Value = byte.Parse(numbers[58].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 6, 8) { Value = byte.Parse(numbers[59].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 7, 9) { Value = byte.Parse(numbers[60].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 8, 9) { Value = byte.Parse(numbers[61].ToString()) });
            this.SudokuItems.Add(new sudokuItem(7, 9, 9) { Value = byte.Parse(numbers[62].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 1, 7) { Value = byte.Parse(numbers[63].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 2, 7) { Value = byte.Parse(numbers[64].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 3, 7) { Value = byte.Parse(numbers[65].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 4, 8) { Value = byte.Parse(numbers[66].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 5, 8) { Value = byte.Parse(numbers[67].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 6, 8) { Value = byte.Parse(numbers[68].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 7, 9) { Value = byte.Parse(numbers[69].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 8, 9) { Value = byte.Parse(numbers[70].ToString()) });
            this.SudokuItems.Add(new sudokuItem(8, 9, 9) { Value = byte.Parse(numbers[71].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 1, 7) { Value = byte.Parse(numbers[72].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 2, 7) { Value = byte.Parse(numbers[73].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 3, 7) { Value = byte.Parse(numbers[74].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 4, 8) { Value = byte.Parse(numbers[75].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 5, 8) { Value = byte.Parse(numbers[76].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 6, 8) { Value = byte.Parse(numbers[77].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 7, 9) { Value = byte.Parse(numbers[78].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 8, 9) { Value = byte.Parse(numbers[79].ToString()) });
            this.SudokuItems.Add(new sudokuItem(9, 9, 9) { Value = byte.Parse(numbers[80].ToString()) });

            return;
        }

        public solve()
        {



        }

    }
}