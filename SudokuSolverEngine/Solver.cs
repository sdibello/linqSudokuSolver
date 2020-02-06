using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SudokuSolverEngine.Objects;
using SudokuSolverEngine.Solver;

namespace SudokuSolverEngine.Solver
{
    public enum processingLevel
    {
        simple = 0,
        intermeddiate = 1,
        hard = 2
    }

    public class Solver
    {
        private bool progressMade = false;
        List<item> puzzle = null;
        processingLevel level = processingLevel.simple;

        public void Solve()
        {

            puzzle = Loader.Load();

            if (puzzle == null)
                throw new ApplicationException("Error Loading Puzzle");

            Trace.WriteLine("Starting to process puzzle");
            //writeEntirePuzzle(puzzle);
            writeEntirePuzzelPossibles(puzzle);

            //take all items that are solved and clean up around them.
            List<item> changedItems = puzzle.Where(x => x.Value != null).ToList();
            foreach (var item in changedItems) {
                item.possibles = null;
            }

            changedItems.CleanUp(puzzle);
            writeEntirePuzzelPossibles(puzzle);
            while (puzzle.Where(y => y.Value == null).Count() != 0)
            {
                this.progressMade = false;
                iterativeSolverLevelOne(ref puzzle);
                if (this.progressMade == false)
                {
                    iterativeSolverSingleAreaValues(ref puzzle);
                }

                if (this.progressMade == false)
                {
                    iterativeSolverCandidateLines(ref puzzle);
                }

                if (this.progressMade == false)
                {
                    iterativeSolverNakedPairs(ref puzzle);
                }

                if (this.progressMade == false)
                {
                    iterativeSolverTriples(ref puzzle);
                }


                if (progressMade == false) {
                    Trace.WriteLine("Quitting, nothing solved, but no recourse");
                    break;
                }
            }
        
        }

    /// <summary>
    /// level 1... simple look to see if there are cells with single possible answers
    /// </summary>
    /// <param name="puzzle"></param>
    private void iterativeSolverLevelOne(ref List<item> puzzle)
    {
        //level one, get the possibilities
        List<item> singlePoss = puzzle.Where(z => z.possibles != null).Where(x => x.possibles.Count() == 1).ToList();
        if (singlePoss.Count() == 0) {
            progressMade = false;
            return;
        }

        progressMade = true;
        Trace.WriteLine(string.Format(">> Level One - Found ({0}) single possible", singlePoss.Count()));
        singlePoss.SetValuePossible(puzzle);

        //writeEntirePuzzle(puzzle);
        writeEntirePuzzelPossibles(puzzle);
    }

        /// <summary>
        /// see if there is an occurrence of a single value in a block, row, or column this is unique
        /// in other words, if there is only one two in a column, 
        /// </summary>
        /// <param name="puzzle"></param>
    private void iterativeSolverSingleAreaValues(ref List<item> puzzle)
    {
        IterationFactory factory = new IterationFactory();
        factory.iteration = IterationFactory.iterationType.standardRowColBlk;
        List<item> ItoratedList = factory.getNext(puzzle);
        while (ItoratedList != null)
        {
            List<itemCount> matchedValues = new List<itemCount>();
            string posString = ItoratedList.getPossiblesString();
            for (int i = 1; i < 10; i++)
            {
                string reg = string.Format("{0}", i);
                Regex regex = new Regex(reg);
                MatchCollection matches = regex.Matches(posString);
                matchedValues.Add(new itemCount() { value = i, count = matches.Count });
            }

            //are there any numbers that have only 1 possible in a row/col/blk.
            List<itemCount> hits = matchedValues.Where(x => x.count == 1).ToList();

            //if not, continue to the next one
            if (hits.Count > 0)
            {
                //we have a winner.
                Trace.WriteLine(string.Format(">> Level Two - Found ({0}) single possible by region", hits.Count()));
                foreach (var item in hits)
                {
                    item winner;
                    if (factory.Currently == IterationFactory.currentIteration.Row) {
                        this.progressMade = true;
                        winner = puzzle.Where(x => x.Row == ItoratedList[0].Row).Where(z => z.possibles != null).Where(y => y.possibles.Contains(item.value)).Single();
                    } else if (factory.Currently == IterationFactory.currentIteration.Column) {
                        this.progressMade = true;
                        winner = puzzle.Where(x => x.Column == ItoratedList[0].Column).Where(z => z.possibles != null).Where(y => y.possibles.Contains(item.value)).Single();
                    } else {
                        this.progressMade = true;
                        winner = puzzle.Where(x => x.Block == ItoratedList[0].Block).Where(z => z.possibles != null).Where(y => y.possibles.Contains(item.value)).Single();
                    }
                    winner.SetValue(item.value, puzzle);
                    //writeEntirePuzzle(puzzle);
                    writeEntirePuzzelPossibles(puzzle);
                }
            }


            ItoratedList = factory.getNext(puzzle);
        }
    }

