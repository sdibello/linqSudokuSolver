using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolverEngine.Objects;

namespace SudokuSolverEngine.Solver
{
    public class IterationFactory
    {
        public enum iterationType
        {
            standardRowColBlk = 0,
            PuzzleBlocks = 1,
            ColumnsAndRowsInBlock = 2,
            standardRowCol = 3
        }

        public enum currentIteration
        {
            Row =1,
            Column = 2,
            Block = 3
        }
        const int MaxIndex = 9;


        public iterationType  iteration { get; set; }
        public Guid uniqueId { get; set; }
        public int rowIndex { get; set; }
        public int colIndex { get; set; }
        public int BlockIndex { get; set; }
        public currentIteration Currently { get; set; }

        public void InterationFactory (iterationType iterator) {
            this.iteration = iterator;
            this.uniqueId = Guid.NewGuid();

            rowIndex = 0;
            colIndex = 0;
            BlockIndex = 0;
            this.Currently = currentIteration.Row;

        }

        public List<item> getNext(List<item> puzzle)
        {
            if (this.iteration == iterationType.standardRowColBlk) {
                return getNextStandardRowColBlock(puzzle);
            }
            else if (this.iteration == iterationType.PuzzleBlocks)
            {
                return getNextBlocks(puzzle);
            }
            else if (this.iteration == iterationType.ColumnsAndRowsInBlock)
            {
                return getNextRowColInBlocks(puzzle);
            }
            else if (this.iteration == iterationType.standardRowCol)
            {
                return getNextStandardRowCol(puzzle);
            }

            return null;
        }

        public List<item> getNextBlocks(List<item> puzzle)
        {

            if (BlockIndex == 0)
            {     // this is just starting. 
                BlockIndex = 1;
                this.Currently = currentIteration.Block;
            }
            else            
                BlockIndex++;

            if (BlockIndex <= 9)
            {
                this.Currently = currentIteration.Block;
                return puzzle.Where(x => x.Block == BlockIndex).ToList();
            }
            else
                return null;
        }


        public List<item> getNextStandardRowCol(List<item> puzzle)
        {

                if (rowIndex == 0)
                {     // this is just starting. 
                    rowIndex = 1;
                    colIndex = 1;
                    this.Currently = currentIteration.Row;
                }
                else
                {
                    if (rowIndex < 9)
                        rowIndex++;
                    else if (colIndex < 9)
                        colIndex++;
                    else
                        return null;
                }


                if ((rowIndex < 10) && (colIndex == 1))
                {
                    this.Currently = currentIteration.Row;
                    return puzzle.Where(x => x.Row == rowIndex).ToList();
                }
                else if (colIndex < 10)
                {
                    this.Currently = currentIteration.Column;
                    return puzzle.Where(x => x.Column == colIndex).ToList();
                }
                else
                    return null;
        }


        public List<item> getNextStandardRowColBlock(List<item> puzzle)
        {

                if (rowIndex == 0)
                {     // this is just starting. 
                    rowIndex = 1;
                    colIndex = 1;
                    BlockIndex = 1;
                    this.Currently = currentIteration.Row;
                }
                else
                {
                    if (rowIndex <= 9)
                        rowIndex++;
                    else if (colIndex <= 9)
                        colIndex++;
                    else if (BlockIndex <= 9)
                        BlockIndex++;
                    else
                        return null;
                }


                if ((rowIndex < 10) && (colIndex == 1))
                {
                    this.Currently = currentIteration.Row;
                    return puzzle.Where(x => x.Row == rowIndex).ToList();
                }
                else if ((colIndex < 10) && (BlockIndex == 1))
                {
                    this.Currently = currentIteration.Column;
                    return puzzle.Where(x => x.Column == colIndex).ToList();
                }
                else if (BlockIndex <= 9)
                {
                    this.Currently = currentIteration.Block;
                    return puzzle.Where(x => x.Block == BlockIndex).ToList();
                }
                else
                    return null;
        }


        public List<item> getNextRowColInBlocks(List<item> block)
        {
            //should only pass in one block
            if (block.Count > 9)
                throw new ApplicationException("More items passed into parameter then expected");

            /// blocks return non consecutive rows.  So need to logically skip the empty ones.
            /// easier by brute force cause there can be only NINE...
            Currently = currentIteration.Row;
            for (int i = this.rowIndex + 1; i < 10; i++)
            {
                this.rowIndex = i;
                List<item> rowItems = block.Where(x => x.Row == this.rowIndex).ToList();
                //if there are no items, skip
                if (rowItems.Count == 0)
                    continue;
                else {
                    return rowItems;
                }
            }

            Currently = currentIteration.Column;
            for (int y = this.colIndex + 1; y < 10; y++)
            {
                this.colIndex = y;
                List<item> colItems = block.Where(x => x.Column == y).ToList();
                if (colItems.Count == 0)
                    continue;
                else {
                    return colItems;
                }
            }

            return null;
        }


    }
}
