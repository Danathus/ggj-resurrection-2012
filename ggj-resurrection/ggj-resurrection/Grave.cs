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
    class Grave : GameObject
    {
        // static variables
        static Texture2D mTexture;
        public static Vector2 kFrameSizeInPixels = new Vector2(50, 60); // C# won't let me make this const, but please don't change!

        // instance members
        public DeathWorld mDeathWorld;
        Vector2 mTilePos;
        bool mMoved;

        static private SoundEffect mMoveGraveSnd;
        static private float mMoveGraveSndVolume;

        static private SoundEffect mSoulEmergeSnd;
        static private float mSoulEmergeSndVolume;

        public Grave(World world, Vector2 initPos, Vector2 tilePos)   //this is never called. We need it for physics object
            : base(world, initPos)
        {
            mTilePos = tilePos;

            mFixture = FixtureFactory.AttachRectangle(30 * Camera.kPixelsToUnits, 30 * Camera.kPixelsToUnits,
                100f, // density
                kFrameSizeInPixels / 2 * Camera.kPixelsToUnits, // offset
                new Body(mPhysicsWorld));
            mFixture.Body.CollisionCategories = Category.Cat31;
            mFixture.Body.CollidesWith        = Category.All;// &~Category.Cat31;
            mFixture.CollisionCategories      = Category.Cat31;
            mFixture.CollidesWith             = Category.All ;//& ~Category.Cat31;
            mFixture.Body.OnCollision        += graveOnCollision;
            mFixture.Body.BodyType            = BodyType.Static;

            mFixture.UserData      = "Grave";
            mFixture.Body.UserData = "Grave";

            mFixture.Body.Position = mPosition;

            mMoved = false;

            mMoveGraveSndVolume = 1f;
            mSoulEmergeSndVolume = 1f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteOffset =
                new Vector2(kFrameSizeInPixels.X / 2, kFrameSizeInPixels.Y / 2)
                * Camera.kPixelsToUnits;
            spriteBatch.Draw(mTexture, spriteOffset, null, new Color(1f, 1f, 1f, 1f),
                0f, new Vector2(mTexture.Width / 2, mTexture.Height / 2), new Vector2(1f, -1f) * Camera.kPixelsToUnits, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            // scoot ourselves quickly towards the desired position
            Vector2 desiredPos = (mTilePos - new Vector2(8, 8)) * 50 * Camera.kPixelsToUnits;
            mPosition = (mPosition + desiredPos) / 2;

            // override physics
            mFixture.Body.ResetDynamics();
            mFixture.Body.Rotation = 0;
            mFixture.Body.Position = mPosition;

            // copy physics -> position
            //mPosition = new Vector2(mFixture.Body.Position.X, mFixture.Body.Position.Y); // converts Body.Position (meters) into pixels
        }

        public static void LoadData(Game myGame)
        {
            mTexture = myGame.Content.Load<Texture2D>("Grave");

            mMoveGraveSnd = myGame.Content.Load<SoundEffect>("Audio/moveGrave");
            mSoulEmergeSnd = myGame.Content.Load<SoundEffect>("Audio/soulEmerge");
            
        }

        public bool graveOnCollision(Fixture one, Fixture two, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (two.UserData.ToString() == "Player" || two.Body.UserData.ToString() == "Player")
            {
                // see if a grave is on the other side
                Vector2 direction = one.Body.Position - two.Body.Position;
                if (Vector2.Dot(direction, new Vector2(0, +1)) > 0.7f)
                {
                    direction = new Vector2(0, +1);
                }
                else if (Vector2.Dot(direction, new Vector2(0, -1)) > 0.7f)
                {
                    direction = new Vector2(0, -1);
                }
                else if (Vector2.Dot(direction, new Vector2(-1, 0)) > 0.7f)
                {
                    direction = new Vector2(-1, 0);
                }
                else if (Vector2.Dot(direction, new Vector2(+1, 0)) > 0.7f)
                {
                    direction = new Vector2(+1, 0);
                }
                else { return true; }

                //
                if (mDeathWorld.IsTileOccupied(mTilePos + direction) || mMoved)
                {
                    // yes -- don't allow self to move
                }
                else
                {
                    // no -- allow self to move
                    mMoved = true;
                    mMoveGraveSnd.Play(mMoveGraveSndVolume, 0, 0);


                    // spawn soul
                    mSoulEmergeSnd.Play(mSoulEmergeSndVolume, 0, 0);
                    GetGameWorld().AddGameObject(new Soul(mPhysicsWorld,
                        (mTilePos - new Vector2(8, 8)) * 50 * Camera.kPixelsToUnits)
                        //+ kFrameSizeInPixels/2
                        );

                    // perform move
                    mDeathWorld.SetTileOccupied(mTilePos, false);
                    mTilePos += direction;
                    mDeathWorld.SetTileOccupied(mTilePos, true);
                }
                //
                return true;
            }
            if (two.UserData.ToString() == "Grave"  || two.Body.UserData.ToString() == "Grave")
            {
                return true;
            }

            return false;
        }
    }
}
