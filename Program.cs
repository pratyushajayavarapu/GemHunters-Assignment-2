//GEM HUNTERS ASSIGNMENT 2
using System;
using System.Linq;

//Main entry of the program
class Program
{
    //start of the game
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}
//Represents the position 
class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}
//Represents a player
class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; }
    //contructor to initialize the game
    public Player(string name, Position startPosition)
    {
        Name = name;
        Position = startPosition;
        GemCount = 0;
    }
    // Represents the direction 
    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': Position.Y--; break;
            case 'D': Position.Y++; break;
            case 'L': Position.X--; break;
            case 'R': Position.X++; break;
        }
    }
}

class Cell
{
    public string Occupant { get; set; }
    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

class Board
{
    public Cell[,] Grid { get; set; } = new Cell[6, 6];

    public Board()
    {
        // Initialize all cells as empty
        for (int i = 0; i < 6; i++)
            for (int j = 0; j < 6; j++)
                Grid[i, j] = new Cell("-");

        // Place players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Randomly place gems and obstacles (for simplicity, we're placing 5 gems and 5 obstacles)
        Random rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            int gemX, gemY, obsX, obsY;
            do
            {
                gemX = rand.Next(6);
                gemY = rand.Next(6);
            } while (Grid[gemX, gemY].Occupant != "-");
            Grid[gemX, gemY].Occupant = "G";

            do
            {
                obsX = rand.Next(6);
                obsY = rand.Next(6);
            } while (Grid[obsX, obsY].Occupant != "-");
            Grid[obsX, obsY].Occupant = "O";
        }
    }
    // Method to display the current state of the board
    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
                Console.Write(Grid[i, j].Occupant + " ");
            Console.WriteLine();
        }
    }
    // Method to check if a move is valid for a given player in the specified direction
    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U': newY--; break;
            case 'D': newY++; break;
            case 'L': newX--; break;
            case 'R': newX++; break;
            default: return false;
        }

        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6) return false;  // Check bounds
        if (Grid[newX, newY].Occupant == "O" || Grid[newX, newY].Occupant == "P1" || Grid[newX, newY].Occupant == "P2") return false;  // Check obstacles and other players

        return true;
    }
    // Method to collect a gem from the current player's position
    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }
}

class Game
{
    public Board Board { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public Player CurrentTurn { get; set; }
    public int TotalTurns { get; set; } = 0;

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
    }

    public void Start()
    {
        Console.WriteLine("Welcome to Gem Hunters!");
        while (!IsGameOver())
        {
            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn. Please enter direction (U, D, L, R):");
            char direction;
            do
            {
                direction = Char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
            } while (!new char[] { 'U', 'D', 'L', 'R' }.Contains(direction) || !Board.IsValidMove(CurrentTurn, direction));

            CurrentTurn.Move(direction);
            Board.CollectGem(CurrentTurn);
            TotalTurns++;
            SwitchTurn();
        }
        Board.Display();
        AnnounceWinner();
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns == 30;
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount) Console.WriteLine("Player 1 Wins the game!");
        else if (Player2.GemCount > Player1.GemCount) Console.WriteLine("Player 2 Wins the game!");
        else Console.WriteLine("It's a tie!");
    }
}
  
    