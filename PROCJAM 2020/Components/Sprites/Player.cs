using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PROCJAM_2020.Components.Sprites.Enemies;
using System.Collections.Generic;

namespace PROCJAM_2020.Components.Sprites
{
    public class Player : Sprite
    {
        enum DIRECTION
        {
            NORTH,
            SOUTH,
            EAST,
            WEST
        }
        private KeyboardState previousState;
        float _speed = 150;
        double AttackTimer = 0.4;
        double AttackDuration = 0.1;
        double AttackDelay = 0.4;
        bool Attacking = false;
        Rectangle attackingRect;
        DIRECTION direction = DIRECTION.NORTH;

        int attackingWidth = 50;
        int attackingHeight = -10;
        public Player(Vector2 position, Texture2D texture) : base(position, texture)
        {
            Health = 3;
            colour = new Color(34, 177, 76);
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            base.Update(gameTime, components, paths);

            if(Attacking)
            {
                switch (direction)
                {
                    case DIRECTION.NORTH:
                        attackingRect = new Rectangle((int)Position.X - attackingHeight, (int)Position.Y - attackingWidth, _texture.Height + (attackingHeight * 2), attackingWidth);
                        break;
                    case DIRECTION.SOUTH:
                        attackingRect = new Rectangle((int)Position.X - attackingHeight, (int)Position.Y + _texture.Height, _texture.Height + (attackingHeight * 2), attackingWidth);
                        break;
                    case DIRECTION.WEST:
                        attackingRect = new Rectangle((int)Position.X - attackingWidth, (int)Position.Y - attackingHeight, attackingWidth, _texture.Width + (attackingHeight * 2));
                        break;
                    case DIRECTION.EAST:
                        attackingRect = new Rectangle((int)Position.X + _texture.Width, (int)Position.Y - attackingHeight, attackingWidth, _texture.Width + (attackingHeight * 2));
                        break;
                }
                foreach (var comp in components)
                {
                    if(comp is Enemy)
                    {
                        if(comp.Rectangle.Intersects(attackingRect))
                        {
                            (comp as Enemy).Damage(this.Position);
                        }
                    }
                    if(comp is Projectile)
                    {
                        if ((comp as Projectile).Parent != this)
                        if ((comp as Projectile).Active)
                            if (comp.Rectangle.Intersects(attackingRect))
                                (comp as Projectile).Reverse(this);
                        //(comp as Projectile).IsRemoved = true;
                    }
                }
            }
           
            previousState = Keyboard.GetState();
            AttackTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Controls()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Velocity.Y -= _speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Velocity.Y += _speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Velocity.X -= _speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Velocity.X += _speed;
            }
            if (!Attacking)
            {
                if (Velocity.Y < 0)
                {
                    direction = DIRECTION.NORTH;
                }
                if (Velocity.Y > 0)
                {
                    direction = DIRECTION.SOUTH;
                }
                if (Velocity.X < 0)
                {
                    direction = DIRECTION.WEST;
                }
                if (Velocity.X > 0)
                {
                    direction = DIRECTION.EAST;
                }
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Z) && previousState.IsKeyUp(Keys.Z) && AttackTimer > AttackDelay)
            {
                AttackTimer = 0;
                Attacking = true;
            }
            if (AttackTimer > AttackDuration)
                Attacking = false;
        }

        private void CollisionChecking(GameTime gameTime, List<Rectangle> colliders)
        {
            Vector2 previousPosition = Position;

            Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Colliding(colliders))
            {
                Ycolliding(gameTime, colliders, previousPosition);
            }
            
            Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Colliding(colliders))
            {
                Xcolliding(gameTime, colliders, previousPosition);
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

        private void Attack()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Attacking)
            {
                spriteBatch.Draw(_texture, attackingRect, null, new Color(100, 100, 100), 0f, Vector2.Zero, SpriteEffects.None, 0.5f );
            }
            else if(AttackTimer < AttackDelay)
            {
                float width = _texture.Width / 10;
                Color tempColour = colour;
                for (int i = 0; i < 10; i++)
                {
                    spriteBatch.Draw(_texture, new Rectangle((int)Position.X + (int)(i * width), (int)Position.Y + (int)_texture.Width, (int)width, (int)width), null, tempColour, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
                    if(((AttackTimer - AttackDuration) / AttackDelay) * 15 < i)
                        tempColour = Color.Red;
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
