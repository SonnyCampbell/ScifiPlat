using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAPlatformer
{
    public class Tile
    {
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };

        State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;

        float range;
        int counter;
        bool increase;
        float moveSpeed;
        bool containsEntity;

        Animation animation;

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for(int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
            {
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }
            }

            croppedImage.SetData<Color>(croppedImageData);

            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSheet, Rectangle tileArea)
        {
            this.state = state;
            this.motion = motion;
            this.position = position;
            

            tileImage = CropImage(tileSheet, tileArea);
            range = 50;
            counter = 0;
            increase = true;
            moveSpeed = 100f;
            containsEntity = false;
            velocity = Vector2.Zero;

            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
        }

        public void Update(GameTime gameTime)
        {
            counter++;
            prevPosition = position;

            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }


            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            position += velocity;
            animation.Position = position;

            
            
            
        }


        public void UpdateCollision(ref Entity e)
        {
            FloatRect rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (e.OnTile && containsEntity)
            {
                if (!e.SyncTilePosition)
                {
                    e.Position += velocity;
                    e.SyncTilePosition = true;
                }

                if (e.Rect.Right < rect.Left || e.Rect.Left > rect.Right
                    || Math.Round(e.Rect.Botton, 2) != Math.Round(rect.Top,2) )
                {
                    e.OnTile = false;
                    containsEntity = false;
                    
                    
                }
            }

            if (!e.OnTile)
            {
                e.ActivateGravity = true;
                e.SyncTilePosition = false;
                containsEntity = false;
            }



            if (e.Rect.Intersects(rect) && state == State.Solid)
            {
                FloatRect prevE = new FloatRect(e.PrevPosition.X, e.PrevPosition.Y, e.Animation.FrameWidth, e.Animation.FrameHeight);
                FloatRect prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);


                if (e.Rect.Botton >= rect.Top && prevE.Botton <= prevTile.Top)
                {
                    e.Position = new Vector2(e.Position.X, position.Y - e.Animation.FrameHeight);
                    e.ActivateGravity = false;
                    e.OnTile = true;
                    containsEntity = true;
                }
                else if (e.Rect.Top <= rect.Botton && prevE.Top >= prevTile.Botton)
                {
                    e.Position = new Vector2(e.Position.X, position.Y + Layer.TileDimensions.Y);
                    e.Velocity = new Vector2(e.Velocity.X, 0);
                    e.ActivateGravity = true;
                }
                else if (e.Rect.Right >= rect.Left && prevE.Right <= prevTile.Left)
                {
                    e.Position = new Vector2(position.X - e.Animation.FrameWidth, e.Position.Y);
                    if (e.Direction == 1)
                        e.Direction = 2;
                    else
                        e.Direction = 1;
                }
                else if (e.Rect.Left <= rect.Right && prevE.Left >= prevTile.Right)
                {
                    e.Position = new Vector2(position.X + Layer.TileDimensions.X, e.Position.Y);
                    if (e.Direction == 1)
                        e.Direction = 2;
                    else
                        e.Direction = 1;
                }


            }

            e.Animation.Position = e.Position;
            
        
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
