using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROCJAM_2020.GameWorld
{
    public class LoadedMap
    {

        public List<Room> Rooms { get; private set; }
        public List<Rectangle> ClosedArea;
        public List<Rectangle> BlankedArea;
 
        Vector2 _roomCentreOrigin = new Vector2(0, 0);
        float _width = 400;
        float _height = 400;
        float _unloadRoom = 900f;
        public LoadedMap()
        {
            Rooms = new List<Room>();
            ClosedArea = new List<Rectangle>();
            BlankedArea = new List<Rectangle>();
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            addRoom(player);
        }

        float distanceBetween(Rectangle rect1, Rectangle rect2) =>
            calcEuclidean(rect1.Center.ToVector2(), rect2.Center.ToVector2());

        float calcEuclidean(Vector2 point1, Vector2 point2) =>
            (float)Math.Sqrt(((point1.X - point2.X) * (point1.X - point2.X)) + ((point1.Y - point2.Y) * (point1.Y - point2.Y)));
    
        
        public void addRoom(Rectangle player)
        {
            bool roomsUpdated = false;
            //remove distant rooms
            for (int i = 0; i < Rooms.Count; i++)
                if (calcEuclidean(Rooms[i].Location, player.Location.ToVector2()) > _unloadRoom)
                {
                    Rooms[i].IsRemoved = true;
                    Rooms.RemoveAt(i--);
                    roomsUpdated = true;
                }

            Room inRoom = default;
            foreach(var room in Rooms)
            {
                if(player.Intersects(room.Rectangle))
                {
                    room.Discovered = true;
                    inRoom = room;
                    roomsUpdated = true;
                }
            }
            Vector2 loadedTopLeft;
            if (inRoom == default)
            {
                Vector2 topLeftOrigin = new Vector2(_roomCentreOrigin.X - (_width / 2), _roomCentreOrigin.Y - (_height / 2));
                loadedTopLeft = new Vector2(topLeftOrigin.X - (_width * 2), topLeftOrigin.Y - (_height * 2));
            }
            else
            {
                inRoom.Discovered = true;
                loadedTopLeft = new Vector2(inRoom.Rectangle.X - (_width * 2), inRoom.Rectangle.Y - (_height * 2));
            }
            List<Vector2> unfilledLocations = new List<Vector2>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vector2 locationCheck = new Vector2(loadedTopLeft.X + (_width * i), loadedTopLeft.Y + (_height * j));
                    bool locationFound = false;
                    foreach(var room in Rooms)
                    {
                        if (room.Rectangle.Location.ToVector2() == locationCheck)
                            locationFound = true;
                    }
                    if(!locationFound)
                    {
                        unfilledLocations.Add(locationCheck);
                    }
                }
            }
            foreach(var location in unfilledLocations)
            {
                Rooms.Add(new Room(new Rectangle((int)location.X, (int)location.Y, (int)_width, (int)_height), Rooms));
                roomsUpdated = true;
            }

            if(roomsUpdated)
                updateColliders();
        }

        void updateColliders()
        {
            ClosedArea.Clear();
            BlankedArea.Clear();
            foreach (var room in Rooms)
            {
                foreach (var rect in room.Colliders)
                    ClosedArea.Add(rect);
                if (!room.Discovered)
                    BlankedArea.Add(room.Rectangle);
            }
        }
    }
}
