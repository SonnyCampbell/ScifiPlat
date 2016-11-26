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
    public class SplashScreen : GameScreen
    {

        KeyboardState keyState;
        SpriteFont font;
        List<Animation> animation;
        List<Texture2D> images;

        FadeAnimation FAnimation;

        FileManager fileManager;

        int imageNumber;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");

            imageNumber = 0;
            fileManager = new FileManager();
            animation = new List<Animation>();
            FAnimation = new FadeAnimation();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", "");

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    { 
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(fileManager.Contents[i][j]));
                            animation.Add(new FadeAnimation());
                            break;
                    }
                }
            }

            for (int i = 0; i < animation.Count; i++)
            {
                animation[i].ScaleY = (ScreenManager.Instance.Dimensions.Y / images[i].Height);
                animation[i].ScaleX = (ScreenManager.Instance.Dimensions.X / images[i].Width);
                animation[i].LoadContent(content, images[i], "", new Vector2((images[i].Width / 2) * animation[i].ScaleX - (images[i].Width / 2), (images[i].Height / 2) * animation[i].ScaleY - (images[i].Height / 2)));

                animation[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;

        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            Animation a = animation[imageNumber];
            FAnimation.Update(gameTime, ref a);
            animation[imageNumber] = a;

            if (animation[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber >= animation.Count - 1 || inputManager.KeyPressed(Keys.Z))
            {
                imageNumber = animation.Count - 1;
                if (animation[imageNumber].Alpha != 1.0f)
                    ScreenManager.Instance.AddScreen(new TitleScreen(), animation[imageNumber].Alpha, inputManager);
                else
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(font, "Splash Screen", new Vector2(100, 100), Color.Black);

            animation[imageNumber].Draw(spriteBatch);
        }

    }
}
