/*
Rotate Matrix: Given an image represented by an NxN matrix, where each pixel is 4 bytes, write a method to rotate the image by 90 degrees. Can you do this in place?
*/

public class Pixel
{
  public byte R { get; set; } = 0;
  public byte G { get; set; } = 0;
  public byte B { get; set; } = 0;
  public byte A { get; set; } = 1;
}

// O(n^2) runtime
// O(1) space
public static void Rotate90<T>(T[,] matrix)
{
  if (matrix.GetLength(0) != matrix.GetLength(1))
  {
    throw new ArgumentException("Image is not square");
  }

  for (int row = 0; row < matrix.GetLength(0) / 2; row++)
  {
    for (int col = row; col < matrix.GetLength(0) - row - 1; col++)
    {
      RotatePoint(matrix, row, col);
    }
  }
}

// O(1) runtime
// O(1) space
private static void RotatePoint<T>(T[,] matrix, int row, int col)
{
  if (matrix.GetLength(0) != matrix.GetLength(1))
  {
    throw new ArgumentException("Matrix is not square");
  }

  int end = matrix.GetLength(0) - 1;

  // point positions:
  // [A B]
  // [C D]

  // A and D
  SwapPoints(matrix, row, col,
    end - row, end - col);
  // A and C
  SwapPoints(matrix, row, col,
    end - col, row);
  // B and D
  SwapPoints(matrix, col, end - row,
    end - row, end - col);
}

// O(1) runtime
// O(1) space
private static void SwapPoints<T>(T[,] matrix, int row1, int col1, int row2, int col2)
{
  T temp = matrix[row2, col2];
  matrix[row2, col2] = matrix[row1, col1];
  matrix[row1, col1] = temp;
}

