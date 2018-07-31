using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


/**
 * Send your busters out into the fog to trap ghosts and bring them home!
 **/

class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return X+" "+Y;
    }

    public override bool Equals(object obj)
    {
        bool equals;
        if (obj.GetType() != this.GetType())
            equals = false;
        else
        {
            Point point = (Point) obj;
            if (this.X == point.X && this.Y == point.Y)
                equals = true;
            else
            {
                equals = false;
            }

        }

        return equals;
    }

    public static int Distance(Point first, Point second)
    {
        return (int) Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
    }

}

class Entity
{
    public Point Position { get; set; }
    public int Id { get;}
    public String Type { get; }

    public Entity(Point position, int id, String type)
    {
        this.Position = position;
        this.Id = id;
        this.Type = type;
    }

    public override bool Equals(object obj)
    {
        Entity entity = (Entity) obj;
        return Type == entity.Type && Id == entity.Id;
    }
}

class Buster : Entity
{
    public bool IsCarrying { get; set; }
    public Ghost GhostCarryied { get; set; }

    public Buster(Point position, int id, String team, bool isCarrying=false, Ghost ghostCarryied=null) : base(position, id, team + "_Buster")
    {
        IsCarrying = isCarrying;
        GhostCarryied = ghostCarryied;
    }


    public void Bust(Ghost ghost)
    {
        int distance = Point.Distance(Position, ghost.Position);
        if (distance <= 1760 && distance >= 900)
        {
            Console.WriteLine("BUST "+ghost.Id);
        }
        else
        {
            MoveTo(ghost);
        }
    }

    public void MoveTo(Entity entity)
    {
        Console.WriteLine("MOVE "+entity.Position);
    }

    public void MoveTo(Point point)
    {
        Console.WriteLine("MOVE "+point);
    }

    public void Explore()
    {
        if (Id %2==0)
        {
            MoveTo(new Point(10000,5000));//moving right
        }
        else
        {
            MoveTo(new Point(5000,7000));//moving left
        }
    }

    public void Release()
    {
        Console.WriteLine("RELEASE");
        GhostCarryied = null;
        IsCarrying = false;
    }

}

class Ghost : Entity
{
    public int NumberOfBusters { get; set; }
    public int Timer { get; set; } //when Timer ==0 ghost is deleted from the list

    public Ghost(Point position, int id, int numberOfBusters):base(position,id,"Ghost")
    {
        NumberOfBusters = numberOfBusters;
        Timer = 10;
    }
}


class Player
{
    static void Main(string[] args)
    {
        int bustersPerPlayer = int.Parse(Console.ReadLine()); // the amount of busters you control
        int ghostCount = int.Parse(Console.ReadLine()); // the amount of ghosts on the map
        int myTeamId = int.Parse(Console.ReadLine()); // if this is 0, your base is on the top left of the map, if it is one, on the bottom right

        List<Buster> friends = new List<Buster>();
        //List<Buster> enemies = new List<Buster>();

        List<Ghost> ghosts = new List<Ghost>();
        List<Point> bases = new List<Point>();
        bases.Add(new Point(0,0));
        bases.Add(new Point(16000,9000));

        //game loop
        while (true)
        {

            int entities = int.Parse(Console.ReadLine()); // the number of busters and ghosts visible to you
            Console.Error.WriteLine(entities);
            for (int i = 0; i < entities; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');

                int entityId = int.Parse(inputs[0]); // buster id or ghost id
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]); // position of this buster / ghost
                int entityType = int.Parse(inputs[3]); // the team id if it is a buster, -1 if it is a ghost.
                int state = int.Parse(inputs[4]); // For busters: 0=idle, 1=carrying a ghost.
                int value = int.Parse(inputs[5]); // For busters: Ghost id being carried. For ghosts: number of busters attempting to trap this ghost.
                Point position = new Point(x,y);
                Console.Error.WriteLine(i+": entityId "+ entityId+" Point: "+position+" entityType: "+entityType+" state: "+state+" value: "+ value);
                if(entityType==myTeamId)
                {
                    if (friends.Count < bustersPerPlayer)
                    {
                        friends.Add(new Buster(position, entityId, "Friend"));
                        continue;
                    }

                    Buster buster = friends[entityId%bustersPerPlayer];
                    buster.Position = position;

                    bool isCarrying = (state == 0 ? false : true);
                    buster.IsCarrying = isCarrying;
                    if (isCarrying)
                    {
                        foreach (var ghost in ghosts)
                        {
                            if (ghost.Id ==value)
                            {
                                buster.GhostCarryied = ghost;
                                ghosts.Remove(ghost);
                                break;
                            }

                        }

                    }
                }
                else if (entityType == -1)
                {
                    Ghost ghost = new Ghost(position,entityId,value);
                    bool isInList = false;
                    foreach (var g in ghosts)
                    {
                        if (g.Equals(ghost))
                        {
                            isInList = true;
                            ghost = g;
                            break;
                        }
                    }
                    if(!isInList)
                        ghosts.Add(ghost);

                    ghost.Position = position;
                    ghost.NumberOfBusters = value;
                }
            }

            try
            {
                foreach (var ghost in ghosts)
                {

                    if (ghost.Timer == 0)
                        ghosts.Remove(ghost); //removing old ghosts
                    ghost.Timer--;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            foreach (var buster in friends)
            {

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                // MOVE x y | BUST id | RELEASE

                if(!buster.IsCarrying) //if buster if not carrying a ghost
                {
                    if (ghosts.Count == 0)
                    {
                        buster.Explore();
                    }
                    else
                    {
                        bool ghostNear = false;
                        foreach (var ghost in ghosts)
                        {
                            if (Point.Distance(buster.Position, ghost.Position) <= 2200)
                            {
                                ghostNear = true;
                                buster.Bust(ghost);
                                break;
                            }
                        }

                        if (!ghostNear)
                            buster.Explore();
                    }
                }
                else if (Point.Distance(buster.Position, bases[myTeamId]) <= 1600) //if on release distance
                {
                    buster.Release();
                }
                else //going to base
                {
                    buster.MoveTo(bases[myTeamId]);
                }
            }
        }
    }
}