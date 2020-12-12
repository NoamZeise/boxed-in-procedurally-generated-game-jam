using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROCJAM_2020.Components
{
    public abstract class Component
    {
        public Vector2 Position;
        public Texture2D _texture;

        protected float _Layer;
        protected Color colour = Color.White;
        public bool IsRemoved = false;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Component(Vector2 position, Texture2D texture)
        {
            Position = position;
            _texture = texture;
            _Layer = 0.5f;
        }


        public abstract void Update(GameTime gameTime, List<Component> components, List<Rectangle> paths);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, colour, 0f, Vector2.Zero, 1f, SpriteEffects.None, _Layer);
        }
    }
}