    private void iterativeSolverCandidateLines(ref List<item> puzzle)
    {
        IterationFactory factory = new IterationFactory();
        factory.iteration = IterationFactory.iterationType.PuzzleBlocks;
        List<item> BlockOfItems = factory.getNext(puzzle);

        //Iterate over each of the 9 blocks
        while (BlockOfItems != null)
        {
            //iterate over the rows and columns.
            IterationFactory factoryInBlock = new IterationFactory();
            factoryInBlock.iteration = IterationFactory.iterationType.ColumnsAndRowsInBlock;
            List<item> RowOrColOfBlock = factoryInBlock.getNext(BlockOfItems);
            while (RowOrColOfBlock != null)
            {
                int activeBlock = RowOrColOfBlock[0].Block;
                //Trace.WriteLine(string.Format(">> Level 3 - Candidate Line Check ({0})", activeBlock));

                List<item> Unsolved = RowOrColOfBlock.Where(x => x.Value == null).ToList();
                //if there is only one, or less unknown, skip
                if (Unsolved.Count <= 1) {
                    RowOrColOfBlock = factoryInBlock.getNext(BlockOfItems);                
                    continue;
                }
                for (int i = 1; i < 10; i++) {
                    //see if the current row/col has i in it's possibles.
                    List<item> hasNumber = RowOrColOfBlock.Where(y => y.possibles != null).Where(x => x.possibles.Contains(i)).ToList();

                    if (hasNumber.Count == 0)
                        continue;

                    // have found items with a possible that matches i
                    if (hasNumber != null)
                    {
                        //does it exist elsewhere in the block (count column/row == count for entire block
                        // if the two counts are not the same, this doesn't exist elsewhere in the block.
                        List<item> totalBlockItems = BlockOfItems.Where(y => y.possibles != null).Where(x => x.possibles.Contains(i)).ToList();
                        if (hasNumber.Count != totalBlockItems.Count)
                            continue;
                        
                        if (factoryInBlock.Currently == IterationFactory.currentIteration.Column) {
                            int activeColumn = hasNumber[0].Column;

                            //search the rest of the column for that value.
                            List<item> column = puzzle.GetCol(activeColumn);
                            List<item> itemsInColumn = column.Where(y => y.possibles != null).Where(x => x.possibles.Contains(i)).ToList();
                            //if the total number of items with the i possible match the total number of
                            // items in the block row.  Then skip, there is nothing to get rid of.
                            if (itemsInColumn.Count == hasNumber.Count)
                                continue;
                            else { 
                                //if the counts are different (total items in row/col vs total items in block row/col) then remove the other possibles
                                List<item> toRemove = column.Where(x => x.Block != activeBlock).Where(y => y.possibles != null).Where(z => z.possibles.Contains(i)).ToList();
                                if (toRemove.Count > 0)
                                { 
                                    Trace.WriteLine(string.Format(">> Level 3 - Found candidate lines in column {0}", activeColumn));
                                    toRemove.RemovePossibilities(i);
                                    this.progressMade = true;
                                    //writeEntirePuzzle(puzzle);
                                    writeEntirePuzzelPossibles(puzzle);
                                }
                            }
                        } else {
                            int activeRow = hasNumber[0].Row;

                            //search the rest of the row for that value                        
                            List<item> row = puzzle.GetRow(activeRow);
                            List<item> itemsInRow = row.Where(y => y.possibles != null).Where(x => x.possibles.Contains(i)).ToList();
                            //if the item counts are the same, skip.. nothing to get ride of
                            if (itemsInRow.Count == hasNumber.Count)
                                continue;
                            else
                                {
                                    //the item counts are different, we need to remove some.
                                    List<item> toRemove = row.Where(x => x.Block != activeBlock).Where(y => y.possibles != null).Where(z => z.possibles.Contains(i)).ToList();
                                    if (toRemove.Count > 0) { 
                                        Trace.WriteLine(string.Format(">> Level 3 - Found candidate lines in row {0}", activeRow));
                                        toRemove.RemovePossibilities(i);
                                        this.progressMade = true;
                                        //writeEntirePuzzle(puzzle);
                                        writeEntirePuzzelPossibles(puzzle);
                                    }
                            } //else 
                        }  //else

                    } //if hasNumber != null

                }
                RowOrColOfBlock = factoryInBlock.getNext(BlockOfItems);

            }
            BlockOfItems = factory.getNext(puzzle);            
        }       //while
    }

