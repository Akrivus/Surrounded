using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game.Render
{
    public class AnimatedSprite : Sprite
    {
        // These keep track of frames.
        private Clock FrameTimer;
        private int NumberOfFrames = 6;
        private int FrameRate;
        private int FrameHeight;
        private int FrameWidth;

        // These track what frame the sprite should be on.
        public int Direction;
        public int Step;

        // Class constructor.
        public AnimatedSprite(Texture texture, int frameRate) : base(texture)
        {
            this.FrameRate = frameRate;
            this.FrameWidth = Convert.ToInt32(texture.Size.X / this.NumberOfFrames);
            this.FrameHeight = Convert.ToInt32(texture.Size.Y / 4);
            this.Origin = new Vector2f(this.FrameWidth / 2, this.FrameWidth / 2);
            this.FrameTimer = new Clock();
        }

        // Updates sprite.
        public void Update(Vector2f position, bool walking, bool attacking, bool dead, int attackSpeed = 1000)
        {
            if (dead)
            {
                this.Step = 5;
            }
            else if (attacking)
            {
                if (this.FrameTimer.ElapsedTime.AsMilliseconds() > attackSpeed)
                {
                    this.Step = 0;
                }
                else
                {
                    this.Step = 4;
                }
            }
            else if (walking)
            {
                if (this.FrameTimer.ElapsedTime.AsMilliseconds() > this.FrameRate)
                {
                    ++this.Step;
                    if (this.Step > 3)
                    {
                        this.Step = 1;
                    }
                    this.FrameTimer.Restart();
                }
            }
            else
            {
                this.Step = 0;
            }
            this.Position = position;
            this.TextureRect = new IntRect(this.Step * this.FrameWidth, this.Direction * this.FrameHeight, this.FrameWidth, this.FrameHeight);
        }

        // Gets the sprite's corners.
        public Vector2f[] GetCorners(Vector2f position)
        {
            // Return all four corners and the origin.
            return new Vector2f[] {
                new Vector2f(position.X - (this.FrameWidth / 2), position.Y - (this.FrameHeight / 2)),
                new Vector2f(position.X - (this.FrameWidth / 2), position.Y + (this.FrameHeight / 2)),
                new Vector2f(position.X + (this.FrameWidth / 2), position.Y - (this.FrameHeight / 2)),
                new Vector2f(position.X + (this.FrameWidth / 2), position.Y + (this.FrameHeight / 2)),
                new Vector2f(position.X, position.Y)
            };
        }
    }
}
