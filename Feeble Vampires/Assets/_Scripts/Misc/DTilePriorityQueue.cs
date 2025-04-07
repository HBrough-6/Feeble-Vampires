using System.Collections.Generic;

public class DTilePriorityQueue
{
    private List<DTile> PQ = new List<DTile>();

    // Lambda expression to find the parent index
    private int Parent(int i) => (i - 1) / 2;
    // Lambda expression to find the left child index
    private int Left(int i) => 2 * i + 1;
    // Lambda expression to find the right child index
    private int Right(int i) => 2 * i + 2;

    public int Count => PQ.Count;
    public bool Empty => PQ.Count < 1;

    private void Swap(int i, int j)
    {
        DTile temp = PQ[i];
        PQ[i] = PQ[j];
        PQ[j] = temp;
    }

    public void Insert(DTile tile)
    {
        // add the tile at the end of the heap
        PQ.Add(tile);
        // get the index of the new element
        int i = PQ.Count - 1;

        // compare the distance of the parent and new tile
        while (i > 0 && PQ[Parent(i)].dist > PQ[i].dist)
        {
            // swap until the parent dist is less than the new tile dist
            Swap(i, Parent(i));
            i = Parent(i);
        }
    }

    public DTile Pop()
    {
        if (PQ.Count == 0)
            throw new System.InvalidOperationException("Heap is empty");

        // store the smallest value on the heap
        DTile min = PQ[0];
        // replace the root with the largest element to bubble down
        PQ[0] = PQ[PQ.Count - 1];
        // remove the last element from the heap
        PQ.RemoveAt(PQ.Count - 1);
        // downwards heapify from the root
        MinHeapify(0);
        return min;
    }

    private void MinHeapify(int i)
    {
        int smallest = i;
        int left = Left(i);
        int right = Right(i);

        if (left < PQ.Count && PQ[left].dist < PQ[smallest].dist)
            smallest = left;
        if (right < PQ.Count && PQ[right].dist < PQ[smallest].dist)
            smallest = right;

        if (smallest != i)
        {
            Swap(i, smallest);
            MinHeapify(smallest);
        }
    }

    private void UWHeapify(DTile Tile)
    {
        if (PQ.Count == 0)
            throw new System.InvalidOperationException("Heap is empty");

        int i = PQ.IndexOf(Tile);
        if (i < 0)
            throw new System.InvalidOperationException("Tile not in heap");

        if (i < PQ.Count && PQ[i].dist < PQ[Parent(i)].dist)
        {
            Swap(i, Parent(i));
            i = Parent(i); ;

        }
    }

    public string PrintDist()
    {
        string st = "";
        for (int i = 0; i < PQ.Count; i++)
        {
            st += PQ[i].dist + ", ";
        }
        return st;
    }
}
