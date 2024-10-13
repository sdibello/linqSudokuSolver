[1mdiff --git a/SudokuSolverEngine/Solver.cs b/SudokuSolverEngine/Solver.cs[m
[1mindex 1edb183..83c2c06 100644[m
[1m--- a/SudokuSolverEngine/Solver.cs[m
[1m+++ b/SudokuSolverEngine/Solver.cs[m
[36m@@ -265,32 +265,32 @@[m [mnamespace SudokuSolverEngine.Solver[m
                 //    Console.WriteLine(string.Format("Level 4: Naked Pair.. processing block {0} ", factory.BlockIndex));[m
 [m
                 if (onlyActive.Count > 0) {[m
[31m-                List<int> completeUniquePossibles = onlyActive.getPossibleUnique();[m
[31m-                List<MatrixItem> possibilityMatrix = new List<MatrixItem>();[m
[31m-                possibilityMatrix = onlyActive.generatePossiblityMatrix();[m
[31m-[m
[31m-                // NAKED PAIR[m
[31m-                List<MatrixItem> Candidates = possibilityMatrix.Where(x => x.items.Count == 2).Where(z => z.UnquePossibles.Count == 2).ToList();[m
[31m-                foreach (var item in Candidates) {[m
[31m-                    if (factory.Currently == IterationFactory.currentIteration.Row)[m
[31m-                            Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in row {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.rowIndex.ToString()));[m
[31m-                    else if (factory.Currently == IterationFactory.currentIteration.Column)[m
[31m-                            Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in column {2}", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.colIndex.ToString()));[m
[31m-                    else[m
[31m-                            Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in block {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.BlockIndex.ToString()));[m
[31m-[m
[31m-                    //remove the candidates unique keys from the items to clean list [m
[31m-                    List<int> candidateKeys = Candidates.getUnqiues();[m
[31m-                    List<item> remove = onlyActive.Where(x => candidateKeys.Contains(x.unique) == false).ToList();[m
[31m-                        //Console.WriteLine(string.Format("Possible Naked Pair items to clean {0} ", remove.Count.ToString()));[m
[31m-                        int count;[m
[31m-[m
[31m-                    foreach (int controlRemove in item.ControlPossibles) {[m
[31m-                        remove.RemovePossibilities(controlRemove, out count);[m
[31m-                        if (count > 0)[m
[31m-                            progressMade = true;[m
[32m+[m[32m                    List<int> completeUniquePossibles = onlyActive.getPossibleUnique();[m
[32m+[m[32m                    List<MatrixItem> possibilityMatrix = new List<MatrixItem>();[m
[32m+[m[32m                    possibilityMatrix = onlyActive.generatePossiblityMatrix();[m
[32m+[m
[32m+[m[32m                    // NAKED PAIR[m
[32m+[m[32m                    List<MatrixItem> Candidates = possibilityMatrix.Where(x => x.items.Count == 2).Where(z => z.UnquePossibles.Count == 2).ToList();[m
[32m+[m[32m                    foreach (var item in Candidates) {[m
[32m+[m[32m                        if (factory.Currently == IterationFactory.currentIteration.Row)[m
[32m+[m[32m                                Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in row {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.rowIndex.ToString()));[m
[32m+[m[32m                        else if (factory.Currently == IterationFactory.currentIteration.Column)[m
[32m+[m[32m                                Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in column {2}", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.colIndex.ToString()));[m
[32m+[m[32m                        else[m
[32m+[m[32m                                Console.WriteLine(string.Format("Possible Naked Pair ({0},{1}) in block {2} ", item.ControlPossibles[0].ToString(), item.ControlPossibles[1].ToString(), factory.BlockIndex.ToString()));[m
[32m+[m
[32m+[m[32m                        //remove the candidates unique keys from the items to clean list[m[41m [m
[32m+[m[32m                        List<int> candidateKeys = Candidates.getUnqiues();[m
[32m+[m[32m                        List<item> remove = onlyActive.Where(x => candidateKeys.Contains(x.unique) == false).ToList();[m
[32m+[m[32m                            //Console.WriteLine(string.Format("Possible Naked Pair items to clean {0} ", remove.Count.ToString()));[m
[32m+[m[32m                            int count;[m
[32m+[m
[32m+[m[32m                        foreach (int controlRemove in item.ControlPossibles) {[m
[32m+[m[32m                            remove.RemovePossibilities(controlRemove, out count);[m
[32m+[m[32m                            if (count > 0)[m
[32m+[m[32m                                progressMade = true;[m
[32m+[m[32m                        }[m
                     }[m
[31m-                }[m
 [m
                 ////NAKED TRIPLE[m
                 //if (progressMade == false)[m
[36m@@ -331,7 +331,7 @@[m [mnamespace SudokuSolverEngine.Solver[m
                 //    }[m
 [m
                 //}[m
[31m-            }[m
[32m+[m[32m                }[m
 [m
             CurrentItems = factory.getNext(puzzle) ?? null;[m
         }[m
