using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Send your busters out into the fog to trap ghosts and bring them home!
 **/

class Cell
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return X+" "+Y;
    }
}

class Entity
{
    public Cell Position { get; set; }
    public int Id { get;}
    public String Type { get; }

    public Entity(Cell position, int id, String type)
    {
        this.Position = position;
        this.Id = id;
        this.Type = type;        
    }

}

class Buster : Entity
{
    public bool IsCarrying { get; set; }
    public Ghost GhostCarryied { get; set; }

    public Buster(Cell position, int id, String team) : base(position, id, team + "_Buster")
    {
        this.IsCarrying = false;
        this.GhostCarryied = null;
    }

    //walking methods
}

class Ghost : Entity
{
    public int NumberOfBusters { get; set; }

    public Ghost(Cell position, int id, int numberOfBusters):base(position,id,"Ghost")
    {
        this.NumberOfBusters = numberOfBusters;
    }
}


class Player
{
    static void Main(string[] args)
    {
        int bustersPerPlayer = int.Parse(Console.ReadLine()); // the amount of busters you control
        int ghostCount = int.Parse(Console.ReadLine()); // the amount of ghosts on the map
        int myTeamId = int.Parse(Console.ReadLine()); // if this is 0, your base is on the top left of the map, if it is one, on the bottom right

        // game loop
        while (true)
        {
            int entities = int.Parse(Console.ReadLine()); // the number of busters and ghosts visible to you
            for (int i = 0; i < entities; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                int entityId = int.Parse(inputs[0]); // buster id or ghost id
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]); // position of this buster / ghost
                int entityType = int.Parse(inputs[3]); // the team id if it is a buster, -1 if it is a ghost.
                int state = int.Parse(inputs[4]); // For busters: 0=idle, 1=carrying a ghost.
                int value = int.Parse(inputs[5]); // For busters: Ghost id being carried. For ghosts: number of busters attempting to trap this ghost.
            }
            for (int i = 0; i < bustersPerPlayer; i++)
            {

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                Console.WriteLine("MOVE 8000 4500"); // MOVE x y | BUST id | RELEASE
            }
        }
    }
}