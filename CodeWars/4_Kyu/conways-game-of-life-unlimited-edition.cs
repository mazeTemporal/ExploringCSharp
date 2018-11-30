// https://www.codewars.com/kata/52423db9add6f6fc39000354/train/csharp

/*
Given a 2D array and a number of generations, compute n timesteps of Conway's Game of Life.

The rules of the game are:

Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
Any live cell with more than three live neighbours dies, as if by overcrowding.
Any live cell with two or three live neighbours lives on to the next generation.
Any dead cell with exactly three live neighbours becomes a live cell.
Each cell's neighborhood is the 8 cells immediately around it (i.e. Moore Neighborhood). The universe is infinite in both the x and y dimensions and all cells are initially dead - except for those specified in the arguments. The return value should be a 2d array cropped around all of the living cells. (If there are no living cells, then return [[]].)

For illustration purposes, 0 and 1 will be represented as ░░ and ▓▓ blocks respectively (PHP, C: plain black and white squares). You can take advantage of the htmlize function to get a text representation of the universe, e.g.:

trace (htmlize cells)
*/

using System;

public class ConwayLife
{
  public static int[,] GetGeneration(int[,] cells, int generation)
    {
      if (1 > generation)
      {
        return (cells);
      }
      // update cells to next generation
      int[,] nextGen = GetNextGeneration(cells);
      // call recursively
      return (GetGeneration(nextGen, generation - 1));
    }

    // given array of live/dead cells, generate next iteration of cells
    public static int[,] GetNextGeneration(int[,] cells)
    {
      int[,] nextGen = new int[2 + cells.GetLength(0), 2 + cells.GetLength(1)];
      int rowMax = cells.GetLength(0);
      int colMax = cells.GetLength(1);
      // iterate through nextGen
      for (int i = 0; i < nextGen.GetLength(0); ++i)
      {
        for (int j = 0; j < nextGen.GetLength(1); ++j)
        {
          // iterate through neighbors, minding array boundaries
          int count = 0;
          for (int row = Math.Max(1, i - 1); row <= Math.Min(i + 1, rowMax); ++row)
          {
            for (int col = Math.Max(1, j - 1); col <= Math.Min(j + 1, colMax); ++col)
            {
              if (row != i || col != j)
              {
                count += cells[row - 1, col - 1];
              }
            }
          }
          // determine if will be live or dead
          bool inRange = 0 < i && 0 < j && rowMax >= i && colMax >= j;
          bool wasLive = inRange && 1 == cells[i - 1, j - 1];
          nextGen[i, j] = IsLive(wasLive, count) ? 1 : 0;
        }
      }
      return (SimplifyEdges(nextGen));
    }

    // previous status and neighbor count
    // return true if cell will be live
    public static bool IsLive(bool wasLive, int neighborCount)
    {
      return (3 == neighborCount || (2 == neighborCount && wasLive));
    }

    // remove empty edge rows and columns
    public static int[,] SimplifyEdges(int[,] cells)
    {
      // track minimum and maximum live rows/columns
      int[] rowRange = new int[2] { cells.GetLength(0), -1 };
      int[] colRange = new int[2] { cells.GetLength(1), -1 };
      for (int row = 0; row < cells.GetLength(0); ++row)
      {
        for (int col = 0; col < cells.GetLength(1); ++col)
        {
          if (1 == cells[row, col])
          {
            rowRange[0] = Math.Min(row, rowRange[0]);
            rowRange[1] = Math.Max(row, rowRange[1]);
            colRange[0] = Math.Min(col, colRange[0]);
            colRange[1] = Math.Max(col, colRange[1]);
          }
        }
      }
      return (GetSubArray(cells, rowRange, colRange));
    }

    // creates subset of array from ranges [xStart, xEnd] and [yStart, yEnd]
    public static int[,] GetSubArray(int[,] arr, int[] xRange, int[] yRange)
    {
      int[,] subArr = new int[
        Math.Max(0, xRange[1] - xRange[0] + 1),
        Math.Max(0, yRange[1] - yRange[0] + 1)
      ];
      for (int x = 0; x < subArr.GetLength(0); ++x)
      {
        for (int y = 0; y < subArr.GetLength(1); ++y)
        {
          subArr[x, y] = arr[x + xRange[0], y + yRange[0]];
        }
      }
      return (subArr);
    }
}

