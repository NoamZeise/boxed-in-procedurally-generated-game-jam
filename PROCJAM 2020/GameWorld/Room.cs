using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PROCJAM_2020.GameWorld
{
    public class Room
    {
        public Vector2 Location;
        public Rectangle Rectangle;

        public List<Rectangle> Colliders;
        public List<Vector2> Follow;
        public List<Vector2> Projectile;
        public List<Vector2> Mover;
        public List<Vector2> Quad;
        public List<Vector2> Repel;

        public bool Discovered = false;
        public bool IsRemoved = false;
        public bool EnemiesSpawned = false;
        public bool Scored = false;

        bool Top;
        bool Bottom;
        bool Left;
        bool Right;
        static Random rand = new Random();

        public Room(Rectangle roomRect, List<Room> rooms)
        {
            Follow = new List<Vector2>();
            Projectile = new List<Vector2>();
            Mover = new List<Vector2>();
            Quad = new List<Vector2>();
            Repel = new List<Vector2>();
            Location = new Vector2(roomRect.X + (roomRect.Width / 2), roomRect.Y + (roomRect.Height / 2));
            Rectangle = roomRect;
            Colliders = new List<Rectangle>();
            SetWalls(roomRect);
            SetExits(rooms);
            if (Location != Vector2.Zero)
                LoadRoomFromFile();
        }

        private void LoadRoomFromFile()
        {
            int RotationOrder = rand.Next(0, 4);
            StreamReader sr = new StreamReader("layouts/" + rand.Next(1, 21) + ".tmx");
            XmlReader xmlR = XmlReader.Create(sr);
            while (xmlR.Read())
            {
                if (xmlR.Name == "objectgroup")
                {
                    if (xmlR.GetAttribute("name") == "obstacles" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.AttributeCount > 3)
                            {
                                var rect = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("width")),
                                    (int)Convert.ToDouble(xmlR.GetAttribute("height"))));

                                rect = RotatedRect(RotationOrder, rect); 
                                Colliders.Add(rect);
                            }
                            xmlR.Read();
                        }
                    }
                    if (xmlR.GetAttribute("name") == "projectile" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.Name == "object" && xmlR.AttributeCount == 3)
                            {
                                var place = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    40,
                                    40));
                                place = RotatedRect(RotationOrder, place);
                                Projectile.Add(new Vector2(place.X, place.Y));
                            }
                            xmlR.Read();
                        }
                    }
                    if (xmlR.GetAttribute("name") == "follow" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.Name == "object" && xmlR.AttributeCount == 3)
                            {
                                var place = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    40,
                                    40));
                                place = RotatedRect(RotationOrder, place);
                                Follow.Add(new Vector2(place.X, place.Y));
                            }
                            xmlR.Read();
                        }
                    }
                    if (xmlR.GetAttribute("name") == "mover" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.Name == "object" && xmlR.AttributeCount == 3)
                            {
                                var place = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    40,
                                    40));
                                place = RotatedRect(RotationOrder, place);
                                Mover.Add(new Vector2(place.X, place.Y));
                            }
                            xmlR.Read();
                        }
                    }
                    if (xmlR.GetAttribute("name") == "quad" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.Name == "object" && xmlR.AttributeCount == 3)
                            {
                                var place = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    40,
                                    40));
                                place = RotatedRect(RotationOrder, place);
                                Quad.Add(new Vector2(place.X, place.Y));
                            }
                            xmlR.Read();
                        }
                    }
                    if (xmlR.GetAttribute("name") == "repel" && !xmlR.IsEmptyElement)
                    {
                        xmlR.Read();
                        while (xmlR.Name != "objectgroup")
                        {
                            if (xmlR.Name == "object" && xmlR.AttributeCount == 3)
                            {
                                var place = (new Rectangle(
                                    (int)Convert.ToDouble(xmlR.GetAttribute("x")) - 180,
                                    (int)Convert.ToDouble(xmlR.GetAttribute("y")) - 180,
                                    40,
                                    40));
                                place = RotatedRect(RotationOrder, place);
                                Repel.Add(new Vector2(place.X, place.Y));
                            }
                            xmlR.Read();
                        }
                    }
                }
            }
        }

        private Rectangle RotatedRect(int RotationOrder, Rectangle rect)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(rect.X, rect.Y));
            points.Add(new Point(rect.X + rect.Width, rect.Y));
            points.Add(new Point(rect.X, rect.Y + rect.Height));
            points.Add(new Point(rect.X + rect.Width, rect.Y + rect.Height));

            int smallestX = default;
            int smallestY = default;
            int largestX = default;
            int largestY = default;
            for (int i = 0; i < points.Count; i++)
            {
                switch (RotationOrder)
                {
                    case 0:
                        break;
                    case 1:
                        points[i] = new Point(-points[i].Y, points[i].X);
                        break;
                    case 2:
                        points[i] = new Point(-points[i].X, -points[i].Y);
                        break;
                    case 3:
                        points[i] = new Point(points[i].Y, -points[i].X);
                        break;
                }
                //get largest and smallest x, y values
                if (smallestX == default)
                    smallestX = points[i].X;
                if (largestX == default)
                    largestX = points[i].X;
                if (smallestY == default)
                    smallestY = points[i].Y;
                if (largestY == default)
                    largestY = points[i].Y;

                if (points[i].X < smallestX)
                    smallestX = points[i].X;
                if (points[i].X > largestX)
                    largestX = points[i].X;
                if (points[i].Y < smallestY)
                    smallestY = points[i].Y;
                if (points[i].Y > largestY)
                    largestY = points[i].Y;

            }

            //set rectangle dimentions + location to rect coords
            rect = new Rectangle((int)(smallestX + 180 + Rectangle.X + 20), (int)(smallestY + 180 + Rectangle.Y + 20), largestX - smallestX, largestY - smallestY);
            return rect;
        }
        private void SetExits(List<Room> rooms)
        {
                Room top = default;
                Room bottom = default;
                Room left = default;
                Room right = default;
                foreach (var room in rooms)
                {
                    if (room.Rectangle.Location == new Point(this.Rectangle.Location.X, this.Rectangle.Location.Y - 400))
                    { //top
                        top = room;
                    }
                    if (room.Rectangle.Location == new Point(this.Rectangle.Location.X, this.Rectangle.Location.Y + 400))
                    { //bottom
                        bottom = room;
                    }
                    if (room.Rectangle.Location == new Point(this.Rectangle.Location.X, this.Rectangle.Location.Y - 400))
                    { //left
                        left = room;
                    }
                    if (room.Rectangle.Location == new Point(this.Rectangle.Location.X, this.Rectangle.Location.Y + 400))
                    { //right
                        right = room;
                    }
                }
                int numExits = rand.Next(2, 4);
                int exitsPlaced = 0;
                List<int> sidesChecked = new List<int>();
                while (exitsPlaced < numExits)
                {
                    int exit = rand.Next(4);

                    switch (exit)
                    {
                        case 0://top
                            if (sidesChecked.Contains(0))
                                break;
                            sidesChecked.Add(0);
                            if (top != default)
                            {
                                if (top.Bottom)
                                {
                                    exitsPlaced++;
                                    Top = true;
                                }
                                else
                                {
                                    Colliders.Add(new Rectangle(Rectangle.X, Rectangle.Y - 20, 400, 40));
                                }
                            }
                            else
                            {
                                exitsPlaced++;
                                Top = true;
                            }
                            break;
                        case 1://bottom
                            if (sidesChecked.Contains(1))
                                break;
                            sidesChecked.Add(1);
                            if (bottom != default)
                            {
                                if (bottom.Top)
                                {
                                    exitsPlaced++;
                                    Bottom = true;
                                }
                                else
                                {
                                    Colliders.Add(new Rectangle(Rectangle.X, Rectangle.Y + 380, 400, 40));
                                }
                            }
                            else
                            {
                                exitsPlaced++;
                                Bottom = true;
                            }
                            break;
                        case 2://left
                            if (sidesChecked.Contains(2))
                                break;
                            sidesChecked.Add(2);
                            if (left != default)
                            {
                                if (left.Right)
                                {
                                    exitsPlaced++;
                                    Left = true;
                                }
                                else
                                {
                                    Colliders.Add(new Rectangle(Rectangle.X - 20, Rectangle.Y, 40, 400));
                                }
                            }
                            else
                            {
                                exitsPlaced++;
                                Left = true;
                            }
                            break;
                        case 3://right
                            if (sidesChecked.Contains(3))
                                break;
                            sidesChecked.Add(3);
                            if (right != default)
                            {
                                if (right.Left)
                                {
                                    exitsPlaced++;
                                    Right = true;
                                }
                                else
                                {
                                    Colliders.Add(new Rectangle(Rectangle.X + 380, Rectangle.Y, 40, 400));
                                }
                            }
                            else
                            {
                                exitsPlaced++;
                                Right = true;
                            }
                            break;
                    }
                    if (sidesChecked.Count > 3)
                        break;
                }


            
        }

        private void SetWalls(Rectangle roomRect)
        {
            Colliders.Add(new Rectangle(roomRect.Location.X, roomRect.Location.Y + 380, 160, 20));
            Colliders.Add(new Rectangle(roomRect.Location.X + 240, roomRect.Location.Y + 380, 160, 20));

            Colliders.Add(new Rectangle(roomRect.Location.X, roomRect.Location.Y, 160, 20));
            Colliders.Add(new Rectangle(roomRect.Location.X + 240, roomRect.Location.Y, 160, 20));

            Colliders.Add(new Rectangle(roomRect.Location.X, roomRect.Location.Y, 20, 160));
            Colliders.Add(new Rectangle(roomRect.Location.X, roomRect.Location.Y + 240, 20, 160));

            Colliders.Add(new Rectangle(roomRect.Location.X + 380, roomRect.Location.Y, 20, 160));
            Colliders.Add(new Rectangle(roomRect.Location.X + 380, roomRect.Location.Y + 240, 20, 160));
        }

    }
}
