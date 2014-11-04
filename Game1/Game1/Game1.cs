#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Game1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level = new Level();       
        WaveManager waveManager;       
        Player player;
        Toolbar toolBar;
        Button arrowButton;
        Button spikeButton;
        Button slowButton;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Синякдефенс";
            graphics.PreferredBackBufferWidth = level.Width * 32;
            graphics.PreferredBackBufferHeight = 32 + level.Height * 32;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

     
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            Texture2D grass = Content.Load<Texture2D>("grass.png");
            Texture2D path = Content.Load<Texture2D>("path.png");
            level.AddTexture(grass);
            level.AddTexture(path);
            Texture2D enemyTexture = Content.Load<Texture2D>("enemy");
            //enemy1 = new Enemy(enemyTexture, Vector2.Zero, 100, 10, 0.5f);
            //enemy1.SetWaypoints(level.Waypoints);

            // Texture2D towerTexture = Content.Load<Texture2D>("arrowtower");
            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");



            Texture2D[] towerTextures = new Texture2D[]
            {
            Content.Load<Texture2D>("arrowtower"),
            Content.Load<Texture2D>("spike tower"),
            Content.Load<Texture2D>("slow tower")
            };

            player = new Player(level, towerTextures, bulletTexture);
            Texture2D topBar = Content.Load<Texture2D>("tool bar");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            waveManager = new WaveManager(player, level, 24, enemyTexture);
            toolBar = new Toolbar(topBar, font, new Vector2(0, level.Height * 32), graphics.PreferredBackBufferWidth);

            // The "Normal" texture for the arrow button.
            Texture2D arrowNormal = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrowbutton");
            // The "MouseOver" texture for the arrow button.
            Texture2D arrowHover = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrowhover");
            // The "Pressed" texture for the arrow button.
            Texture2D arrowPressed = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrowpressed");
            // Initialize the arrow button.
            arrowButton = new Button(arrowNormal, arrowHover, arrowPressed, new Vector2(0, level.Height * 32));
            // arrowButton.Clicked += new EventHandler(arrowButton_Clicked);

            // The "Normal" texture for the spike button.
            Texture2D spikeNormal = Content.Load<Texture2D>("GUI\\Spike Tower\\spike button");
            // The "MouseOver" texture for the spike button.
            Texture2D spikeHover = Content.Load<Texture2D>("GUI\\Spike Tower\\spike hover");
            // The "Pressed" texture for the spike button.
            Texture2D spikePressed = Content.Load<Texture2D>("GUI\\Spike Tower\\spike pressed");
            // Initialize the spike button.
            spikeButton = new Button(spikeNormal, spikeHover, spikePressed, new Vector2(32, level.Height * 32));
            //spikeButton.Clicked += new EventHandler(spikeButton_Clicked);

            // The "Normal" texture for the spike button.
            Texture2D slowNormal = Content.Load<Texture2D>("GUI\\Slow Tower\\slow button");
            // The "MouseOver" texture for the spike button.
            Texture2D slowHover = Content.Load<Texture2D>("GUI\\Slow Tower\\slow hover");
            // The "Pressed" texture for the spike button.
            Texture2D slowPressed = Content.Load<Texture2D>("GUI\\Slow Tower\\slow pressed");
            // Initialize the spike button.
            slowButton = new Button(slowNormal, slowHover, slowPressed, new Vector2(64, level.Height * 32));
            //slowButton.Clicked += new EventHandler(slowButton_Clicked);
            arrowButton.OnPress += new EventHandler(arrowButton_OnPress);
            spikeButton.OnPress += new EventHandler(spikeButton_OnPress);
            slowButton.OnPress += new EventHandler(slowButton_OnPress);
        }

        private void arrowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
            player.NewTowerIndex = 0;
        }
        private void spikeButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Spike Tower";
            player.NewTowerIndex = 1;
        }
        private void slowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Slow Tower";
            player.NewTowerIndex = 2;
        }

        private void arrowButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
            player.NewTowerIndex = 0;
        }
        private void spikeButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Spike Tower";
            player.NewTowerIndex = 1;
        }
        private void slowButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Slow Tower";
            player.NewTowerIndex = 2;
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            waveManager.Update(gameTime);
            player.Update(gameTime, waveManager.Enemies);
            arrowButton.Update(gameTime);
            //Update the spike button.
            spikeButton.Update(gameTime);
            slowButton.Update(gameTime);
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            level.Draw(spriteBatch);
            waveManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
         
            toolBar.Draw(spriteBatch, player);
            
            spikeButton.Draw(spriteBatch);

            arrowButton.Draw(spriteBatch);

                        slowButton.Draw(spriteBatch);

            player.DrawPreview(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
