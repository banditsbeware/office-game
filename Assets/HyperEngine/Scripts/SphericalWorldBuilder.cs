using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalWorldBuilder : WorldBuilder
{
    public static int RADIUS = 2;
    public GameObject debug_tile;
    public string pathToTiles = "";

    public override int MaxExpansion()
    {
        HM.SetTileType(3);
        return RADIUS;
    }

    public override GameObject GetTile(string coord)
    {
        string path = pathToTiles + "/tile_" + coord;
        GameObject tile = Resources.Load<GameObject>(path);

      
        
        if (tile == null)
        {  
            Debug.Log("Used Debug!");
            return Instantiate(debug_tile);
        }
        return Instantiate(tile);

    }
}
