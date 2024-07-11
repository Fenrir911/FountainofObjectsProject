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
    }
    
}
enum Contents { Empty, Entrance, Fountain, PtTrap }