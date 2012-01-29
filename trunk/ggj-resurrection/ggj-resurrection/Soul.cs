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
    class Soul : GameObject
    {
        // static data
        public static Vector2 kFrameSizeInPixels = new Vector2(50, 50); // C# won't let me make this const, but please don't change!
        protected static SpriteSheet     mSoulSpriteSheet;
        protected static SpriteAnimation mSoulAnimation;

        // instance data
        protected SpriteAnimationPlayer mSpriteAnimPlayer;
        float mFloatie;

        public Soul(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mSpriteAnimPlayer = new SpriteAnimationPlayer();
            mSpriteAnimPlayer.SetAnimationToPlay(mSoulAnimation);

            mFixture = FixtureFactory.AttachRectangle(30 * Camera.kPixelsToUnits, 30 * Camera.kPixelsToUnits,
                1f, // density
                kFrameSizeInPixels / 2 * Camera.kPixelsToUnits, // offset
                new Body(mPhysicsWorld));
            mFixture.Body.CollisionCategories = Category.Cat30;
            mFixture.Body.CollidesWith        = Category.All;// &~Category.Cat31;
            mFixture.CollisionCategories      = Category.Cat30;
            mFixture.CollidesWith             = Category.All;//& ~Category.Cat31;
            mFixture.Body.OnCollision        += soulOnCollision;
            mFixture.Body.BodyType            = BodyType.Static;

            mFixture.UserData      = "Soul";
            mFixture.Body.UserData = "Soul";

            mFixture.Body.Position = mPosition;
            mFloatie = 0;
        }

        public override void Update(GameTime gameTime)
        {
            mSpriteAnimPlayer.Update(gameTime);
            if (!mSpriteAnimPlayer.IsPlaying())
            {
                mSpriteAnimPlayer.Play();
            }
            mFloatie = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 0.5f + 0.5f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /*
            Vector2 spriteOffset =
                -new Vector2(kFrameSizeInPixels.X / 2, -kFrameSizeInPixels.Y * 2 / 2) * Camera.kPixelsToUnits;
            
            //*/
            Vector2 spriteOffset =
                //new Vector2(kFrameSizeInPixels.X / 2, kFrameSizeInPixels.Y / 2)
                new Vector2(0, kFrameSizeInPixels.Y)
                * Camera.kPixelsToUnits
                + new Vector2(0, mFloatie)
                ;
            //spriteBatch.Draw(mTexture, spriteOffset, null, new Color(1f, 1f, 1f, 1f),
              //  0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), new Vector2(1f, -1f) * Camera.kPixelsToUnits, SpriteEffects.None, 0f);
            mSpriteAnimPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(
                spriteOffset, 0, Color.White, 1 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
        }

        public static void LoadData(Game myGame)
        {
            mSoulSpriteSheet = new SpriteSheet();
            mSoulSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("enemySprites/Soul"));
            mSoulSpriteSheet.AddSprite(new Vector2(0, 0), kFrameSizeInPixels);
            //
            mSoulAnimation = new SpriteAnimation();
            mSoulAnimation.AddFrame(mSoulSpriteSheet, 0, 0.2f);
            mSoulAnimation.AddFrame(mSoulSpriteSheet, 0, 0.2f, true);
        }

        public bool soulOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            // todo
            return false;
        }
    }
}