    private void iterativeSolverNakedPairs(ref List<item> puzzle)
    {
        IterationFactory factory = new IterationFactory();
        factory.iteration = IterationFactory.iterationType.standardRowColBlk;
        List<item> CurrentItems = factory.getNext(puzzle).ToList();
        Trace.WriteLine("Starting Naked Pairs search");
        
        //loop through all the rows, then the columns
        while (CurrentItems != null) {
            List<item> onlyActive = CurrentItems.Where(x => x.possibles != null).ToList();
            //for debugging only
            //if (factory.Currently == IterationFactory.currentIteration.Row)
            //    Trace.WriteLine(string.Format("Level 4: Naked Pair.. processing row {0} ", factory.rowIndex));
            //else if (factory.Currently == IterationFactory.currentIteration.Column)
            //    Trace.WriteLine(string.Format("Level 4: Naked Pair.. processing column {0} ", factory.colIndex));
            //else
            //    Trace.WriteLine(string.Format("Level 4: Naked Pair.. processing block {0} ", factory.BlockIndex));

            if (onlyActive.Count > 0) {
                List<int> completeUniquePossibles = onlyActive.getPossibleUnique();
                List<MatrixItem> possibilityMatrix = new List<MatrixItem>();
                possibilityMatrix = onlyActive.generatePossiblityMatrix();

                // NAKED PAIR
                List<MatrixItem> Candidates = possibilityMatrix.Where(x => x.items.Count == 2).Where(z => z.UnquePossibles.Count == 2).ToList();
                foreach (var item in Candidates) {
                    if (factory.Currently == IterationFactory.currentIteration.Row)
                        Trace.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in row {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.rowIndex.ToString()));
                    else if (factory.Currently == IterationFactory.currentIteration.Column)
                        Trace.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in column {2}", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.colIndex.ToString()));
                    else
                        Trace.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in block {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.BlockIndex.ToString()));

                    //remove the candidates unique keys from the items to clean list 
                    List<int> candidateKeys = Candidates.getUnqiues();
                    List<item> remove = onlyActive.Where(x => candidateKeys.Contains(x.unique) == false).ToList();
                    //Trace.WriteLine(string.Format("Possible Naked Pair items to clean {0} ", remove.Count.ToString()));
                    int count;

                    foreach (int controlRemove in item.ControlPossibles) {
                        remove.RemovePossibilities(controlRemove, out count);
                        if (count > 0)
                            progressMade = true;
                    }
                }

                ////NAKED TRIPLE
                //if (progressMade == false)
                //{
                //    List<MatrixItem> simpleMatrix = new List<MatrixItem>();
                //    simpleMatrix = onlyActive.generateSimpleMatrix();

                //    // remove the control and see what we have left in possibles
                //    foreach (var item in simpleMatrix) {
                //        item.UnquePossibles.Remove(item.ControlPossibles[0]);
                //    }

                //    //give me a list of all simple matrix items, when filtered by the control, return 2 unique possibles
                //    List<MatrixItem> TripleCandidates = simpleMatrix.Where(x => x.UnquePossibles.Count == 2).ToList();
                //    List<MatrixItem> FilteredTriples = new List<MatrixItem>();

                //    MatrixItem mi = new MatrixItem();
                //    mi.ControlPossibles =   

                //    FilteredTriples.Add()
                //    //List<item> combined = new List<item>();
                //    //foreach (MatrixItem item in TripleCandidates) {
                //    //    combined.Union(item.items).Distinct();
                //    //}

                //    if (TripleCandidates.Count > 0)
                //    {
                //        foreach (MatrixItem fil in TripleCandidates)
                //        {
                //            if (factory.Currently == IterationFactory.currentIteration.Row)
                //                Trace.WriteLine(string.Format("Possible Naked Triple ({0},{1},{2}) in row {3} ", fil.ControlPossibles[0].ToString(), fil.UnquePossibles[0].ToString(), fil.UnquePossibles[1].ToString(), factory.rowIndex.ToString()));
                //            else if (factory.Currently == IterationFactory.currentIteration.Column)
                //                Trace.WriteLine(string.Format("Possible Naked Triple ({0},{1},{2}) in column {3}", fil.ControlPossibles[0].ToString(), fil.UnquePossibles[0].ToString(), fil.UnquePossibles[1].ToString(), factory.colIndex.ToString()));
                //            else
                //                Trace.WriteLine(string.Format("Possible Naked Triple ({0},{1},{2}) in block {3} ", fil.ControlPossibles[0].ToString(), fil.UnquePossibles[0].ToString(), fil.UnquePossibles[1].ToString(), factory.BlockIndex.ToString()));
                            
                //        }
                //    }

                //}
            }

