using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace SquaresVersusCircles
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Room active;
        int timer;
        Menu Pause;
        bool pRelease;
        bool rRelease;
        bool qRelease;
        bool lRelease;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
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
            // TODO: Add your initialization logic here
            active = new Room();
            //active.TestRoom();
            active.LoadRoom();
            Pause = new Menu(active);
            base.Initialize();
            pRelease = true;
            qRelease = true;
            rRelease = true;
            lRelease = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Quad.SquareTexture = Content.Load<Texture2D>("MenuImage");
            Circle.CircleTexture = Content.Load<Texture2D>("Circle");
            Menu.Myfont = Content.Load<SpriteFont>("small");
            Menu.Bigfont = Content.Load<SpriteFont>("font");
            Menu.MenuTexture = Content.Load<Texture2D>("Wood");
            Menu.ClearTexture = Content.Load<Texture2D>("Square");


            // TODO: use this.Content to load your game content here
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
            HandleInput();
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > 17)
            {
                timer %= 17;


                active.Update();
                // TODO: Add your update logic here

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Rectangle r = Room.Screen;
            r.X = (GraphicsDevice.Viewport.Width - Room.Screen.Width) / 2;
            r.Y = (GraphicsDevice.Viewport.Height - Room.Screen.Height) / 2;
            Room.Screen = r;
            r = Pause.Screen;
            r.X = (GraphicsDevice.Viewport.Width - Pause.Screen.Width) / 2;
            r.Y = (GraphicsDevice.Viewport.Height - Pause.Screen.Height) / 2;
            Pause.Screen = r;
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            //Single State Machine (Paused/NotPaused)
            if (active.IsPaused)
            {
                Pause.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(Quad.SquareTexture, Room.Screen, Color.MediumSeaGreen);
                for(int i = 0; i < active.Immobile.Count; i++)
                {
                    active.Immobile[i].Draw(spriteBatch);
                }
                for (int i = 0; i < active.Mobile.Count; i++)
                {
                    active.Mobile[i].Draw(spriteBatch);
                }
                for (int i = 0; i < active.ProjectileSquare.Count; i++)
                {
                    active.ProjectileSquare[i].Draw(spriteBatch);
                }
                for (int i = 0; i < active.ProjectileCircle.Count; i++)
                {
                    active.ProjectileCircle[i].Draw(spriteBatch);
                }
                for (int i = 0; i < active.Entity.Count; i++)
                {
                    active.Entity[i].Draw(spriteBatch);
                }
                if (active.Player.Health > 0)
                    active.Player.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleInput()
        {
            KeyboardState kb = Keyboard.GetState();
            Point newAccel = new Point(0, 3);
            if (active.IsPaused)
            {
                if (kb.IsKeyDown(Keys.Right))
                {
                    if (rRelease)
                    {
                        rRelease = false;
                        if (active.RoomNumber >= 1 && File.Exists("level" + (active.RoomNumber + 1) + ".txt"))
                        {
                            active.RoomNumber++;
                            active.LoadRoom();
                        }
                    }
                }
                else
                    rRelease = true;
                if (kb.IsKeyDown(Keys.Left))
                {
                    if (lRelease)
                    {
                        lRelease = false;
                        if(active.RoomNumber > 1 && File.Exists("level" + (active.RoomNumber - 1) + ".txt"))
                        {
                            active.RoomNumber--;
                            active.LoadRoom();
                        }
                            
                    }
                }
                else
                    lRelease = true;
                if (kb.IsKeyDown(Keys.Q))
                {
                    if (qRelease)
                    {
                        Exit();
                    }
                }
                else
                    qRelease = true;
            }
            else
            {

                bool stop = true;
                if (kb.IsKeyDown(Keys.D))
                {
                    newAccel.X = 1;
                    stop = !stop;
                }
                if (kb.IsKeyDown(Keys.A))
                {
                    newAccel.X = -1;
                    stop = !stop;
                }
                if (stop)
                {
                    newAccel.X = 0;
                    active.Player.VelocityX.X = 0;
                }
                if(active.Player.ShotTimer == 0 && active.Player.Health > 0)
                {
                    if (kb.IsKeyDown(Keys.Up) && kb.IsKeyUp(Keys.Down))
                    {
                        active.ProjectileSquare.Add(new Quad(active.Player.Position, new Point(0, -15), active));
                        active.Player.ShotTimer = 10;
                    }
                    else if (kb.IsKeyDown(Keys.Down) && kb.IsKeyUp(Keys.Up))
                    {
                        active.ProjectileSquare.Add(new Quad(active.Player.Position, new Point(0, 15), active));
                        active.Player.ShotTimer = 10;
                    }
                    if (kb.IsKeyDown(Keys.Left) && kb.IsKeyUp(Keys.Right))
                    {
                        active.ProjectileSquare.Add(new Quad(active.Player.Position, new Point(-15, 0), active));
                        active.Player.ShotTimer = 10;
                    }
                    if (kb.IsKeyDown(Keys.Right) && kb.IsKeyUp(Keys.Left))
                    {
                        active.ProjectileSquare.Add(new Quad(active.Player.Position, new Point(15, 0), active));
                        active.Player.ShotTimer = 10;
                    }
                }
                else
                {
                    active.Player.ShotTimer--;
                }
                if (kb.IsKeyDown(Keys.R))
                {
                    active.LoadRoom();
                }
                if (kb.IsKeyDown(Keys.Q))
                {
                    active.LoadRoom();
                    active.IsPaused = true;
                    qRelease = false;
                }
                if ((kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.W)) && active.Player.CanJump)
                {
                    active.Player.VelocityY = new Point(0, -15);
                    newAccel.Y = 0;
                    active.Player.CanJump = false;
                }
            }
            if ((kb.IsKeyDown(Keys.P)|| (active.IsPaused && kb.IsKeyDown(Keys.Enter))) && active.CanPlay)
            {
                if (pRelease)
                {
                    active.IsPaused = !active.IsPaused;
                    pRelease = false;
                    if (active.Player.Health <= 0)
                        active.LoadRoom();
                }
            }
            else
            {
                pRelease = true;
            }


            active.Player.Acceleration = newAccel;
        }

    }
}
