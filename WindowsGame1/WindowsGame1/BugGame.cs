using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// /// This is the main type for your game
    /// </summary>
    public class BugGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera camera;
        private Model field;
        private Model sky;
        private Vector3 skyPosition = new Vector3(0, -5, 0);
        private Butterfly butterfly;

        private KeyboardState lastKeyboardState;
        

        //private Model butterfly;

        public BugGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            butterfly = new Butterfly(this);
            camera = new Camera(graphics);
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
            camera.Initialize();
            base.Initialize();
            lastKeyboardState = Keyboard.GetState();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            butterfly.LoadContent(Content);
            field = Content.Load<Model>("GrassField2");
            sky = Content.Load<Model>("Sky");

            // TODO: use this.Content to load your game content here
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
            camera.Eye = Vector3.Transform(new Vector3(0, 10, -50), butterfly.Transform);
            camera.Center = butterfly.Position;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                butterfly.IncreaseAltitude = true;
            }
            else
                butterfly.IncreaseAltitude = false;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                butterfly.TurnRate = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                butterfly.TurnRate = -1;
            }
            else
            {
                butterfly.TurnRate = 0;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                butterfly.Move = 1;
            }
            else
            {
                butterfly.Move = 0;
            }

            lastKeyboardState = keyboardState;

            butterfly.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           GraphicsDevice.Clear(Color.CornflowerBlue);
           Matrix position =  Matrix.CreateTranslation(new Vector3(0,0,0));
           Matrix scale = Matrix.CreateScale(20f, 0, 20f);
            
            DrawGround(graphics, field, scale * position);
            Matrix transform = Matrix.CreateTranslation(skyPosition);
            
            DrawSky(graphics, sky, transform);
            // TODO: Add your drawing code here
            butterfly.Draw(graphics, gameTime);
            base.Draw(gameTime);
        }

        private void DrawGround(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
           Matrix[] transforms = new Matrix[model.Bones.Count];
           model.CopyAbsoluteBoneTransformsTo(transforms);
           
           //transforms[wing1] = Matrix.CreateRotationZ(wingAngle) * transforms[wing1];
          // transforms[wing2] = Matrix.CreateRotationZ(-wingAngle) * transforms[wing2];
           //Console.WriteLine("here");
           foreach (ModelMesh mesh in model.Meshes)
           {
               foreach (BasicEffect effect in mesh.Effects)
               {
                   effect.EnableDefaultLighting();
                   effect.World = transforms[mesh.ParentBone.Index] * world;
                   effect.View = Camera.View;
                   effect.Projection = Camera.Projection;
               }
               mesh.Draw();
           }
        }

        private void DrawSky(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            //transforms[wing1] = Matrix.CreateRotationZ(wingAngle) * transforms[wing1];
            // transforms[wing2] = Matrix.CreateRotationZ(-wingAngle) * transforms[wing2];
            //Console.WriteLine("here");
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = Camera.View;
                    effect.Projection = Camera.Projection;
                }
                mesh.Draw();
            }
        }

        public Camera Camera { get { return camera; } }

    }
}
