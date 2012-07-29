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
    class EntAirBalloon : Ent
    {
        public static new Model defaultModel;
        double wanderangle = 0;
        EntPC target;

        public EntAirBalloon()
        {
            model = defaultModel;
            size = 3;
            wrapX = false;
            wrapY = false;
            mycolor = new Vector3(0.6f, 0.2f, 0.1f);
        }

        public override void join()
        {
            base.join();
            position = new Vector3(-30, ((float)rand.NextDouble() - 0.5f) * 8f + 20f, ((float)rand.NextDouble() - 0.5f) * 32f);
            velocity = Vector3.Zero;
            resistance = 0.707f;

            List<Ent> ents = manager.Ents;
            foreach (Ent e in ents)
            {
                if (e is EntPC)
                {
                    target = (EntPC)e;
                    break;
                }
            }
        }

        public override void think()
        {
            velocity += new Vector3(1.0f + (float)Math.Cos(Math.Sin(wanderangle) / 2), (float)Math.Sin(Math.Sin(wanderangle) / 2), 0) / 100;
            wanderangle += 0.1;
            base.think();

            if (rand.NextDouble() < 0.01)
            {
                Vector3 d = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                if (target != null)
                    d = target.position - position;
                d.Normalize();

                EntBalloon b = new EntBalloon(true);
                b.position = position + d * size;
                b.velocity = d * 0.2f;

                manager.queueAdd(b);
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
                ((EntBullet)target).owner.score += 10;
            }
        }
    }
}
