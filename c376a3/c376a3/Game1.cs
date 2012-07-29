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

namespace c376a2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int width = 1024;
        public int height = 768;
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Switcher inst;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            Switcher.game = this;
            Switcher.size = new Vector2(width, height);

            inst = new Switcher(new MenuScreen());
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D Tundef = Content.Load<Texture2D>("undef");
            
            SpriteFont Ftahoma = Content.Load<SpriteFont>("tahoma");
            Texture2D Tskygrad = Content.Load<Texture2D>("skygrad");
            SpriteFont Fmenu = Content.Load<SpriteFont>("menufont");
            Texture2D Tstory = Content.Load<Texture2D>("story");
            SpriteFont Fend = Content.Load<SpriteFont>("endfont");
            Texture2D Tpaperplane = Content.Load<Texture2D>("paper1_2d");
            Texture2D Tpaperball = Content.Load<Texture2D>("paper2_2d");

            Model Mskybox = Content.Load<Model>("skybox");
            Model Mcube = Content.Load<Model>("cube");
            Model Mballoon = Content.Load<Model>("balloon");
            Model Mbullet = Content.Load<Model>("bullet");
            Model Mpc = Content.Load<Model>("pc");
            Model Mairballoon = Content.Load<Model>("airballoon");
            Model Mpaperplane = Content.Load<Model>("paper1");
            Model Mpaperball = Content.Load<Model>("paper2");
            
            Ent.defaultSprite = Tundef;
            EntPC.defaultFont = Ftahoma;
            NormalVariant.skygrad = Tskygrad;
            MenuScreen.font = Fmenu;
            StoryScreen.story = Tstory;
            EntText.font = Fend;
            EntPaperBall.defaultSprite = Tpaperball;
            EntPaperPlane.defaultSprite = Tpaperplane;

            Ent.defaultModel = Mcube;
            EntPC.defaultModel = Mpc;
            EntBalloon.defaultModel = Mballoon;
            EntBullet.defaultModel = Mbullet;
            EntAirBalloon.defaultModel = Mairballoon;
            EntPaperPlane.defaultModel = Mpaperplane;
            EntPaperBall.defaultModel = Mpaperball;
            NormalVariant.skyboxModel = Mskybox;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            inst.think(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.YellowGreen);

            inst.draw(spriteBatch);


            base.Draw(gameTime);
        }
    }
}
