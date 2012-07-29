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
    class EntBullet : Ent
    {
        public int life = 200;
        public EntPC owner;
        public bool used;

        public static new Model defaultModel;

        public EntBullet(EntPC o, Quaternion q)
        {
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;

            wrapX = false;
            wrapY = false;
            used = false;

            owner = o;

            position = owner.position;
            orientation = q;

            Quaternion r = Quaternion.CreateFromYawPitchRoll((float)rand.NextDouble() / 20 - 1 / 40f, (float)rand.NextDouble() / 20 - 1 / 40f, (float)rand.NextDouble() / 20 - 1 / 40f);

            velocity = Vector3.Transform(Vector3.UnitZ * -2f, orientation * r);
            velocity += owner.velocity;
            resistance = 1;

            mycolor = new Vector3(0.8f, 0.3f, 0);

            size *= 2;
        }

        public override void think()
        {
            base.think();
            //life--; wrap is not on
            if (life < 0)
                manager.queueRemove(this);
        }

        public override void collision(Ent target)
        {

        }
    }
}
