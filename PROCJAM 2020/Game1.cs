using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PROCJAM_2020.Components.Sprites;
using PROCJAM_2020.Components;
using System.Collections.Generic;
using PROCJAM_2020.GameWorld;
using PROCJAM_2020.Components.Sprites.Enemies;

namespace PROCJAM_2020
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int SCREEN_WIDTH = 400;
        public static int SCREEN_HEIGHT = 400;
        Camera _camera;
        Player _player;
        Texture2D enemyTex;
        LoadedMap _map;
        List<Component> _components;
        int highScore = 0;
        int RoomsDiscovered = 0;
        Texture2D path;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
            _camera = new Camera(GraphicsDevice, graphics, new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT));
            _components = new List<Component>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            path = Content.Load<Texture2D>("path");
            enemyTex = Content.Load<Texture2D>("unit");
            Reset();
        }

        private void Reset()
        {
            _player = new Player(new Vector2( - 20, -20), Content.Load<Texture2D>("unit"));
            _map = new LoadedMap();
            _components.Clear();
            _components.Add(_player);
            _components.Add(new Projectile(Vector2.Zero, Content.Load<Texture2D>("bullet")));
            if (RoomsDiscovered > highScore)
                highScore = RoomsDiscovered;
            RoomsDiscovered = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if(_player.IsRemoved)
            {
                Reset();
                
            }

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update(gameTime, _components, _map.ClosedArea);
            }
            _map.Update(gameTime, _player.Rectangle);
            _camera.Follow(_player);

            foreach(var room in _map.Rooms)
            {
                if(room.Discovered && !room.Scored)
                {
                    RoomsDiscovered++;
                    room.Scored = true;
                }
                if(!room.EnemiesSpawned)
                {
                    //add enemies
                    foreach (var enemy in room.Follow)
                        _components.Add(new Follow(enemy, enemyTex, room));
                    foreach(var enemy in room.Projectile)
                        _components.Add(new Throwing(enemy, enemyTex, room));
                    foreach (var enemy in room.Mover)
                        _components.Add(new Mover(enemy, enemyTex, room));
                    foreach(var enemy in room.Quad)
                        _components.Add(new Quad(enemy, enemyTex, room));
                    foreach (var enemy in room.Repel)
                        _components.Add(new Repel(enemy, enemyTex, room));
                    room.EnemiesSpawned = true;
                }
            }
            for (int i = 0; i < _components.Count; i++)
                if (_components[i].IsRemoved)
                    _components.RemoveAt(i--);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_camera.RenderTarget);
            GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Translation, sortMode: SpriteSortMode.FrontToBack);
            //draw here

            foreach (var comp in _components)
                comp.Draw(spriteBatch);

            foreach(var rect in _map.ClosedArea)
            {
                spriteBatch.Draw(path, rect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f); 
            }
            foreach(var rect in _map.BlankedArea)
                spriteBatch.Draw(path, rect, null, Color.DarkGray, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_camera.RenderTarget, _camera.ScreenRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
