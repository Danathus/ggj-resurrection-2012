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

using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    class DeadPlayer : Player
    {
        public DeadPlayer(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mFixture.Body.OnCollision += deadPlayerOnCollision;
        }

        public bool deadPlayerOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (two.UserData.ToString() == "Grave" || two.Body.UserData.ToString() == "Grave")
            {
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset =
                -new Vector2(kFrameSizeInPixels.X / 2, -kFrameSizeInPixels.Y * 2 / 2) * Camera.kPixelsToUnits;
            mSpriteAnimPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(
                spriteOffset, 0, Color.White, 1 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            UpdateInput();

            Vector2 direction = DetermineDesiredDirection();

            mFixture.Body.ResetDynamics();
            mFixture.Body.LinearVelocity = new Vector2(0, 0);
            mFixture.Body.Rotation = 0;

            if (direction.Length() > .065f)
            {
                mFixture.Body.LinearVelocity = (direction * mMaxSpeed);
            }

            if (direction.Length() > 0)
            {
                direction.Normalize();
                mDirection = direction;
            }

            mPosition = new Vector2(mFixture.Body.Position.X, mFixture.Body.Position.Y); // converts Body.Position (meters) into pixels

            UpdateAnimation(gameTime, direction);
        }

        public static void LoadData(Game myGame)
        {
            // load all static data here
        } // LoadData()
    } // class DeadPlayer
} // namespace ggj_resurrection
