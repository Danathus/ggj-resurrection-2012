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

using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ggj_resurrection
{
    class SwordSlash : GameObject
    {
        static private Texture2D mTexture;

        bool timedOut = false;
        float mSlashTimeout;
        Vector2 rightStickRotation;
        Vector2 oldRightStick;
        float batRotation;
        const float mMaxSlashTimeout = 0.5f;

        public SwordSlash(World world, Vector2 initPos)
            : base(world, initPos)
        {
            mSlashTimeout = mMaxSlashTimeout;
            mRadius = 1;

            //mBody = BodyFactory.CreateRectangle(mPhysicsWorld, mTexture.Width / 64f, mTexture.Height / 64f, .1f);         
     

            mFixture = FixtureFactory.AttachRectangle(mTexture.Width * Camera.kPixelsToUnits, mTexture.Height * Camera.kPixelsToUnits, 10f, new Vector2(0 ,0) *Camera.kPixelsToUnits, new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Static;
            mFixture.Body.Restitution = 0f;
            mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);


            //mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);
            mFixture.CollisionCategories = Category.Cat2;
            mFixture.Body.CollisionCategories = Category.Cat2;
            mFixture.CollidesWith = Category.All & ~Category.Cat1 & ~Category.Cat2;
            mFixture.Body.CollidesWith = Category.All & ~Category.Cat1 & ~Category.Cat2;
            mFixture.UserData = "Sword";
            mFixture.Body.UserData = "Sword";
            mFixture.Restitution = 0f;
            
            mFixture.Body.OnCollision += swordOnCollision;
        }

        public bool swordOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (contact.FixtureA.Body.UserData.ToString() == "Sword" || contact.FixtureB.Body.UserData.ToString() == "Player")
            {
                return false;
            }

            return true;
        }

        public bool isTimedOut()
        {
            if (timedOut)
            {
                timedOut = false;
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mSlashTimeout > 0)
            {
                int opacity = (int)(mSlashTimeout / mMaxSlashTimeout * 255);
                spriteBatch.Draw(mTexture, mFixture.Body.Position, null, new Color(opacity, opacity, opacity, opacity),
                                batRotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), Camera.kPixelsToUnits, SpriteEffects.None, 0f);
            }
        }

        public void setAngularVelocity(SwordSlash old, SwordSlash current)
        {
            float difference = current.batRotation - old.batRotation;
            difference /= 5000f;
            mFixture.Body.AngularVelocity = difference;
        }

        public void setRotation(Vector2 stick)
        {
            oldRightStick = rightStickRotation;
            rightStickRotation = stick;

            Vector2 stickDifference = rightStickRotation - oldRightStick;

            //rightStickRotation.Normalize();
            double ratio = rightStickRotation.Y / rightStickRotation.X;
            float rotator = (float)Math.Atan2(stickDifference.Y, stickDifference.X);
            
            //rotator *= (3.1415926535f / 180f);
            rotator += ((3.1415926535f) / 2f);
            rotator += (float)Math.PI;

            //if (rightStickRotation.X < 0)
            //{
            //    rotator += (3.1415926535f);
            //}

            batRotation = rotator;
            mFixture.Body.Rotation = rotator;
        }

        public override void Update(GameTime gameTime)
        {
            mSlashTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //SetVelocity(GetVelocity() * 0.9f);
           
            if (mSlashTimeout < 0.0f)
            {
                mSlashTimeout = 0.0f;
                timedOut = true;
                // remove thyself
                GetGameWorld().RemoveGameObject(this);
            }
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("Bat");
        }
    }
}
