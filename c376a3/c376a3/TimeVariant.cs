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
    class TimeVariant : NormalVariant
    {
        public double startTime;


        public TimeVariant()
        {
            startTime = 0;
        }

        public override void think(GameTime gt)
        {
            base.think(gt);
            double currentTime = gt.TotalGameTime.TotalMilliseconds;

            if (startTime == 0)
            {
                startTime = currentTime;
            }

            foreach (Ent e in entities.Ents)
            {
                if (e is EntBullet)
                    entities.queueRemove(e);

                if (e is EntBalloon)
                {
                    if (Ent.rand.NextDouble() < 0.0005)
                    {

                        Vector3 d = (Epc.position - e.Position);

                        d.Normalize();

                        d *= 0.1f;

                        EntPaperBall pb = new EntPaperBall(e.Position);

                        pb.velocity = d;

                        entities.queueAdd(pb);
                    }
                }

            }
            entities.dequeAll();


            if (Epc.lives > 0)
                Epc.score = (int)(currentTime - startTime) / 1000;
        }
    }
}
