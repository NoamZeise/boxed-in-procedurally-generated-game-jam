using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PROCJAM_2020.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROCJAM_2020.Components.Sprites.Enemies
{
    public abstract class Enemy : Sprite
    {
        protected Vector2 destination = default;
        public Room Parent;
        float speed = 100f;
        public Enemy(Vector2 position, Texture2D texture, Room parent) : base(position, texture)
        {
            Health = 2;
            Parent = parent;
        }

       

        protected override void Controls()
        {
          
            if (destination != default)
            {
                float xDiff = destination.X - Position.X;
                float yDiff = destination.Y - Position.Y;
                if (Math.Abs(xDiff) > Math.Abs(yDiff))
                {
                    float scale = Math.Abs(yDiff) / Math.Abs(xDiff);
                    Velocity.X = speed;
                    Velocity.Y = speed * scale;
                }
                if (Math.Abs(xDiff) < Math.Abs(yDiff))
                {
                    float scale = Math.Abs(xDiff) /  Math.Abs(yDiff);
                    Velocity.Y = speed;
                    Velocity.X = speed * scale;
                }
                if (xDiff < 0)
                    Velocity.X *= -1;
                if (yDiff < 0)
                    Velocity.Y *= -1;
            }
        }

        protected float calcEuclidean(Vector2 point1, Vector2 point2) =>
           (float)Math.Sqrt(((point1.X - point2.X) * (point1.X - point2.X)) + ((point1.Y - point2.Y) * (point1.Y - point2.Y)));


    }
}
