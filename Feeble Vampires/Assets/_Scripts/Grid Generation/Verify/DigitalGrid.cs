using System.Collections.Generic;
using UnityEngine;

public class DigitalGrid
{
    // GOAL: create a data representation of the grid so that i can check the validity of the grid without using raycasts
    // set up the grid ---- DONE
    // fill grid with chunks (done in grid manager) -- DONE
    // then verify -- check multiple starting tiles
    public DTile[] grid;

    public int width, height;
    // TileIndex = .x + .y * 8 * width;
    public void SetUpGrid(int width, int height)
    {
        //Debug.Log("setting width of " + width + " and height of " + height);
        this.width = width;
        this.height = height;
        grid = new DTile[width * height * 8 * 8];

        // fill the grid with tiles 
        for (int row = 0; row < height * 8; row++)
        {
            for (int col = 0; col < width * 8; col++)
            {
                //Debug.Log("index: " + GetTileIndex(col, row) + "\n length of array: " + grid.Length);
                // create a new tile and set its position
                grid[GetTileIndex(col, row)] = new DTile(0, new Vector2Int(col, row));
                //Debug.Log(col + "," + row + "---" + width * 8 + "," + height * 8);
            }
        }
    }

    // returns the index of a tile in the grid array
    public int GetTileIndex(int col, int row)
    {
        return col + (row * width * 8);
    }

    // returns the index of a tile in the grid array
    public int GetTileIndex(Vector2Int posiiton)
    {
        return posiiton.x + (posiiton.y * width * 8);
    }

