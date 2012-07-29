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
    class EntPaperBall : Ent
    {
        public static new Model defaultModel;
        double angle;

        public EntPaperBall(Vector3 p)
        {
            position = p;
            velocity = Vector3.Zero;
            resistance = 1;
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;
            wrapX = false;
            wrapY = false;
            resistance = 1;
            angle = 0;
        }

        public override void think()
        {
            velocity -= Vector3.UnitY * 0.01f;
            base.think();
            angle += 0.1;
        }

        public override bool collides()
        {
            return false;
        }

    }
}
