using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossword : MonoBehaviour {

  public static char[] g = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; 

  public int size;

  private char [,] M;

    void Start() {

      size = 5;

      M = new char [size, size];

      for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
          M[i, j] = g[5 * i + j];
        }
      }  

    }

    void Update() {
        
    }
}