    public void PlaceChunk(int[,] chunk, Vector2Int chunkPos)
    {
        // Debug.Log("Placing new Chunk" + chunkPos);

        if (chunkPos.x >= width || chunkPos.y >= height)
        {
            Debug.Log(chunkPos + " is out of bounds, width: " + width + " height: " + height + "sdoifds");
            return;
        }
        Vector2Int Offset = new Vector2Int(chunkPos.x * 8, chunkPos.y * 8);

        // place tiles in the grid
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // set the tile type;
                grid[GetTileIndex(Offset.x + col, Offset.y + row)].type = chunk[col, row];
                //Debug.Log("Set Tile: (" + (Offset.x + col) + "," + (Offset.y + row) + ") to " + chunk.chunkData.GetCell(col, row) + "\nHas tile index of " + GetTileIndex(Offset.x + col, Offset.y + row));
                // should look like: Set Tile: (0, 0) to 1
                //                   Has tile index of 0
            }
        }
    }

    public bool VerifyPos(Vector2Int position)
    {
        // check if the x and y position passed in is within the bounds of the grid
        if (position.x < 0 || position.x >= width * 8 || position.y < 0 || position.y >= height * 8)
            return false;
        return true;
    }

    public Vector2Int GetChunkLocation(Vector2Int pos)
    {
        int x = pos.x / 8;
        int y = pos.y / 8;

        return new Vector2Int(x, y);
    }

    public Vector2Int GetChunkLocation(int x, int y)
    {
        return new Vector2Int(x % 8, y % 8);
    }

    public void ResetGridAfterSearch()
    {
        // reset all tiles to unfound
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].found = false;
            grid[i].prev = null;
        }
    }

    /// <summary>
    /// Checks if the digital grid is valid
    /// </summary>
    /// <returns>returns true if the grid is valid</returns>
    public DResult Verify()
    {
        int timesRun = 0;
        Debug.Log(grid.Length);
        // reset all tiles to make sure they are all unfound
        for (int i = 0; i < grid.Length; i++)
        {
            //Debug.Log(i);
            grid[i].found = false;
        }

        // bfs
        // let Q be a queue
        Queue<DTile> Q = new Queue<DTile>();
        // find possible roots
        List<DTile> root = new List<DTile>();
        // list to store results of each run of BFS
        List<DResult> results = new List<DResult>();

        // find all possible roots
        for (int i = 0; i < width * 8; i++)
        {
            if (grid[i].type != 1 && grid[i].type != 2)
            {
                root.Add(grid[i]);
            }
        }

        // keep verifying while there are possible roots
        while (root.Count > 0)
        {
            timesRun++;
            DResult tempResult = new DResult();

            int rand = Random.Range(0, root.Count);
            DTile startTile = root[rand];
            tempResult.startPoint = startTile.pos;

            Q.Enqueue(startTile);
            startTile.found = true;
            root.RemoveAt(rand);


            // while Q is not empty
            while (Q.Count > 0)
            {
                // pop first tile off queue
                DTile v = Q.Dequeue();

                // if the tile is a sigil and not in the same chunk as the player spawn, add it to the list
                if (v.type == 3 && GetChunkLocation(v.pos) != GetChunkLocation(startTile.pos))
                {
                    tempResult.sigilPoints.Add(v.pos);
                }

                int upIndex = -1;
                // if the position is valid, find the index of the above tile
                if (VerifyPos(v.adjacentTiles[0]))
                    upIndex = GetTileIndex(v.adjacentTiles[0]);

                int downIndex = -1;
                // if the position is valid, find the index of the below tile
                if (VerifyPos(v.adjacentTiles[1]))
                    downIndex = GetTileIndex(v.adjacentTiles[1]);

                int leftIndex = -1;
                // if the position is valid, find the index of the left tile
                if (VerifyPos(v.adjacentTiles[2]))
                    leftIndex = GetTileIndex(v.adjacentTiles[2]);

                int rightIndex = -1;
                // if the position is valid, find the index of the left tile
                if (VerifyPos(v.adjacentTiles[3]))
                    rightIndex = GetTileIndex(v.adjacentTiles[3]);

                /*Debug.Log("Checking neighbors of tile " + v.pos +
                    "\n Up - Pos: " + v.adjacentTiles[0] + " Index: " + upIndex
                    + "\n Down - Pos: " + v.adjacentTiles[1] + " Index: " + downIndex
                    + "\n left - Pos: " + v.adjacentTiles[2] + " Index: " + leftIndex
                    + "\n right - Pos: " + v.adjacentTiles[3] + " Index: " + rightIndex);*/

                // check if the position was validated, tile has not been found, and the tile is not a wall
                if (upIndex != -1 && !grid[upIndex].found && grid[upIndex].type != 1 && grid[upIndex].type != 2)
                {
                    Q.Enqueue(grid[upIndex]);
                    grid[upIndex].found = true;
                }
                // check if the position was validated, tile has not been found, and the tile is not a wall
                if (downIndex != -1 && !grid[downIndex].found && grid[downIndex].type != 1 && grid[downIndex].type != 2)
                {
                    Q.Enqueue(grid[downIndex]);
                    grid[downIndex].found = true;
                }
                // check if the position was validated, tile has not been found, and the tile is not a wall
                if (leftIndex != -1 && !grid[leftIndex].found && grid[leftIndex].type != 1 && grid[leftIndex].type != 2)
                {
                    Q.Enqueue(grid[leftIndex]);
                    grid[leftIndex].found = true;
                }
                // check if the position was validated, tile has not been found, and the tile is not a wall
                if (rightIndex != -1 && !grid[rightIndex].found && grid[rightIndex].type != 1 && grid[rightIndex].type != 2)
                {
                    Q.Enqueue(grid[rightIndex]);
                    grid[rightIndex].found = true;
                }
            }

            // check the results of the BFS
            // end tiles, start tiles, number of available sigils

            // check all the points in the final row
            for (int i = 0; i < width * 8 - 1; i++)
            {
                // check that there are 2 open tiles to place a door
                if (grid[GetTileIndex(i, height * 8 - 1)].found && grid[GetTileIndex(i + 1, height * 8 - 1)].found)
                {
                    // add the position
                    tempResult.endPoints.Add(grid[GetTileIndex(i, height * 8 - 1)].pos);
                }
            }

            int chunkCount = width * height;

            if (chunkCount < 7)
            {
                tempResult.sigilCount = 2;
            }
            else if (chunkCount < 13)
            {
                tempResult.sigilCount = 5;
            }
            else
            {
                tempResult.sigilCount = 6;
            }

            // save the current run through the BFS if it reached end tiles and has enough sigil positions
            if (tempResult.endPoints.Count > 0 && tempResult.sigilPoints.Count > tempResult.sigilCount)
            {
                tempResult.resultOfBFS = new int[width * height * 64];
                for (int i = 0; i < grid.Length; i++)
                {
                    DTile tTile = grid[i];
                    // passes through wall tiles
                    if (tTile.type == 1 || tTile.type == 2)
                    {
                        tempResult.resultOfBFS[i] = tTile.type;
                    }
                    // fills in tiles that weren't found
                    else if (!tTile.found)
                    {
                        tempResult.resultOfBFS[i] = 1;
                    }
                    // passes through empty tiles
                    else
                    {
                        tempResult.resultOfBFS[i] = 0;
                    }
                }
                // place results in the list of results
                results.Add(tempResult);
                //Debug.Log("added result has: " + tempResult.endPoints.Count + "endpoints, " + tempResult.sigilPoints.Count + " sigil points, starts at " + tempResult.startPoint);
            }
            else
            {
                //Debug.Log("failed result has: " + tempResult.endPoints.Count + "endpoints, " + tempResult.sigilPoints.Count + " sigil points, starts at " + tempResult.startPoint);

            }

            string st = "removed start points: ";
            // check all the tiles in the bottom row
            for (int i = 0; i < width * 8; i++)
            {
                DTile temp = grid[GetTileIndex(i, 0)];
                if (temp.found)
                {
                    // if a tile has been found, remove it from the possible root list
                    root.Remove(temp);
                    st += temp.pos + " ";
                }
            }
            //Debug.Log(st);

            // if there are more starting tiles to check, reset all tiles status'
            if (root.Count > 0)
            {
                // reset all tiles to unfound
                for (int i = 0; i < grid.Length; i++)
                {
                    grid[i].found = false;
                }
            }
        }

        // determine the best start point
        if (results.Count == 0)
        {
            //Debug.Log("no results");
            return null;
        }
        else if (results.Count == 1)
        {
            //Debug.Log("Only 1 result");
            return results[0];
        }
        else
        {
            int priority = 0;
            int bestIndex = 0;
            // compare and find the best results
            for (int i = 0; i < results.Count; i++)
            {
                int tempPriority = results[i].endPoints.Count + results[i].sigilPoints.Count;
                //Debug.Log("tempPriority: " + tempPriority + " old priority" + priority);
                if (tempPriority > priority)
                {
                    bestIndex = i;
                }
            }
            return results[bestIndex];
        }
    }
}
