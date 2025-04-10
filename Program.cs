using MineSweeperClasses;

namespace MineSweeperConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, welcome to Minesweeper");
            // size 10 and difficulty 0.1
            Board board = new Board(10, 0.1f);
            PrintAnswers(board);
            Console.WriteLine("Here is the current board.");
            PrintBoard(board);
            
            // Deactivate second board game
            // Size 15 and difficulty 0.15
            //board = new Board(15, 0.15f);
            //Console.WriteLine("Here is the answer key for the second board.");
            //PrintAnswers(board);

            bool victory = false; // player wins
            bool death = false; // player loses

            // Repeat until game is over
            while (!victory && !death)
            {
                // Prompt the player to play
                
                   
                    Console.WriteLine("Enter a row number from the board: ");
                    int row = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter a column number from the board: ");
                    int col = int.Parse(Console.ReadLine());
                
                // Console.WriteLine("Choose an action: Flag or Visit");

                // Ask if this is for Flag or Visit or Use Reward only after the player has collected a reward
                Console.WriteLine("Choose an action: Flag, Visit, or Use Reward");
                // Get the action from the player
                string action = Console.ReadLine();

                // Update cell if Flag was chosen
                if (action == "Flag")
                {
                    
                    board.Cells[row, col].IsFlagged = true;
                    
                    Console.WriteLine($"Cell ({row}, {col}) is flagged.");
                }
                else if (board.Cells[row, col].HasSpecialReward && action == "Visit")
                    {
                    board.Cells[row, col].IsVisited = true;
                    Console.WriteLine($"Cell ({row}, {col}) has a special reward.");
                    }
                // Update cell if Visit is chosen
                else if (action == "Visit")
                {
                    board.PushUndo(board.Cells[row, col]);
                    // Check if the cell is a bomb
                    if (board.Cells[row, col].IsBomb)
                    {
                        death = true;
                        board.Cells[row, col].IsVisited = true;
                        
                        Console.WriteLine($"Cell ({row}, {col}) is a bomb.");
                        
                    }
                    
                    
                    // Not a bomb
                    else
                    {
                        // Run flood fill check on the board and print results 
                        board.FloodFill(row, col, PrintBoard);                     

                        Console.WriteLine($"Cell ({row}, {col}) has been visited. No bomb present.");
                    }
                }
                // Use the special reward
               else if (board.RewardsRemaining > 0)
                {
                    Console.WriteLine("Would you like to use your undo reward? Type 'Yes' to use reward or press Enter to continue:");
                    string input = Console.ReadLine();
                    if (input == "Yes")
                    {
                        if (board.RewardsRemaining > 0)
                        {
                            board.UseSpecialReward();
                            Console.WriteLine($"Undo used. Remaining rewards: {board.RewardsRemaining}");
                        }
                        else
                        {
                            Console.WriteLine("No rewards available.");
                        }
                    }
                }
                // Invalid input
                else
                {
                    Console.WriteLine("Input invalid input. Try again.");
                }
                

                

                victory = true;
                //Force to visit cells until all non-bomb cells have been found.
                foreach (Cell cell in board.Cells)
                {
                    if (!cell.IsBomb && !cell.IsVisited)
                    {
                        victory = false; // Found a non-bomb cell that isn't visited
                        break;
                    }
                }
                
               
                PrintBoard(board);

            }
            
             //If not death then the play has won otherwise they lost
                if (!death)
                {
                    Console.WriteLine("You won the game!");
                }
                else
                {
                    Console.WriteLine("You hit a bomb. You lost the game.");
                }
            
            Console.WriteLine("Here is the answer key for the board.");
            PrintAnswers(board);
        }
        // Print the board
        public static void PrintBoard(Board board)
        {
            // Print column numbers
            Console.WriteLine(" ");
            Console.Write("  ");
            for (int col = 0; col < board.Size; col++)
            {

                Console.Write($" {col}");
            }
            Console.WriteLine();

            // Print divider line
            Console.WriteLine(new string('-', board.Size * 2 + 3));

            // Print the rows and cells
            for (int row = 0; row < board.Size; row++)
            {
                // Print row number
                Console.Write($"{row}| ");
                // Print the cells
                for (int col = 0; col < board.Size; col++)
                {
                    PrintCell(board.Cells[row, col]);
                }
                Console.WriteLine();
            }
            // Print divider line
            Console.WriteLine(new string('-', board.Size * 2 + 3));
        }
        // Print a single cell
        static void PrintCell(Cell cell)
        {
            if (!cell.IsVisited && !cell.IsFlagged)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("? ");
            }
            else if (cell.IsBomb)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("B ");
            }
            else if (cell.NumberOfBombNeighbors > 0)
            {
                SetNeighborColor(cell.NumberOfBombNeighbors);
                // Print the number of bomb neighbors
                Console.Write($"{cell.NumberOfBombNeighbors} ");
            }
            else if (cell.HasSpecialReward && cell.IsVisited)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("R ");
            }
            else if (cell.IsFlagged)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("F ");
            }
            else
            {
                // Apply flood fill
                cell.IsVisited = true; // Mark the cell as visited

                // If the cell is not a bomb and has no neighbors, print a dot
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(". ");
            }
            Console.ResetColor();
        }
        // Set color based on number of bombs near by
        static void SetNeighborColor(int bombNeighbors)
        {
            switch (bombNeighbors)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

// Method to print the board answers
static void PrintAnswers(Board board)
        {
            // Print column numbers
            Console.WriteLine(" ");
            Console.Write("  ");
            for (int col = 0; col < board.Size; col++)
            {
                
                Console.Write($" {col}");
            }
            Console.WriteLine();

            // Print divider line
            Console.WriteLine(new string('-', board.Size * 2 + 3));

            // Print the rows and cells
            for (int row = 0; row < board.Size; row++)
            {
                // Print row number
                Console.Write($"{row}| ");
                // Print the cells
                for (int col = 0; col < board.Size; col++)
                {
                    // If the cell is flagged, print a flag
                    if (board.Cells[row, col].IsFlagged)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("F ");
                    }
                    // If the cell contains a special reward, print a reward
                    else if (board.Cells[row, col].HasSpecialReward)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("R ");
                    }
                    // Print the bomb if it is a bomb
                    else if (board.Cells[row, col].IsBomb)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("B ");
                    }
                    // Print the number of bomb neighbors
                    else if (board.Cells[row, col].NumberOfBombNeighbors > 0)
                    {
                        SetNeighborColor(board.Cells[row, col].NumberOfBombNeighbors);
                        Console.Write($"{board.Cells[row, col].NumberOfBombNeighbors} ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(". ");
                    }

                    Console.ResetColor();
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', board.Size * 2 + 3));
        }
    }
}
