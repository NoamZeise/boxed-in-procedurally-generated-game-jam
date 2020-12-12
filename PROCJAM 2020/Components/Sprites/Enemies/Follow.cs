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
    class Follow : Enemy
    {
        public Follow(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            Health = 3;
            Parent = parent;
            colour = Color.DarkBlue;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            if (Parent.Discovered)
            {
                foreach (var comp in components)
                {
                    if (comp is Player)
                    {
                        destination = comp.Position;
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
            }
            if (Parent.IsRemoved && !Parent.Discovered)
                this.IsRemoved = true;
        }
    }
}
