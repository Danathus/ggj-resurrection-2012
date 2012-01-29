using System;
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
    public class Player : GameObject
    {
        // static data
        static SpriteSheet mBlinkingSpriteSheet, mRunningSouthSpriteSheet, mRunningSidewaysSpriteSheet, mRunningNorthSpriteSheet;
        static SpriteAnimation mBlinkingAnimation,
            mRunningNorthAnimation, mRunningSouthAnimation,
            mRunningEastAnimation,  mRunningWestAnimation;
        static Texture2D mHackSmoke;

        // member data
        SpriteAnimationPlayer mSpriteAnimPlayer;
        float mMaxSpeed = 5;
        Color tempColor = Color.YellowGreen;
        List<SwordSlash> bats = new List<SwordSlash>();

        KeyboardState mCurrKeyboardState, mPrevKeyboardState;
        GamePadState mCurrControllerState, mPrevControllerState;

        public Player(World world, Vector2 initPos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mRadius = 1f;

            //mBody = BodyFactory.CreateRectangle(mPhysicsWorld, 1f, 1f, 1f);
           // mBody.BodyType = BodyType.Dynamic;

            mFixture = FixtureFactory.AttachRectangle(1f, 1f, 1f, new Vector2(0f, 0f), new Body(mPhysicsWorld));
            mFixture.Body.CollisionCategories = Category.Cat1;
            mFixture.Body.CollidesWith = Category.All & ~Category.Cat1 & ~Category.Cat2;
            mFixture.CollisionCategories = Category.Cat1;
            mFixture.CollidesWith = Category.All & ~Category.Cat1 & ~Category.Cat2;
            mFixture.Body.OnCollision += playerOnCollision;
            mFixture.Body.BodyType = BodyType.Dynamic;

            mFixture.UserData = "Player";
            mFixture.Body.UserData = "Player";
            //mBody.UserData = "Player";

            mSpriteAnimPlayer = new SpriteAnimationPlayer();
            mSpriteAnimPlayer.SetAnimationToPlay(mBlinkingAnimation);
        }

        ~Player()
        {
        }

        public bool playerOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (two.UserData.ToString() == "Sword" || two.Body.UserData.ToString() == "Sword")
            {
                tempColor = Color.Red;
                return false;
            }
            
            tempColor = Color.Red;
            return true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset =
                -new Vector2(30, -50) * Camera.kPixelsToUnits;
            mSpriteAnimPlayer.Draw(spriteBatch, new SpriteSheet.SpriteRenderingParameters(
                mPosition + spriteOffset, 0, Color.White, 2 * new Vector2(Camera.kPixelsToUnits, -Camera.kPixelsToUnits)));
        }

        public override void Update(GameTime gameTime)
        {
            mFixture.Body.ResetDynamics();
            tempColor = Color.YellowGreen;
            mPrevKeyboardState = mCurrKeyboardState;
            mCurrKeyboardState = Keyboard.GetState();
            mPrevControllerState = mCurrControllerState;
            mCurrControllerState = GamePad.GetState(PlayerIndex.One);

            Vector2 direction = DetermineDesiredDirection();

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

            // choose animation
            SpriteAnimation desiredAnim = mBlinkingAnimation; // the default
            if (Vector2.Dot(direction, new Vector2(0, +1)) > 0.5f)
            {
                desiredAnim = mRunningNorthAnimation;
            }
            else if (Vector2.Dot(direction, new Vector2(0, -1)) > 0.5f)
            {
                desiredAnim = mRunningSouthAnimation;
            }
            else if (Vector2.Dot(direction, new Vector2(-1, 0)) > 0.5f)
            {
                desiredAnim = mRunningWestAnimation;
            }
            else if (Vector2.Dot(direction, new Vector2(+1, 0)) > 0.5f)
            {
                desiredAnim = mRunningEastAnimation;
            }

            // now apply the animation
            if (mSpriteAnimPlayer.GetAnimationToPlay() != desiredAnim)
            {
                mSpriteAnimPlayer.SetAnimationToPlay(desiredAnim);
            }

            Vector2 rightStick = mCurrControllerState.ThumbSticks.Right;
            rightStick.Y *= 1f;

            if (mCurrControllerState.IsConnected && rightStick.Length() > .25)
            {
                if (bats.Count <= 4)
                {
                    Vector2 offset2d = mFixture.Body.Position + rightStick * 1.5f;
                    //Vector3 offset3d = new Vector2(offset2d.X, offset2d.Y, 0);
                    SwordSlash newSwordSlash = new SwordSlash(mPhysicsWorld, offset2d);
                    newSwordSlash.setRotation(rightStick);
                    newSwordSlash.SetPosition(offset2d); // it's lame, but this set after-the-fact may be necessary
                    newSwordSlash.SetVelocity(mFixture.Body.LinearVelocity);
                    bats.Add(newSwordSlash);
                    GetGameWorld().AddGameObject(newSwordSlash);
                }
                else
                {
                    SwordSlash apply = bats.ElementAt(0);
                    apply.setAngularVelocity(bats.ElementAt(2), bats.ElementAt(1));
                    bats.RemoveAt(0);

                   /* bats.ForEach(delegate(SwordSlash curr)
                    {
                        if (curr.isTimedOut())
                        {
                            bats.Remove(curr);
                        }

                    });*/
                    //bats.RemoveAt(0);
                }
            }

            const float speed = 300.0f;
            //mPosition += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition = new Vector2(mFixture.Body.Position.X, mFixture.Body.Position.Y);       //converts Body.Position (meters) into pixels

            // djmc animation test
            if (!mSpriteAnimPlayer.IsPlaying())
            {
                mSpriteAnimPlayer.Play();
            }
            mSpriteAnimPlayer.Update(gameTime);
            // djmc animation test

            if ( (mCurrKeyboardState.IsKeyDown(Keys.X) && !mPrevKeyboardState.IsKeyDown(Keys.X)) ||
                 (mCurrControllerState.Buttons.X == ButtonState.Pressed && mPrevControllerState.Buttons.X != ButtonState.Pressed)
                )
            {
                GetGameWorld().AddGameObject(new Particle(mHackSmoke, mPosition, 1.0f));
            }
        }

        // read input state and return current direction we want to move this frame
        private Vector2 DetermineDesiredDirection()
        {
            Vector2 direction = new Vector2(0, 0);

            if (mCurrControllerState.IsConnected)
            {
                direction.X += mCurrControllerState.DPad.Right - mCurrControllerState.DPad.Left;
                direction.Y += mCurrControllerState.DPad.Up    - mCurrControllerState.DPad.Down;
                if (direction.Length() < .065f)
                {
                    direction.X = mCurrControllerState.ThumbSticks.Left.X;
                    direction.Y = mCurrControllerState.ThumbSticks.Left.Y;

                    direction.Y *= 1f;
                }
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Right))
            {
                direction.X += +1f;
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Left))
            {
                direction.X += -1f;
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Up))
            {
                direction.Y += +1f;
            }

            if (mCurrKeyboardState.IsKeyDown(Keys.Down))
            {
                direction.Y += -1f;
            }

            return direction;
        }

        public static void LoadData(Game myGame)
        {
            // load all static data here
            mBlinkingSpriteSheet = new SpriteSheet();
            mBlinkingSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("CharSprite/boyStandingStill"));
            for (int i = 0; i < 2; ++i)
            {
                mBlinkingSpriteSheet.AddSprite(new Vector2(i * 30, 0), new Vector2(30, 50));
            }
            //
            mBlinkingAnimation = new SpriteAnimation();
            mBlinkingAnimation.AddFrame(mBlinkingSpriteSheet, 1, 1.0f);
            mBlinkingAnimation.AddFrame(mBlinkingSpriteSheet, 0, 0.1f);

            mRunningSouthSpriteSheet = new SpriteSheet();
            mRunningSouthSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("CharSprite/boyRunningFoward5fps"));
            mRunningNorthSpriteSheet = new SpriteSheet();
            mRunningNorthSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("CharSprite/boyRunningBackward5fps"));
            mRunningSidewaysSpriteSheet = new SpriteSheet();
            mRunningSidewaysSpriteSheet.SetTexture(myGame.Content.Load<Texture2D>("CharSprite/boyRunningSideways5fps"));
            mRunningSouthAnimation = new SpriteAnimation();
            mRunningEastAnimation  = new SpriteAnimation();
            mRunningWestAnimation  = new SpriteAnimation();
            mRunningNorthAnimation = new SpriteAnimation();
            for (int i = 0; i < 4; ++i)
            {
                mRunningSouthSpriteSheet.AddSprite(   new Vector2(i * 30, 0), new Vector2(30, 50));
                mRunningSidewaysSpriteSheet.AddSprite(new Vector2(i * 30, 0), new Vector2(30, 50));
                mRunningNorthSpriteSheet.AddSprite(   new Vector2(i * 30, 0), new Vector2(30, 50));
                mRunningSouthAnimation.AddFrame(mRunningSouthSpriteSheet,   i, 0.1f);
                mRunningEastAnimation.AddFrame(mRunningSidewaysSpriteSheet, i, 0.1f, true); // flip this one
                mRunningWestAnimation.AddFrame(mRunningSidewaysSpriteSheet, i, 0.1f);
                mRunningNorthAnimation.AddFrame(mRunningNorthSpriteSheet,   i, 0.1f);
            }

            mHackSmoke = myGame.Content.Load<Texture2D>("Particles/SmokeParticleEffectSprite");
        } // end LoadData
    } // end Player
} // namespace ggj_resurrection
