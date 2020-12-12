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
    class Repel : ProjectileEnemy
    {
        public Repel(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            Health = 2;
            Parent = parent;
            colour = Color.DeepSkyBlue;
            ATTACK_DELAY = 2;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            base.Update(gameTime, components, paths);
        }

        protected override void Attack(List<Component> components, List<Component> addComps, Component comp)
        {
            destination = new Vector2(Position.X + comp.Position.X, Position.Y + comp.Position.Y);
            shootUpLeft(addComps);
            shootUpRight(addComps);
            shootDownLeft(addComps);
            shootDownRight(addComps);
            shootUp(addComps);
            shootDown(addComps);
            shootLeft(addComps);
            shootRight(addComps);
            attacktimer = 0;
        }
    }
}
