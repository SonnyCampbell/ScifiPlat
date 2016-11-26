using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAPlatformer
{
    public class Animation
    {
        private Texture2D image;
        private string text;
        private SpriteFont font;
        private Color color, drawColor;
        private Rectangle sourceRect;
        private float rotation, scaleY, scaleX, axis;
        private Vector2 origin, position;
        private ContentManager content;
        private bool isActive;
        private float alpha;        

        Vector2 frames;
        Vector2 currentFrame;

        #region Properties

        public Color DrawColor
        {
            set { drawColor = value; }
            get { return drawColor; }
        }

        public Vector2 Frames
        {
            set { frames = value; }
            get { return frames; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return (int)(image.Width) / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return (int)(image.Height) / (int)frames.Y; }
        }


        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Texture2D Image
        {
            get { return image; }
        }

        #endregion


        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Font1");
                color = new Color(114, 77, 255);
            }
            if (image != null)
                sourceRect = new Rectangle(0, 0, (int)(image.Width), (int)(image.Height));

            rotation = 0.0f;
            axis = 0.0f;
            scaleY = 1.0f;
            scaleX = 1.0f;
            isActive = false;
            alpha = 1.0f;
            drawColor = Color.White;

            frames = new Vector2(1, 1);
            currentFrame = Vector2.Zero;
            if (image != null)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position, Vector2 frameVec)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Font1");
                color = new Color(114, 77, 255);
            }
            if (image != null)
                sourceRect = new Rectangle(0, 0, (int)(image.Width), (int)(image.Height));

            rotation = 0.0f;
            axis = 0.0f;
            scaleY = 1.0f;
            scaleX = 1.0f;
            isActive = false;
            alpha = 1.0f;
            drawColor = Color.White;

            frames = frameVec;
            currentFrame = Vector2.Zero;
            if (image != null)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }


        public virtual void UnloadContent()
        {
            content.Unload();
            text = string.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
            
        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {
        
        }

        public virtual void Draw(SpriteBatch spriteBatch) 
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(image, position + origin, sourceRect, drawColor * alpha, rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0.0f);
            }

            if (text != string.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0.0f);
            }
             

        }

        public virtual void Draw(SpriteBatch spriteBatch, bool flipDirection)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                if (!flipDirection)
                { 
                    spriteBatch.Draw(image, position + origin, sourceRect, drawColor * alpha, rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0.0f);
                }
                else
                { 
                    spriteBatch.Draw(image, position + origin, sourceRect, Color.White * alpha, rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.FlipHorizontally, 0.0f); 
                }
                
            }

            if (text != string.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0.0f);
            }


        }  
        

    }
}
