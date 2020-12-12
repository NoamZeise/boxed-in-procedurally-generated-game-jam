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
    class Mover : Enemy
    {
        float _speed = 250f;
        static Random rand = new Random();
        Vector2 previousPosition;
        Vector2 currentVelocity;
        public Mover(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            setRandomVelocity();
            previousPosition = Rectangle.Location.ToVector2();
            colour = Color.Yellow;
        }

        private void setRandomVelocity()
        {
            if (rand.Next(2) == 0)
            {
                if (rand.Next(2) == 0)
                    Velocity = new Vector2(_speed, 0);
                else
                    Velocity = new Vector2(-_speed, 0);
            }
            else
            {
                if (rand.Next(2) == 0)
                    Velocity = new Vector2(0, _speed);
                else
                    Velocity = new Vector2(0, -_speed);
            }
            currentVelocity = Velocity;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            if (Parent.Discovered)
            {
                foreach (var comp in components)
                {
                    if (comp is Player)
                    {
                        if (Parent.IsRemoved)
                        {
                            if (calcEuclidean(destination, Position) > 900f)
                                this.IsRemoved = true;
                        }
                        else
                        {
                            if (this.Rectangle.Intersects(comp.Rectangle))
                                (comp as Sprite).Damage(this.Position);
                        }
                    }
                }
                base.Update(gameTime, components, paths);
                previousPosition = Position;
            }
            if (Parent.IsRemoved && !Parent.Discovered)
                this.IsRemoved = true;
        }

        protected override void Controls()
        {
            Velocity = currentVelocity;
            if (collided)
                setRandomVelocity();

        }
    }
}
