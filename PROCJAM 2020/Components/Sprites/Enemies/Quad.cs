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
    class Quad : ProjectileEnemy
    {

        public Quad(Vector2 position, Texture2D texture, Room parent) : base(position, texture, parent)
        {
            colour = Color.Orange;
            Health = 4;
            ATTACK_DELAY = 1.5;
        }

        public override void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths)
        {
            base.Update(gameTime, components, paths);
        }

        protected override void Attack(List<Component> components, List<Component> addComps, Component comp)
        {
            shootUp(addComps);
            shootDown(addComps);
            shootLeft(addComps);
            shootRight(addComps);
            attacktimer = 0;
        }
    }
}
