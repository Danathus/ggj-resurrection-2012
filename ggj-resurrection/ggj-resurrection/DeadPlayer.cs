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
    class DeadPlayer : GameObject
    {
        // static data
        static Vector2 kFrameSizeInPixels = new Vector2(30, 50); // C# won't let me make this const, but please don't change!
        static SpriteSheet mBlinkingSpriteSheet;
        static SpriteAnimation mBlinkingAnimation;

        // member data
        SpriteAnimationPlayer mSpriteAnimPlayer;

        public DeadPlayer(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mSpriteAnimPlayer = new SpriteAnimationPlayer();
            mSpriteAnimPlayer.SetAnimationToPlay(mBlinkingAnimation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset =
                -new Vector2(kFrameSizeInPixels.X / 2, -kFrameSizeInPixels.Y * 2 / 2) * Camera.kPixelsToUnits;
            mSpriteAnimPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(
                mPosition + spriteOffset, 0, Color.White, 1 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
        }

        public override void Update(GameTime gameTime)
        {
        }

        public static void LoadData(Game myGame)
        {
            // load all static data here
            mBlinkingSpriteSheet = new SpriteSheet();
            mBlinkingSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("CharSprite/boyStandingStill"));
            for (int i = 0; i < 2; ++i)
            {
                mBlinkingSpriteSheet.AddSprite(new Vector2(i * kFrameSizeInPixels.X, 0), kFrameSizeInPixels);
            }
            //
            mBlinkingAnimation = new SpriteAnimation();
            mBlinkingAnimation.AddFrame(mBlinkingSpriteSheet, 1, 1.0f);
            mBlinkingAnimation.AddFrame(mBlinkingSpriteSheet, 0, 0.1f);
        }
    }
}
