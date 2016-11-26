using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAPlatformer
{
    public class ScreenManager
    {
        #region Declaring Variables

        /// <summary>
        /// Creating custome content manager.
        /// </summary>
        ContentManager content;

        /// <summary>
        /// The screen currently being displayed
        /// </summary>
        GameScreen currentScreen;

        /// <summary>
        /// The screen to be added to the top of the stack
        /// </summary>
        GameScreen newScreen;

        /// <summary>
        /// Screen manager instance.
        /// </summary>
        private static ScreenManager instance;

        /// <summary>
        /// Lets you know what screens are open and in what order.
        /// </summary>
        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        /// <summary>
        /// Screens width and height.
        /// </summary>
        Vector2 dimensions;

        bool transition;

        Animation animation = new Animation();
        FadeAnimation fade = new FadeAnimation();
        Texture2D fadeTexture;
        Texture2D nullImage;

        InputManager inputManager;

        #endregion

        #region Properties

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
        }


        #endregion

        #region Main Methods

        public void AddScreen(GameScreen screen, InputManager inputManager) 
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 1.0f;
            fade.Increase = true;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;

            
        }

        public void AddScreen(GameScreen screen, float alpha, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            
            this.inputManager = inputManager;


            if (alpha != 1.0f)
                fade.Alpha = 1.0f - alpha;
            else
                fade.Alpha = alpha;
            fade.Increase = true;

        }

        public void Initialize() 
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
            inputManager = new InputManager();
            
        }

        public void LoadContent(ContentManager Content) 
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content, inputManager);

            nullImage = this.content.Load<Texture2D>("null");
            fadeTexture = this.content.Load<Texture2D>("fade");
            animation.LoadContent(content, fadeTexture, "", Vector2.Zero);
            animation.ScaleX = dimensions.X;
            animation.ScaleY = dimensions.Y;
        }

        public void Update(GameTime gameTime) 
        {
            if (!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime);

            Camera.Instance.Update();
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            currentScreen.Draw(spriteBatch);
            if (transition)
                animation.Draw(spriteBatch);
        }

        public ContentManager Content
        {
            get { return content; }
        }

        #endregion


        #region Private Methods

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime, ref animation);

            if (animation.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, this.inputManager);
                animation.IsActive = true;
            }
            else if (animation.Alpha == 0.0f)
            {
                transition = false;
                animation.IsActive = false;
            }
        }

        #endregion

    }
}
