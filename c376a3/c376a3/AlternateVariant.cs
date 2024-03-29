﻿using System;
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
    class AlternateVariant : NormalVariant
    {
        public bool specialFired;
        public bool paperdeployed;

        public AlternateVariant()
        {
            specialFired = false;
            paperdeployed = false;
        }

        public override void think(GameTime gt)
        {
            base.think(gt);
            double currentTime = gt.TotalGameTime.TotalMilliseconds;

            if (!paperdeployed && (Ent.rand.NextDouble() < 0.003))
            {
                paperdeployed = true;
                Ent e = new EntPaperPlane();
                entities.add(e);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z) || (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed))
            {
                if (!specialFired && Epc.CanFire)
                {
                    specialFired = true;

                    for (double i = 0; i < 2 * Math.PI; i += Math.PI / 7)
                    {
                        for (double j = 0; j < 2 * Math.PI; j += Math.PI / 7)
                        {
                            // create bullet and fire it;
                            Quaternion r = Quaternion.CreateFromYawPitchRoll((float)Ent.rand.NextDouble() - 0.5f, (float)Ent.rand.NextDouble() - 0.5f, (float)Ent.rand.NextDouble() - 0.5f);
                            EntBullet Ebullet = new EntBullet(Epc, Quaternion.CreateFromYawPitchRoll((float)i, (float)j, 0) * r);
                            entities.add(Ebullet);
                        }

                    }
                }
            }
        }
    }
}
