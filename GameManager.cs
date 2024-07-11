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
            RoomSize();
            while (!HasWon)
            {
                GetAction();
            }
        }

        public void RoomSize()
        {
            string readResult;
            Console.WriteLine("How big would you like the cave to be\n\t1. Small\n\t2. Medium\n\t3. Large");
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

                }
            }
        }
        public void GetAction()
        {
            player.SetCurrentRoom(RoomGrid);
            CheckWinState();

                string readResult;
                bool legalMove = false;
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"You are in the room at {player.GetCurrentRoom().ToString()}");

                DisplayMessage();
            if (!HasWon)
            {
                while (legalMove == false)
                {
                    readResult = Console.ReadLine();
                    if (readResult != null)
                    {
                        readResult = readResult.ToLower();
                        switch (readResult)
                        {
                            case "move north":
                                if (player.Row() - 1 >= 0)
                                {
                                    player.Action(Actions.north);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move south":
                                if (player.Row() + 1 <= RoomGrid.GetLength(0))
                                {
                                    player.Action(Actions.south);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move east":
                                if (player.Column() + 1 <= RoomGrid.GetLength(1))
                                {
                                    player.Action(Actions.east);
                                    legalMove = true;
                                    break;
                                }
                                Console.WriteLine("Invalid Move");
                                continue;
                            case "move west":
                                if (player.Column() - 1 >= 0)
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
                switch (player.threatType)
                {
                    case Contents.PtTrap:
                        Console.WriteLine("You feel a draft. There is a pit in a nearby room.");
                        break;
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
            if (fountainRoom.FountainEnabled() && player.GetCurrentRoom() == entrance)
            {
                HasWon = true;
            }
        }

    
        public void SetRoomContents()
        {
            RoomGrid[0, 0].RoomContents = Contents.Entrance;
            RoomGrid[0, 2].RoomContents = Contents.Fountain;
            RoomGrid[1, 3].RoomContents = Contents.PtTrap;
            fountainRoom = RoomGrid[0, 2];
            entrance = RoomGrid[0, 0];
        }

    }
}
