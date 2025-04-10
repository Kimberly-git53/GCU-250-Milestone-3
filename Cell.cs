using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperClasses
{
    public class Cell
    {
        // Properties
        public int Row { get; set; } = -1;
        public int Col { get; set; } = -1;
        public bool IsVisited { get; set; } = false;
        public bool IsBomb { get; set; } = false;
        public bool IsFlagged { get; set; } = false;
        public int NumberOfBombNeighbors { get; set; } = 0;
        public bool HasSpecialReward { get; set; } = false;

        // Constructor
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }
        // Method to define undo action
        public void Undo()
        {
            // Undo flagging
            if (IsFlagged)
            {
                IsFlagged = false; // Remove the flag
                Console.WriteLine("Flag undone.");
            }
            // Undo visiting (mark the cell as unvisited)
            else if (IsVisited)
            {
                IsVisited = false; // Reset visited status
                Console.WriteLine("Cell has been reset to unvisited.");
            }

        }
    }
}
