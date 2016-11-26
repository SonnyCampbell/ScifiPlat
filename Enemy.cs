using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAPlatformer
{
    public class Enemy : Entity
    {
        int rangeCounter, direction;
        Vector2 destPosition, origPosition;
        bool flipImage, changeDirection;
        bool isAlive;

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }


        public override void LoadContent(ContentManager Content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(Content, attributes, contents, input);
            rangeCounter = 0;
            direction = 1;
            flipImage = false;
            isAlive = true;
            

            origPosition = position;

            if (direction == 1)
            {
                destPosition.X = origPosition.X + range;
            }
            else
            {
                destPosition.X = origPosition.X - range;
            }

            moveAnimation.IsActive = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            if (isAlive)
            {
                base.Update(gameTime, input, col, layer);

                if (direction == 1)
                {
                    velocity.X = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                }
                else if (direction == 2)
                {
                    velocity.X = -speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                }

                if (activateGravity)
                {
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    velocity.Y = 0;
                }

                position += velocity;

                if (direction == 1 && position.X >= destPosition.X)
                {
                    flipImage = true;
                    direction = 2;
                    destPosition.X = origPosition.X - range;
                }
                else if (direction == 2 && position.X <= destPosition.X)
                {
                    flipImage = false;
                    direction = 1;
                    destPosition.X = origPosition.X + range;
                }

                ssAnimation.Update(gameTime, ref moveAnimation);
                moveAnimation.Position = position;
            }
            else
            {
                UnloadContent();
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (IsAlive)
                moveAnimation.Draw(spritebatch, flipImage);
        }
    }
}
