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
    class Throwing : ProjectileEnemy
    {
        public Throwing(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            colour = Color.RosyBrown;
            ATTACK_DELAY = 1;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            base.Update(gameTime, components, paths);
        }

        protected override void Attack(List<Component> components, List<Component> addComps, Component comp)
        {
            float xDiff = Position.X - comp.Position.X;
            float yDiff = Position.Y - comp.Position.Y;

            if (Math.Abs(xDiff) > Math.Abs(yDiff))
            {
                attacktimer = 0;
                //move on yAxis
                destination.X = this.Position.X;
                destination.Y = comp.Position.Y;
                if (Math.Abs(destination.Y - Position.Y) < 10)
                {
                    destination = Position;
                    if (projectile != default)
                    {
                        if (this.Position.X > comp.Position.X)
                            shootLeft(addComps);
                        else
                            shootRight(addComps);
                    }
                }
            }
            else
            {
                //move on xAxis

                destination.Y = this.Position.Y;
                destination.X = comp.Position.X;
                if (Math.Abs(destination.X - Position.X) < 10)
                {
                    attacktimer = 0;
                    destination = Position;
                    if (projectile == default)
                    {
                        setProjectile(components);
                    }
                    if (projectile != default)
                    {
                        if (this.Position.Y > comp.Position.Y)
                            shootUp(addComps);
                        else
                            shootDown(addComps);
                    }
                }
            }

        }
    }
}
