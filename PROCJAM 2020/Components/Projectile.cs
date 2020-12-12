using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PROCJAM_2020.Components.Sprites;
using PROCJAM_2020.Components.Sprites.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROCJAM_2020.Components
{
    class Projectile : Component, ICloneable 
    {
        Vector2 Velocity = Vector2.Zero;
        public Component Parent;
        public bool Active = false;
        public Projectile(Vector2 position, Texture2D texture): base(position, texture)
        {
            _Layer = 0.1f;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public object Clone(Vector2 position, Vector2 velocity, Component parent)
        {
            Position = position;
            Velocity = velocity;
            Parent = parent;
            return Clone();
        }


        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            if (Active)
            {
                Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (var comp in components)
                {
                    if (!(comp is Sprite))
                        continue;
                    if (!comp.Rectangle.Intersects(this.Rectangle))
                        continue;
                    if (Parent is Enemy)
                    {
                        if (comp is Player)
                        {
                            (comp as Sprite).Damage(this.Position);
                            this.IsRemoved = true;
                        }
                    }
                    else
                    {
                        if (comp != Parent)
                        {
                            (comp as Sprite).Damage(this.Position);
                            this.IsRemoved = true;
                        }

                    }
                }
                foreach(var path in paths)
                {
                    if (this.Rectangle.Intersects(path))
                        this.IsRemoved = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
                base.Draw(spriteBatch);
        }
        public void Reverse(Component parent)
        {
            Parent = parent;
            Velocity = new Vector2(-Velocity.X, -Velocity.Y);
        }
    }
}
