using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace c376a2
{
    class NormalVariant : ThinkDraw
    {
        public static Texture2D skygrad;
        public static Model skyboxModel;

        protected Game caller;
        protected Vector2 windowSize;

        protected EntManager entities;
        protected EntPC Epc;

        protected double lastFired = 0;

        protected bool isOver
        {
            get { return (Epc.lives <= 0); }
        }

        protected bool endShown = false;

        protected bool doNextScreen = false;

        public int startballoons;
        public bool hab1 = true;
        public bool hab2 = true;
        public bool hab3 = true;

        protected Viewport vpfull;
        protected Viewport vptiny;

        public NormalVariant()
        {
            vpfull = Game1.graphics.GraphicsDevice.Viewport;
            vptiny = new Viewport(vpfull.Width * 2 / 3, 0, vpfull.Width / 3, vpfull.Height / 3);

            vpfull.MinDepth = 0.7f;
            vptiny.MaxDepth = 0.7f;

        }

        public NormalVariant(Game g, Vector2 v)
        {
            init(g, v);
        }

        public void init(Game g, Vector2 v)
        {
            caller = g;
            windowSize = v;


            entities = new EntManager();
            entities.gameWidth = ((int)windowSize.X);
            entities.gameHeight = ((int)windowSize.Y);

            Epc = new EntPC();
            Epc.position = Vector3.Zero;
            Epc.resistance = 0.8f;
            Epc.font = EntPC.defaultFont;
            entities.add(Epc);

            int numsets = 5;
            for (int i = 0; i < numsets; ++i)
            {
                Vector3 p = new Vector3((float)Math.Cos(Math.PI * 2 * i / numsets), (float)Math.Sin(Math.PI * 2 * i / numsets), (float)(Ent.rand.NextDouble() * 2.0 - 1.0));
                EntBalloonSet e = new EntBalloonSet(p * 8);
                e.velocity = p * 0.1f;
                entities.add(e);
            }

            startballoons = 0;
            foreach (Ent e in entities.Ents)
            {
                if (e is EntBalloon) startballoons++;
            }

        }

        public virtual void think(GameTime gt)
        {
            double currentTime = gt.TotalGameTime.TotalMilliseconds;
            if (lastFired == 0)
            {
                lastFired = gt.TotalGameTime.TotalMilliseconds;
            }

            // Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                doNextScreen = true;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Epc.velocity += Vector3.Transform(Vector3.UnitZ * -0.2f, Epc.Orientation);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Epc.velocity += Vector3.Transform(Vector3.UnitZ * 0.2f, Epc.Orientation);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Epc.velocity += Vector3.Transform(Vector3.UnitX * -0.2f, Epc.Orientation);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Epc.velocity += Vector3.Transform(Vector3.UnitX * 0.2f, Epc.Orientation);

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Length() > 0.25f) {
                Vector2 d = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                Epc.velocity += new Vector3(d.X * 0.25f, 0, d.Y * 0.25f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.I))
                Epc.orientation = Epc.orientation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.02f);
            
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                Epc.orientation = Epc.orientation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, -0.02f);

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                Epc.orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.02f) * Epc.orientation;

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                Epc.orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -0.02f) * Epc.orientation;

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Length() > 0.25f)
            {
                Vector2 d = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                Epc.orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, d.X * -0.02f) * Epc.orientation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, -0.02f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed))
            {
                if ((currentTime - lastFired) > 200 && Epc.CanFire)
                {
                    lastFired = currentTime;
                    // create bullet and fire it;
                    EntBullet Ebullet = new EntBullet(Epc, Epc.orientation);
                    Ebullet.position +=  Vector3.Transform(-Vector3.UnitZ * 2, Epc.orientation);
                    entities.add(Ebullet);
                }
            }



            entities.thinkAll();
            entities.collisionAll();

            entities.cam = Matrix.CreateTranslation(-Epc.position) * Matrix.CreateFromQuaternion(Quaternion.Inverse(Epc.orientation)) * Matrix.CreateTranslation(Vector3.UnitZ * 0);

            int numballoons = 0;
            int numsets = 0;
            int numairballoons = 0;
            foreach (Ent e in entities.Ents)
            {
                if (e is EntBalloon) numballoons++;
                if (e is EntAirBalloon) numairballoons++;
                if (e is EntBalloonSet) numsets++;
            }

            if (!endShown)
            {
                if (numballoons == 0)
                {
                    EntText e = new EntText("YOU WIN", Color.White);
                    entities.add(e);
                    endShown = true;
                }
                if (Epc.lives == 0)
                {
                    EntText e = new EntText("GAME OVER", Color.Red);
                    entities.add(e);
                    endShown = true;
                }
            }

            if (((float)numballoons / (float)startballoons <= 0.9) && hab1)
            {
                hab1 = false;
                EntAirBalloon e = new EntAirBalloon();
                entities.add(e);
            }
            if (((float)numballoons / (float)startballoons <= 0.6) && hab2)
            {
                hab2 = false;
                EntAirBalloon e = new EntAirBalloon();
                entities.add(e);
            }
            if (((float)numballoons / (float)startballoons <= 0.3) && hab3)
            {
                hab3 = false;
                EntAirBalloon e = new EntAirBalloon();
                entities.add(e);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    Matrix p = Matrix.CreatePerspective(0.5f * entities.gameWidth / entities.gameHeight, 0.5f, 0.4f, 256f);
                    Matrix v = entities.cam;
                    Matrix w = Matrix.CreateScale(32);

                    Vector3 lightpos = new Vector3(1, -1, -1);
                    lightpos.Normalize();

                    effect.Projection = p;
                    effect.World = w;
                    effect.View = v;

                }
                mesh.Draw();
            }

            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            entities.draw3dAll();

            // derp cam
            Game1.graphics.GraphicsDevice.Viewport = vptiny;



            Matrix oldcam = entities.cam;
            entities.cam = Matrix.CreateLookAt(Vector3.One * 31f, Vector3.Zero, Vector3.UnitY);

            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    Matrix p = Matrix.CreatePerspective(0.5f * entities.gameWidth / entities.gameHeight, 0.5f, 0.4f, 256f);
                    Matrix v = entities.cam;
                    Matrix w = Matrix.CreateScale(32);

                    Vector3 lightpos = new Vector3(1, -1, -1);
                    lightpos.Normalize();

                    effect.Projection = p;
                    effect.World = w;
                    effect.View = v;

                }
                mesh.Draw();
            }

            entities.draw3dAll();
            entities.cam = oldcam;

            Game1.graphics.GraphicsDevice.Viewport = vpfull;

            spriteBatch.Begin();

            entities.drawAll(spriteBatch);

            spriteBatch.DrawString(Epc.font, "Score: " + Epc.score.ToString(), new Vector2(64, 32), Color.Red);
            spriteBatch.DrawString(Epc.font, "Lives: " + Epc.lives.ToString(), new Vector2(64, 64), Color.Black);

            spriteBatch.End();
        }

        public ThinkDraw next()
        {
            if (doNextScreen)
            {
                return new MenuScreen();
            }
            return null;
        }
    }
}
