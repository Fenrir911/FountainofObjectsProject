using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainOfObjects
{
    internal class Player
    {
        int _row;
        int _column;
        public (int row, int column) Coordinates;
        Room currentRoom;
        private bool _threatDetected = false;
        public Contents threatType = Contents.Empty;

        public Player()
        {
            _row = 0;
            _column = 0;
            Coordinates = (_row, _column);
        }

        public int Row() { return _row; }
        public int Column() { return _column; }
        public void CheckAdjacentRooms(Room[,] rooms)
        {
            int index = 0;
            Room[]? adjacentRooms = new Room[8];
            Contents[] contents = new Contents[8];
            foreach (Room room in rooms)
            {
                if ((room.Row == _row + 1 && room.Column == _column) || (room.Row == _row - 1 && room.Column == _column) || (room.Column == _column + 1 && room.Row == _row) || (room.Column == _column - 1 && room.Row == _row) ||
                    (room.Row == _row - 1 && room.Column == _column - 1) || (room.Row == _row - 1 && room.Column == _column + 1) ||(room.Row == _row + 1 && room.Column == _column - 1) || (room.Row == _row + 1 && room.Column == _column + 1))
                { 
                    index++;
                    adjacentRooms[index] = room; 
                }
            }
            for (int i = 0; i < adjacentRooms.Length - 1; i++)
            {
                if (adjacentRooms[i] != null)
                    {
                    if (adjacentRooms[i].RoomContents == Contents.PtTrap)
                    {
                        threatType = adjacentRooms[i].RoomContents;
                        _threatDetected = true;
                        return;
                    }
                }
            }
             _threatDetected = false;
            return;
        }
        public bool ThreatDetected(Room[ , ] rooms)
        {
            CheckAdjacentRooms(rooms);
            return _threatDetected;
        }
        public void SetCurrentRoom(Room[,] rooms)
        {
            Coordinates = (_row, _column);
            foreach (Room room in rooms)
            {
                if (room.Coordinates == Coordinates)
                    currentRoom = room;
            }
        }
        public Room GetCurrentRoom()
        {
            return currentRoom;
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
            }
        }

    }
} enum Actions { north, south, east, west, enable}
