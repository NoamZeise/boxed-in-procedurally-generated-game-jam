using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROCJAM_2020.Components.Sprites
{
    public abstract class Sprite : Component
    {
        protected Vector2 Velocity;
        float _speed = 100;
        public int Health = 1;

        double invincibilityTimer = 0.4;
        double flashTimer = 0;
        static double FLASH_DELAY = 0.1;
        static double INVINCIBILITY_DELAY = 0.4;
        static double PUSHBACK_DELAY = 0.15;
        bool damaged = true;
        protected bool collided = false;
        Vector2 pushBack = Vector2.Zero;
        Color damageColour = Color.Red;
        Color defaultColour;
        public Sprite(Vector2 position, Texture2D texture) : base(position, texture)
        {



        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
                Velocity = Vector2.Zero;
                if (Health <= 0)
                    IsRemoved = true;
                if(colour != damageColour)
            {
                defaultColour = colour;
            }


                Controls();
            collided = false;
                if (invincibilityTimer < PUSHBACK_DELAY)
                {
                    Velocity += pushBack;
                    pushBack.X *= 0.90f;
                    pushBack.Y *= 0.90f;
                }
                else if (!damaged)
                {
                    Health--;
                    damaged = true;
                }
                if(invincibilityTimer < INVINCIBILITY_DELAY)
            {
                if (flashTimer < FLASH_DELAY)
                {
                    flashTimer = 0;
                    if (colour == defaultColour)
                        colour = damageColour;
                    else
                        colour = defaultColour;
                }
            }
                else if(colour == damageColour)
            {
                colour = defaultColour;
            }
            List<Rectangle> colliders = new List<Rectangle>();
            foreach (var collider in paths)
                colliders.Add(collider);
                foreach(var component in components)
                {
                if (component is Player)
                    continue;
                if (this is Player)
                    continue;
                    if(component is Sprite)
                    {
                    if (component != this)
                        colliders.Add(component.Rectangle);
                    }
                }

            CollisionChecking(gameTime, colliders);
                if (invincibilityTimer <= INVINCIBILITY_DELAY)
                {
                    invincibilityTimer += gameTime.ElapsedGameTime.TotalSeconds;

                }
        }


        protected abstract void Controls();

        private void CollisionChecking(GameTime gameTime, List<Rectangle> colliders)
        {
            Vector2 previousPosition = Position;

            Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Colliding(colliders))
            {
                Ycolliding(gameTime, colliders, previousPosition);
                collided = true;
            }

            Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Colliding(colliders))
            {
                Xcolliding(gameTime, colliders, previousPosition);
                collided = true;
            }
        }

        private bool Colliding(List<Rectangle> colliders)
        {
            foreach (var collider in colliders)
            {
                if (Rectangle.Intersects(collider))
                {
                    return true;
                }
            }
            return false;
        }

        private void Ycolliding(GameTime gameTime, List<Rectangle> colliders, Vector2 previousPosition)
        {
            if (Velocity.Y > 0) //going down
            {
                while (Velocity.Y > 0 && Colliding(colliders))
                {
                    Position.Y = previousPosition.Y;
                    Velocity.Y--;
                    Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (Velocity.Y < 0)
            {
                while (Velocity.Y < 0 && Colliding(colliders))
                {
                    Position.Y = previousPosition.Y;
                    Velocity.Y++;
                    Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
        private void Xcolliding(GameTime gameTime, List<Rectangle> colliders, Vector2 previousPosition)
        {
            if (Velocity.X > 0) //going right
            {
                while (Velocity.X > 0 && Colliding(colliders))
                {
                    Position.X = previousPosition.X;
                    Velocity.X--;
                    Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (Velocity.X < 0)
            {
                while (Velocity.X < 0 && Colliding(colliders))
                {
                    Position.X = previousPosition.X;
                    Velocity.X++;
                    Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void Damage(Vector2 position)
        {

            if (invincibilityTimer > INVINCIBILITY_DELAY)
            {
                invincibilityTimer = 0;
                damaged = false;
            }
            float xDiff = this.Position.X - position.X;
            float yDiff = this.Position.Y - position.Y;
            if (Math.Abs(xDiff) > Math.Abs(yDiff)) //find attack direction
            {
                if (xDiff > 0) //left
                {
                    pushBack.X = 800;
                }
                if (xDiff < 0)//right
                {
                    pushBack.X = -800;
                }
            }
            else
            {
                if (yDiff > 0)//up
                {
                    pushBack.Y = 800;
                }
                if (yDiff < 0) //down
                {
                    pushBack.Y = -800;
                }
            }
        
    }
    }
}
