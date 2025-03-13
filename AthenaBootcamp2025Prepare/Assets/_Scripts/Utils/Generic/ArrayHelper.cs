using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHelper {
    public static int[,] Create2DArray(int height, int width)
    {
        return new int[height, width]; // Automatically initialized to 0
    }
}
