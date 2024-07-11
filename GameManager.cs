using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FountainOfObjects
{
    internal  class GameManager
    {
        int _maxRows { get; set; } = 4;
        int _maxColumns { get; set; } = 4;
        Room[,] RoomGrid;
        Player player;
        Room fountainRoom;
        Room entrance;
        bool HasWon = false;
        

        public GameManager()
        {
            player = new Player();
        }


        public void InitializeGame()
        {
            Console.WriteLine(StartText());
            RoomSize();
            while (!HasWon && !player.Dead)
            {
                GetAction();
            }
        }

        public void RoomSize()
        {
            string readResult;
            Console.WriteLine("How big would you like the cave to be\n1. Small\n2. Medium\n3. Large");
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                readResult = readResult.ToLower();
                switch (readResult)
                {
                    case "small":
                        CreateRooms();
                        break;
                    case "medium":
                        CreateRooms(6, 6);
                        break;
                    case "large":
                        CreateRooms(8, 8);
                        break;
                    default:
                        CreateRooms();
                        break;

                }
            }
        }
        public void GetAction()
        {
            player.SetCurrentRoom(RoomGrid);
            CheckWinState();

            string readResult;
            bool legalMove = false;

            if (!HasWon && !player.Dead)
            {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"You are in the room at {player.GetCurrentRoom().ToString()} with {player.ArrowCount} arrow(s)");
                DisplayMessage();
                while (legalMove == false)
                {
                    readResult = Console.ReadLine();
                    if (readResult != null)
                    {
                        readResult = readResult.ToLower();
                        switch (readResult)
                        {
                            case "move north":
                                if (player.Row - 1 >= 0)
                                {
                                    player.Action(Actions.north);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move south":
                                if (player.Row + 1 <= RoomGrid.GetLength(0) - 1)
                                {
                                    player.Action(Actions.south);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move east":
                                if (player.Column + 1 <= RoomGrid.GetLength(1) - 1)
                                {
                                    player.Action(Actions.east);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move west":
                                if (player.Column - 1 >= 0)
                                {
                                    player.Action(Actions.west);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "enable fountain":
                                if (player.GetCurrentRoom() == fountainRoom && !fountainRoom.FountainEnabled())
                                {
                                    legalMove = true;
                                    fountainRoom.SetFountainStatus();
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "shoot north":
                                legalMove = true;
                                player.Action(Actions.shootNorth);
                                break;
                            case "shoot south":
                                legalMove = true;
                                player.Action(Actions.shootSouth);
                                break;
                            case "shoot east":
                                legalMove = true;
                                player.Action(Actions.shootEast);
                                break;
                            case "shoot west":
                                legalMove = true;
                                player.Action(Actions.shootWest);
                                break;
                            case "help":
                                legalMove = true;
                                Console.WriteLine(HelpText());
                                break;
                            default:
                                Console.WriteLine("Invalid input");
                                continue;

                        }
                    }

                }
            }



        }
        public Room[,] CreateRooms()
        {
            RoomGrid = new Room[_maxRows, _maxColumns];
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    RoomGrid[i, j] = new Room(i, j);
                }
            }
            SetRoomContents();
            return RoomGrid;
        }
        public Room[,] CreateRooms(int rows, int columns)
        {
            _maxRows = rows;
            _maxColumns = columns;
            RoomGrid = new Room[_maxRows, _maxColumns];
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    RoomGrid[i, j] = new Room(i, j);
                }
            }
            SetRoomContents();
            return RoomGrid;
        }
        public void DisplayMessage()
        {
            if (player.GetCurrentRoom() == entrance)
                Console.WriteLine("You see the light coming from the cavern entrance.");
            else if (player.GetCurrentRoom() == fountainRoom && !fountainRoom.FountainEnabled())
                Console.WriteLine("You hear the water dripping in this room. The Fountian of Objects is here!");
            else if (player.GetCurrentRoom() == fountainRoom && fountainRoom.FountainEnabled())
                Console.WriteLine("You hear the rushing waters from the Fountain of Objects. It has been reactivated!");
            else if (player.ThreatDetected(RoomGrid))
            {
                foreach (Contents threat in player.threatType)
                {
                    switch (threat)
                    {
                        case Contents.PitTrap:
                            Console.WriteLine("You feel a draft. There is a pit in a nearby room.");
                            break;
                        case Contents.Maelstrom:
                            Console.WriteLine("You hear the growling and groaning of a maelstrom nearby");
                            break;
                        case Contents.Amarok:
                            Console.WriteLine("You can smell the rotten stench of an amarok in a nearby room.");
                            break;
                    }
                }
            }
            else if (HasWon)
                Console.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life!\nYouWin!");
            Console.Write("What do you want to do? ");
        }

        public void DisplayRoomCoords()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    Console.Write(RoomGrid[i, j].ToString());
                }
                Console.WriteLine();
            }

        }
        private void CheckWinState()
        {
            if(player.Dead)
            {
                player.PlayerDied();
                PlayAgain();
            }
            if (fountainRoom.FountainEnabled() && player.GetCurrentRoom() == entrance)
            {
                HasWon = true;
            }
        }

        public void PlayAgain()
        {
            string readResult;
            Console.WriteLine("Would you like to play again? y/n");
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                readResult = readResult.ToLower();
                switch (readResult)
                {
                    case "y":
                        player.SetAlive();
                        player.SetCurrentRoom(RoomGrid);
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
        }
        public string HelpText() => @"Commands:
move north: move one row up
move south: move one row down
move east: move one column right
move west: move one column left
shoot north: shoot arrow one row up
shoot south: shoot arrow one row down
shoot east: shoot arrow one column right
shoot west: shoot arrow one column left
enable fountian: enables fountain if in room";
        public string StartText() => @"You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.
Light is visible only in the entrance, and no other light is seen anyhwere in the caverns.
You must navigate the Caverns with your other senses.
Find the Fountain of Objects, activate it, and return to the entrance.

Look out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will die.
Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns. You will be able to hear their growling and groaning in nearby rooms.
Amaroks roam the caverns. Encountering one is certain death, but you can smell their rotten stench in nearby rooms.
You carry with you a bow and a quiver of arrows. You can use them to shoot monsters in the caverns but be warned: you have a limited supply.
";

        public void SetRoomContents()
        {
            RoomGrid[0, 0].RoomContents = Contents.Entrance;
            RoomGrid[0, 2].RoomContents = Contents.Fountain;
            RoomGrid[1, 0].RoomContents = Contents.PitTrap;
            RoomGrid[0, 3].RoomContents = Contents.Maelstrom;
            RoomGrid[3, 3].RoomContents = Contents.Amarok;
            
            fountainRoom = RoomGrid[0, 2];
            entrance = RoomGrid[0, 0];
        }

    }
}
