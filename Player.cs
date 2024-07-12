using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FountainOfObjects
{
    internal class Player
    {
        int _row;
        int _column;
        public (int row, int column) Coordinates;
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public int ArrowCount = 5;
        private bool _threatDetected = false;
        private bool _alive = true;
        public bool Dead { get => !_alive; }
        public Room[,] roomGrid;
        public Contents[] threatType;
        Room currentRoom;
        Room enemyRoom;
        public Player(Room[,] rooms)
        {
            _row = 0;
            _column = 0;
            Coordinates = (_row, _column);
            roomGrid = rooms;
        }
        public Room GetCurrentRoom()
        {
            return currentRoom;
        }
        public void SetCurrentRoom()
        {
            Coordinates = (_row, _column);
            foreach (Room room in roomGrid)
            {
                if (room.Coordinates == Coordinates)
                    currentRoom = room;
            }
            if (currentRoom.RoomContents == Contents.PitTrap)
            {
                Console.WriteLine("You fell into a pit.");
                _alive = false;
            }
            else if (currentRoom.RoomContents == Contents.Amarok)
            {
                Console.WriteLine("The amarok had you for lunch.");
                _alive = false;
            }
            else if (currentRoom.RoomContents == Contents.Maelstrom)
            {
                Console.WriteLine($"You have been wooshed away by a Maelstrom. Your coordinates were {Coordinates.ToString()}.");
                currentRoom.MaelstromEffect(roomGrid);
                _row -= 1; _column += 2;
                if (_row <= 0) _row = 0;
                if (_column >= roomGrid.GetLength(1) - 1) _column = roomGrid.GetLength(1) - 1;
                Coordinates = (_row, _column);

                foreach (Room room in roomGrid)
                {
                    if (room.Coordinates == Coordinates)
                    {
                        currentRoom = room;
                    }
                }
            }
        }

        public void CheckAdjacentRooms()
        {
            int index = 0;
            int roomIndex = 0;
            Room[]? adjacentRooms = new Room[9];
            threatType = new Contents[3];
            foreach (Room room in roomGrid)
            {
                if ((room.Row == _row + 1 && room.Column == _column) || (room.Row == _row - 1 && room.Column == _column) || (room.Column == _column + 1 && room.Row == _row) || (room.Column == _column - 1 && room.Row == _row) ||
                    (room.Row == _row - 1 && room.Column == _column - 1) || (room.Row == _row - 1 && room.Column == _column + 1) ||(room.Row == _row + 1 && room.Column == _column - 1) || (room.Row == _row + 1 && room.Column == _column + 1))
                { 
                    roomIndex++;
                    adjacentRooms[roomIndex] = room; 
                }
            }
            for (int i = 0; i < adjacentRooms.Length - 1; i++)
            {
                if ( adjacentRooms[i] != null)
                    {
                        if (adjacentRooms[i].RoomContents == Contents.PitTrap || adjacentRooms[i].RoomContents == Contents.Maelstrom || adjacentRooms[i].RoomContents == Contents.Amarok)
                        {
                        threatType[index] = adjacentRooms[i].RoomContents;
                        if(adjacentRooms[i].RoomContents == Contents.Maelstrom || adjacentRooms[i].RoomContents == Contents.Amarok)
                            { enemyRoom = adjacentRooms[i]; }
                        index++;
                        }
                    }
            }
            if (threatType != null)
            {
                _threatDetected = true;
                return;
            }    
             _threatDetected = false;
            return;
        }
       
        public bool ThreatDetected()
        {
            CheckAdjacentRooms();
            return _threatDetected;
        }

        public void Action(Actions action)
        {
            
            switch (action)
            {
                case Actions.north:
                    _row--;
                    break;
                case Actions.south:
                    _row++;
                    break;
                case Actions.east:
                    _column++;
                    break;
                case Actions.west:
                    _column--;
                    break;
                case Actions.enable:
                    if (currentRoom.RoomContents == Contents.Fountain && !currentRoom.FountainEnabled())
                    {
                        currentRoom.SetFountainStatus();
                    }
                    break;
                case Actions.shootNorth:
                    if(enemyRoom.Coordinates == (Coordinates.row - 1, Coordinates.column))
                        {
                        Console.WriteLine("You have struck a beast");
                        enemyRoom.RoomContents = Contents.Empty;
                        }
                    ArrowCount--;
                    break;
                case Actions.shootSouth:
                    if (enemyRoom.Coordinates == (Coordinates.row + 1, Coordinates.column))
                    {
                        Console.WriteLine("You have struck a beast");
                        enemyRoom.RoomContents = Contents.Empty;
                    }
                    ArrowCount--;
                    break;
                case Actions.shootEast:
                    if (enemyRoom.Coordinates == (Coordinates.row, Coordinates.column + 1))
                    {
                        Console.WriteLine("You have struck a beast");
                        enemyRoom.RoomContents = Contents.Empty;
                    }
                    ArrowCount--;
                    break;
                case Actions.shootWest:
                    if (enemyRoom.Coordinates == (Coordinates.row, Coordinates.column - 1))
                    {
                        Console.WriteLine("You have struck a beast");
                        enemyRoom.RoomContents = Contents.Empty;
                    }
                    ArrowCount--;
                    break;
            }
        }
            public void SetAlive()
        {
            _row = 0; 
            _column = 0;
            _alive = true;
        }
        public void PlayerDied()
        {
            if (Dead)
            {
                Console.WriteLine("\nYou have Died. Better luck next time");
            }
        }
        

    }
} enum Actions { north, south, east, west, enable, shootNorth, shootSouth, shootEast, shootWest }
