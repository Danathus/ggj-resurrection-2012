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
    class SwordSlash : GameObject
    {
        static private Texture2D mTexture;

        int opacity = 255;
        bool timedOut = false;
        float mSlashTimeout;
        Vector2 rightStickRotation;
        float batRotation;
        const float mMaxSlashTimeout = 0.5f;

        public SwordSlash(World world, Vector2 initPos)
            : base(world, initPos)
        {
            mSlashTimeout = mMaxSlashTimeout;
            mRadius = 1;

            mBody = BodyFactory.CreateRectangle(mPhysicsWorld, mTexture.Width / 64f, mTexture.Height / 64f, 1f);
            mBody.BodyType = BodyType.Dynamic;

            mBody.CollisionCategories = Category.Cat2;
            mBody.UserData = "Sword";
            
            mFixture = FixtureFactory.AttachRectangle(mTexture.Width / 64f, mTexture.Height / 64f, 1f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), mBody);
            mBody.Position = mPosition / 64f;
            mFixture.CollisionCategories = Category.Cat2;
            mFixture.Body.CollisionCategories = Category.Cat2;
            mFixture.CollidesWith = Category.All & ~Category.Cat1;
            mFixture.UserData = "Sword";
            mFixture.Body.UserData = "Sword";
            
            mBody.OnCollision += swordOnCollision;
        }

        public bool swordOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (two.Body.UserData.ToString() == "Sword" || two.Body.UserData.ToString() == "Player")
            {
                return false;
            }

            return true;
        }

        public bool isTimedOut()
        {
            return timedOut;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mSlashTimeout > 0)
            {
                //spriteBatch.Draw(mTexture, mPosition, new Color(255, 255, 255, mSlashTimeout / mMaxSlashTimeout * 255));
                spriteBatch.Draw(mTexture, mBody.Position * 64f, null, new Color(255, 255, 255, opacity),
                                batRotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
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
            rightStickRotation = stick;

            //rightStickRotation.Normalize();
            double ratio = rightStickRotation.Y / rightStickRotation.X;
            float rotator = (float)Math.Atan(ratio);
            

            //rotator *= (3.1415926535f / 180f);
            rotator += ((3f *3.1415926535f )/ 2f);

            if (rightStickRotation.X < 0)
            {
                rotator += (3.1415926535f);
            }

            batRotation = rotator;

        }

        public override void Update(GameTime gameTime)
        {
            mSlashTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            SetVelocity(GetVelocity() * 0.9f);
           
            if (mSlashTimeout < 0.0f)
            {
                mSlashTimeout = 0.0f;
                timedOut = true;
                // remove thyself
                GetGameWorld().RemoveGameObject(this);
            }

            opacity -= 5;

        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("Bat");
        }
    }
}
