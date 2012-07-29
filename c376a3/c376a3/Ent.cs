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
    class Ent
    {
        public static Texture2D defaultSprite;
        public static Model defaultModel;
        public static Random rand = new Random();

        protected Texture2D sprite;
        protected Model model;
        public Vector3 position = new Vector3();
        public Vector3 velocity = new Vector3();
        public Quaternion orientation = Quaternion.Identity;
        public Vector3 mycolor = new Vector3(1, 1, 1);
        public float resistance = 0.9f;
        public EntManager manager;
        public float size = 0;
        public bool wrapX = false;
        public bool wrapY = false;
        public bool pendingRemoval;

        public Ent()
        {
            sprite = defaultSprite;
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;
            pendingRemoval = false;
        }

        public virtual Vector3 Position
        {
            get { return position; }
        }

        public virtual Quaternion Orientation
        {
            get { return orientation; }
        }

        public virtual Vector3 MyColor
        {
            get { return mycolor; }
        }

        public virtual void draw(SpriteBatch sb)
        {
            //sb.Draw(sprite, new Vector2(position.X, position.Y) - new Vector2(sprite.Width / 2, sprite.Height / 2), Color.White);
        }

        public virtual void draw3d() {
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {

                    Matrix p = Matrix.CreatePerspective(0.5f * manager.gameWidth / manager.gameHeight, 0.5f, 0.4f, 256f);
                    Matrix v = manager.cam;
                    Matrix w = Matrix.CreateFromQuaternion(Orientation) * Matrix.CreateTranslation(Position);

                    Vector3 lightpos = new Vector3(1, -1, -1);
                    lightpos.Normalize();

                    effect.DiffuseColor = MyColor;
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One * 0.5f;
                    effect.SpecularPower = 16.0f;
                    effect.PreferPerPixelLighting = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = lightpos;
                    effect.DirectionalLight0.SpecularColor = Vector3.One * 0.8f;
                    effect.Projection = p;
                    effect.World = w;
                    effect.View = v;

                }
                mesh.Draw();
            }
        }

        public virtual void think()
        {
            position += velocity;
            velocity *= resistance;



            if (wrapX)
            {
                position.X = (((position.X % 64) + 96) % 64) - 32;
                position.Z = (((position.Z % 64) + 96) % 64) - 32;
            }
            else
            {
                if ((Math.Abs(position.X) > 32) || (Math.Abs(position.Z) > 32)) {
                    manager.queueRemove(this);                
                }
            }

            if (wrapY)
            {
                position.Y = (((position.Y % 64) + 96) % 64) - 32;
            }
            else
            {
                if (Math.Abs(position.Y) > 32)
                {
                    manager.queueRemove(this);
                }
            }



        }

        public virtual bool collides()
        {
            return false;
        }

        public virtual bool acceptscollides()
        {
            return false;
        }

        public virtual void collision(Ent target)
        {

        }

        public virtual void join() { }
    }
}
