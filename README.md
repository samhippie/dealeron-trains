This is my solution for problem three, Trains.

# Running

You can run the program with the `dotnet run` command. You can either provide the input as a command line input in a string or via stdin. If you use stdin, the program won't begin processing input until it receives an EOF. For example, you can run the program with bash from the root of the repo like this

```dotnet run --project dealeron-trains/dealeron-trains.csproj "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7"```

or like this

```echo "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7" | dotnet run --project dealeron-trains/dealeron-trains.csproj```

I've also included some unit tests, and those can be run outside of visual studio with a simple `dotnet test`.

# Notes

I implemented the graph using standard adjacency lists. The given outputs could be calculated either with simple traversal, breadth-first search, or a modified version of Dijkstra's algorithm. I did end up implementing my own heap so that I could provide a priority queue for Dijkstra's algorithm. I would prefer to use a library for this, but C# doesn't seem to have anything without using a third party library. The rule that "A" isn't a valid path from "A" to "A" did force me to make some changes to some of my algorithms, but I was able to make the adjustment without affecting the efficiency of the code.
