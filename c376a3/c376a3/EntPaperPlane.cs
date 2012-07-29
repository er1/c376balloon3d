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
    class EntPaperPlane : Ent
    {
        public static new Model defaultModel;

        public EntPaperPlane()
        {
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;
            wrapX = false;
            wrapY = true;

            orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -(float)Math.PI / 2.0f);
        }

        public override void join()
        {
            base.join();
            position = new Vector3(-31f, (float)rand.NextDouble() * 12f + 12f, (float)rand.NextDouble() * 48f - 24f);
            velocity = Vector3.UnitX * 0.1f;
            resistance = 1;

        }

        public override void think()
        {
            base.think();
            if (rand.NextDouble() < 0.01) {
                Ent e = new EntPaperBall(position - Vector3.UnitX * size);
                manager.queueAdd(e);
            }
        }

        public override bool collides()
        {
            return true;
        }
        public override void collision(Ent target)
        {
            if (target is EntBullet && (!((EntBullet)target).used) && (!pendingRemoval))
            {
                manager.queueRemove(target);
                manager.queueRemove(this);
                ((EntBullet)target).owner.lives += 1;
            }
        }
    }
}
