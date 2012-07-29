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
    class EntBalloon : Ent
    {
        private Color color;
        private double wobble;
        private double rotation;
        public EntBalloonSet set = null;

        public static new Model defaultModel;

        protected bool gravity;

        public EntBalloon(bool g = false)
        {
            double r = rand.NextDouble() * Math.PI * 2;
            Color c = new Color((float)Math.Cos(r) * 0.5f + 0.5f, (float)Math.Cos(r + Math.PI * 2.0 / 3) * 0.5f + 0.5f, (float)Math.Cos(r - Math.PI * 2.0 / 3) * 0.5f + 0.5f);

            resistance = 1;
            model = defaultModel;
            size = model.Meshes[0].BoundingSphere.Radius;
            wobble = rand.NextDouble() * Math.PI * 2f;
            rotation = rand.NextDouble() * Math.PI * 2f;
            color = c;
            wrapX = true;

            gravity = g;
            wrapY = !gravity;

            resistance = 1;

            set = null;
            size *= 3 / 4.0f;
        }

        public override Vector3 Position
        {
            get { return position + ((set == null) ? Vector3.Zero : set.position); }
        }

        public override Quaternion Orientation
        {
            get { return orientation * Quaternion.CreateFromYawPitchRoll((float)rotation, 0, (float)Math.Cos(wobble) * 0.5f); }
        }

        public override Vector3 MyColor
        {
            get { return color.ToVector3(); }
        }

        public override void think()
        {
            wobble += 0.1;
            rotation += rand.NextDouble() / 20;

            if (gravity)
                velocity += Vector3.UnitY * -0.001f;

            if (set == null)
                base.think();
        }

        public void pop(EntPC source)
        {
            manager.queueRemove(this);
            if (source != null)
                source.score += 2;

        }

        public override bool collides()
        {
            return (set == null);
        }
        public override void collision(Ent target)
        {
            if (set == null)
            {
                if (target is EntBullet && (!((EntBullet)target).used) && (!pendingRemoval))
                {
                    manager.queueRemove(target);
                    pop(((EntBullet)target).owner);
                    ((EntBullet)target).used = true;
                }
            }
        }
    }
}
