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

        float mSlashTimeout;
        const float mMaxSlashTimeout = 0.5f;

        public SwordSlash(World world, Vector2 initPos)
            : base(world, initPos)
        {
            mSlashTimeout = mMaxSlashTimeout;
            mRadius = 50;

            mBody = BodyFactory.CreateCircle(mPhysicsWorld, mRadius / 64f, 1f, mPosition / 64f);
            mBody.BodyType = BodyType.Dynamic;


            
            mFixture = FixtureFactory.AttachCircle(mRadius / 64f, 1f, mBody);
            mFixture.CollisionCategories = Category.Cat2;
            mFixture.CollidesWith = Category.All & ~Category.Cat1;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mSlashTimeout > 0)
            {
                //spriteBatch.Draw(mTexture, mPosition, new Color(255, 255, 255, mSlashTimeout / mMaxSlashTimeout * 255));
                spriteBatch.Draw(mTexture, mBody.Position * 64f, null, new Color(255, 255, 255, mSlashTimeout / mMaxSlashTimeout * 255),
                                0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            mSlashTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            if (mSlashTimeout < 0.0f)
            {
                mSlashTimeout = 0.0f;
                // remove thyself
                GetGameWorld().RemoveGameObject(this);
            }
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("monster");
        }
    }
}