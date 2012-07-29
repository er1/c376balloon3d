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
    class EntPC : Ent
    {
        public static new Model defaultModel;

        public static SpriteFont defaultFont;
        public SpriteFont font;
        public bool facingLeft = false;
        public bool flipped = false;
        public bool dying = false;
        public int score;
        public float lives;
        public double direction;
        public int flashing;

        public bool CanFire
        {
            get { return ((!dying) && (lives > 0)); }
        }

        public EntPC()
        {
            sprite = defaultSprite;
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;

            mycolor = new Vector3(0.3f, 0.3f, 0.4f);

            wrapX = true;
            wrapY = true;
            score = 0;
            lives = 3;

            direction = 0;

            flashing = 0;
        }

        public override void think()
        {
            if (lives <= 0)
                return;

            //base.think();
            position += velocity;
            velocity *= resistance;

           
            // derp

            if (!dying)
            {
                position = Vector3.Clamp(position, Vector3.One * -31, Vector3.One * 31);

                if (velocity.Length() > 0.2f)
                {
                    direction = Math.Atan2(velocity.Y, velocity.X);
                }

                if (flashing > 0)
                    flashing--;
            }
            else
            {

                flashing = 0;
                direction += 0.2;

                velocity -= Vector3.UnitY * 0.2f;

                if (position.Y < 31f)
                {
                    dying = false;

                    flashing = 100;
                    lives--;
                    position = Vector3.Zero;
                }
            }
        }

        public override bool collides()
        {
            return true;
        }

        public override void collision(Ent target)
        {
            if ((flashing == 0) && !dying &&
                ((target is EntBalloon) ||
                (target is EntAirBalloon) ||
                (target is EntPaperBall)))
            {
                dying = true;
            }

        }
    }
}
