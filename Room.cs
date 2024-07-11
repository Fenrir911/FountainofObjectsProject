using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainOfObjects
{
    internal class Room
    {
        private int _row;
        private int _column;
        public (int, int) Coordinates;
        public Contents RoomContents = Contents.Empty;
        private bool _fountainEnabled = false;

        public Room( int row, int column)
        {
            _row = row;
            _column = column;
            Coordinates = (_row, _column);
        }
        public int Row { get { return _row; } }
        public int Column { get { return _column; } }
        public override string ToString()
        {
            return Coordinates.ToString() + RoomContents;
        }
        public void SetRoomContents(Contents content)
        {
            RoomContents = content;
        }
        public bool FountainEnabled()
        {
            if (RoomContents == Contents.Fountain)
                return _fountainEnabled;
         return _fountainEnabled;
        }
        public void SetFountainStatus()
        {
            if (RoomContents == Contents.Fountain)
                {
                _fountainEnabled = true;
                }
        }
        public void MaelstromEffect(Room[ , ] rooms)
        {
            this.RoomContents = Contents.Empty;
            (int row, int column) newCoordinates = (_row + 1, _column - 2);
            if (newCoordinates.row > rooms.GetLength(0) - 1)
                newCoordinates.row = rooms.GetLength(0) - 1;
            if (newCoordinates.column <= 0)
                newCoordinates.column = 0;
             for(int i = 0; i < rooms.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <  rooms.GetLength(1) - 1; j++)
                {
                    if(rooms[i, j].Coordinates == newCoordinates)
                        {
                        rooms[i, j].RoomContents = Contents.Maelstrom;
                        break;
                    }
                }
            }
            }
        }
        
    }
    
enum Contents { Empty, Entrance, Fountain, PitTrap, Maelstrom, Amarok }