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
    abstract class ProjectileEnemy : Enemy
    {
        protected Projectile projectile = default;

        protected double attacktimer = 10;
        protected static double ATTACK_DELAY = 1;

        public ProjectileEnemy(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            colour = Color.RosyBrown;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {

            if (Parent.Discovered)
            {
                var addComps = new List<Component>();
                foreach (var comp in components)
                {
                    if (comp is Player)
                    {
                        if (attacktimer > ATTACK_DELAY)
                        {
                            if (projectile == default)
                            {
                                setProjectile(components);
                            }
                            Attack(components, addComps, comp);
                        }
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
                foreach (var comp in addComps)
                    components.Add(comp);
                if (attacktimer <= ATTACK_DELAY)
                    attacktimer += gameTime.ElapsedGameTime.TotalSeconds;
                base.Update(gameTime, components, paths);
            }
            if (Parent.IsRemoved && !Parent.Discovered)
                this.IsRemoved = true;
        }

        protected abstract void Attack(List<Component> components, List<Component> addComps, Component comp);


        protected void shootDown(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X + ((_texture.Width / 2) - projectile._texture.Width / 2), Position.Y + _texture.Height), new Vector2(0, 200), this) as Component);
        }

        protected void shootUp(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X + ((_texture.Width / 2) - projectile._texture.Width / 2), Position.Y), new Vector2(0, -200), this) as Component);
        }

        protected void shootRight(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X + _texture.Width, Position.Y + ((_texture.Width / 2) - projectile._texture.Width / 2)), new Vector2(200, 0), this) as Component);
        }

        protected void shootLeft(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X, Position.Y + ((_texture.Width / 2) - projectile._texture.Width / 2)), new Vector2(-200, 0), this) as Component);
        }

        protected void shootUpLeft(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X, Position.Y), new Vector2(-200, -200), this) as Component);
        }
        protected void shootUpRight(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X + _texture.Width, Position.Y), new Vector2(200, -200), this) as Component);
        }
        protected void shootDownLeft(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X, Position.Y + _texture.Height), new Vector2(-200, 200), this) as Component);
        }
        protected void shootDownRight(List<Component> addComps)
        {
            addComps.Add(projectile.Clone(new Vector2(Position.X + _texture.Width, Position.Y + _texture.Height), new Vector2(200, 200), this) as Component);
        }
        protected void setProjectile(List<Component> components)
        {
            foreach (var p in components)
            {
                if (p is Projectile)
                {
                    if (!(p as Projectile).Active)
                        projectile = (p as Projectile).Clone() as Projectile;
                }
            }
            if (projectile != default)
                projectile.Active = true;
        }

    }
}