            CurrentItems = factory.getNext(puzzle) ?? null;
        }
        progressMade = false;
    }

    private void iterativeSolverTriples(ref List<item> puzzle)
    {
        IterationFactory factory = new IterationFactory();
        factory.iteration = IterationFactory.iterationType.standardRowColBlk;
        List<item> CurrentItems = factory.getNext(puzzle).ToList();
        Trace.WriteLine("Level 5: Starting Triples search");

        //loop through all the rows, then the columns
        while (CurrentItems != null)
        {
            List<item> onlyActive = CurrentItems.Where(x => x.possibles != null).ToList();

            //for debugging only
            if (factory.Currently == IterationFactory.currentIteration.Row)
                Trace.WriteLine(string.Format("Level 5: Triples .. processing row {0} ", factory.rowIndex));
            else if (factory.Currently == IterationFactory.currentIteration.Column)
                Trace.WriteLine(string.Format("Level 5: Triples.. processing column {0} ", factory.colIndex));
            else
                Trace.WriteLine(string.Format("Level 5: Triples.. processing block {0} ", factory.BlockIndex));

            if (onlyActive.Count > 0)
            {
                //Type "3" triples.. where one item has 3 of the possibles of the triple
                List<item> step1 = onlyActive.Where(x => x.possibles.Count == 3).ToList();
                foreach (var pos in step1)
                {
                    List<MatrixItem> step2 = step1.generatePossiblityMatrix(pos).ToList();
                    List<MatrixItem> step25 = new List<MatrixItem>();
                    foreach (var item in step2)
                    {
                        item.items.Remove(item);
                    }                    
                }


            }

            CurrentItems = factory.getNext(puzzle) ?? null;
        }
        progressMade = false;

    }

    /// <summary>
    /// Solve for nakid pairs
    /// </summary>
    /// <param name="puzzle"></param>
    //private void iterativeSolverNakedPairs(ref List<item> puzzle)
    //{
    //    IterationFactory factory = new IterationFactory();
    //    factory.iteration = IterationFactory.iterationType.standardRowCol;
    //    List<item> CurrentItems = factory.getNext(puzzle);
    //    Trace.WriteLine("Starting Naked Pair search");

    //    while (CurrentItems != null)
    //    {
    //        //Naked pairs will only have two possible answers
    //        List<item> pairs = CurrentItems.Where(y => y.possibles != null).Where(x => x.possibles.Count == 2).ToList();

    //        if (factory.Currently == IterationFactory.currentIteration.Row)
    //            Trace.WriteLine(string.Format("Level 4: Naked Pair.. processing row {0} ", factory.rowIndex));
    //        else
    //            Trace.WriteLine(string.Format("Level 4: Naked Pair.. processing column {0} ", factory.colIndex));


    //        if (pairs == null) {   
    //            CurrentItems = factory.getNext(puzzle);
    //            continue;
    //        }

    //        if (pairs.Count == 0) {
    //            CurrentItems = factory.getNext(puzzle);
    //            continue;
    //        }
            
    //        if (pairs.Count > 2)
    //        {
    //            //todo not sure what to do if there are more then 2 naked pairs, is it possible.. yes
    //        } else {

    //            //only two, you can see if they are a naked pair.
    //            List<item> candidates = CurrentItems.Where(x => x.possibles != null).ToList();
    //            int removeCnt = candidates.RemoveAll(x => x.unique == (pairs.SingleOrDefault(z => z.unique == x.unique) ?? new item(0, 0, 0, 0)).unique);

    //            if (candidates.Count == 0) {
    //                Trace.WriteLine(string.Format("Level 4: Naked Pair.. no candidates left "));
    //                CurrentItems = factory.getNext(puzzle);
    //                continue;
    //            }

    //            if (pairs.getPossibleUniqueString().Length > 2)
    //            {
    //                Trace.WriteLine(string.Format("Level 4: Naked Pair.. Not a Naked Pair ({0}) ", pairs.getPossiblesString()));
    //                CurrentItems = factory.getNext(puzzle);
    //                continue;
    //            }

    //            Trace.WriteLine(string.Format("Remove Count {0}", removeCnt));
    //            Trace.WriteLine(string.Format("Level 4: Naked Pair: possible string -{0}- |  pair => {1}", candidates.getPossibleUniqueString(), pairs.getPossibleUniqueString()));

    //            //now loop through the candidates left, and remove possibles
    //            foreach (var i in pairs[0].possibles)
    //            {
    //                candidates.RemovePossibilities(i);
    //            }                
            
    //        }

    //        CurrentItems = factory.getNext(puzzle);
    //    }
    //    progressMade = false;
    //}




    private void interativeSolverHiddenPair(ref List<item> puzzle)
    { 
    
    }

    #region Trace Output
    private void writeEntirePuzzle(List<item> puzzle)
    {
        for (int i = 1; i <= 9; i++)
        {
            List<item> row = puzzle.Where(x => x.Row == i).ToList();
            writeDebugRow(row);
        }

    }

    private void writeEntirePuzzelPossibles(List<item> puzzle)
    {
        for (int i = 1; i <= 9; i++)
        {
            List<item> row = puzzle.Where(x => x.Row == i).ToList();
            writeDebugRowPossibles(row);
        }

    }

    private void writeDebugRowPossibles(List<item> row)
    {
        StringBuilder RowOut = new StringBuilder();
        RowOut.Append(string.Format("Row-Out ({0}) ", row.First().Row.ToString()));
        row.OrderBy(x => x.Column);     //make sure it's in order

        foreach (var i in row)
        {
            StringBuilder internalTemp = new StringBuilder();
            if (i.possibles == null)
            {
                internalTemp.Append(string.Format("----{0}----", i.Value));
            } else { 
                foreach (var thisInt in i.possibles) {
                    internalTemp.Append(string.Format("{0}", thisInt.ToString()));
                }
            }
            RowOut.Append(string.Format("({0,9})", internalTemp.ToString()));
        }
        Trace.WriteLine(RowOut.ToString());
    }

    /// <summary>
    /// outputs a row for the puzzle to trace
    /// </summary>
    /// <param name="row"></param>
    private void writeDebugRow(List<item> row)
    {
        StringBuilder RowOut = new StringBuilder();
        RowOut.Append(string.Format("Row-Out ({0}) ", row.First().Row.ToString()));
        string outPutValue = "-";
        row.OrderBy(x => x.Column);     //make sure it's in order

        foreach (var i in row)
	    {
            outPutValue = "-";
            if (i.Value != null) 
                outPutValue = i.Value.ToString();

            RowOut.Append(string.Format(" {0} ", outPutValue)); 
	    }
        Trace.WriteLine(RowOut.ToString());
    }
    #endregion

    }   //class
} //namespace
