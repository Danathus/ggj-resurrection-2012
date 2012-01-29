﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    class EvilCow : Monster
    {
        //static private Texture2D mTexture;

        static SpriteSheet lightningCowSpriteSheet;
        static SpriteAnimation lightningCowAnimation;

        SpriteAnimationPlayer lightningPlayer;

        Color tempColor = Color.White;


        public EvilCow(World world, Vector2 initPos, Player player)
            : base(world, initPos, player)
        {

            mPlayer = player;
           lightningPlayer = new SpriteAnimationPlayer();
            lightningPlayer.SetAnimationToPlay(lightningCowAnimation);

            mFixture = FixtureFactory.AttachRectangle(90f * Camera.kPixelsToUnits, 90f * Camera.kPixelsToUnits, .0125f, new Vector2(0, 0), new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.CollisionCategories = Category.Cat3;
            mFixture.Body.OnCollision += monsterOnCollision;

            //Correct for meters vs pixels
            mFixture.Body.Position = new Vector2(mPosition.X, mPosition.Y);
            //mFixture.Body.UserData = "Monster";
            mFixture.Body.UserData = "Monster";
            mFixture.UserData = "Monster";
            

            /*
            //mBody = BodyFactory.CreateRectangle(mPhysicsWorld, 3f, 3f, .0125f);
            
            mFixture = FixtureFactory.AttachRectangle(3f, 3f, .0125f, new Vector2(0,0), new Body(mPhysicsWorld));
            mFixture.Body.BodyType = BodyType.Dynamic;
            mFixture.UserData = "EvilCow";
            mFixture.Body.UserData = "EvilCow";
            mFixture.CollisionCategories = Category.Cat3;
            mFixture.Body.OnCollision += monsterOnCollision;*/

        }

        ~EvilCow()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset = -new Vector2(50, -50) * Camera.kPixelsToUnits;
            lightningPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(mFixture.Body.Position + spriteOffset, 0f, Color.White, 1 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
            //float proximity = Vector2.Distance(mBody.Position, mPlayer.GetPosition());
           // spriteBatch.Draw(mTexture, mFixture.Body.Position, null, tempColor, 0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), Camera.kPixelsToUnits, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (!lightningPlayer.IsPlaying())
            {
                lightningPlayer.Play();
            }

            lightningPlayer.Update(gameTime);

            base.Update(gameTime);

        }

        new public static void LoadData(Game myGame)
        {
            lightningCowSpriteSheet = new SpriteSheet();
            lightningCowSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("Enemies/LightningCow5fps"));
            lightningCowAnimation = new SpriteAnimation();

            for (int i = 0; i < 4; i++)
            {
                lightningCowSpriteSheet.AddSprite(new Vector2(i * 100, 0), new Vector2(100, 100));
                lightningCowAnimation.AddFrame(lightningCowSpriteSheet, i, .1f);
            }
            //mTexture = myGame.Content.Load<Texture2D>("enemySprites/evilCow");
        }
    }

}
