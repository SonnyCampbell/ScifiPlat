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
    public class Player : Entity
    {

        float jumpSpeed = 1500f;
        float bulletSpeet = 400f;
        FileManager fileManager;



        List<Vector2> bullets;
        List<int> bulletDirections;
        Texture2D bulletImage;
        

        public override void  LoadContent(ContentManager Content, List<string> attributes, List<string> contents, InputManager input)
        {
 	        base.LoadContent(Content, attributes, contents, input);

            string[] saveAttribute = { "PlayerPosition" };
            string[] saveContent = { position.X.ToString() + "," + position.Y.ToString() };

            fileManager = new FileManager();
            fileManager.SaveContent("Load/Maps/map1.cme", saveAttribute, saveContent, "");

            bullets = new List<Vector2>();
            bulletDirections = new List<int>();
            bulletImage = content.Load<Texture2D>("bullet");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            base.Update(gameTime, input, col, layer);
            moveAnimation.DrawColor = Color.White;
            moveAnimation.IsActive = true;
            if (input.KeyDown(Keys.Right, Keys.D))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                velocity.X = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                direction = 1;

            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X = -speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                direction = 2;
            }
            else
            {
                moveAnimation.IsActive = false;
                velocity.X = 0;
            }

            if (input.KeyDown(Keys.Up, Keys.W) && !activateGravity)
            {
                velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                activateGravity = true;
            }


            if (activateGravity)
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                velocity.Y = 0;

            position += velocity;
            if (position.X <= 0)
                position.X = 0;

            if (position.Y >= ScreenManager.Instance.Dimensions.Y)
                position = new Vector2(0, 0);

            if (input.KeyDown(Keys.Space))
            {
                bullets.Add(position);
                bulletDirections.Add(direction);
            }

            for (int i = 0; i < bullets.Count; i++)
            {

                float x = bullets[i].X;
                if (bulletDirections[i] == 1)
                {
                    x += bulletSpeet * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (bulletDirections[i] == 2)
                {
                    x -= bulletSpeet * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                bullets[i] = new Vector2(x, bullets[i].Y);
            }



            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);


            Camera.Instance.SetFocalPoint(new Vector2(position.X, ScreenManager.Instance.Dimensions.Y / 2));

            
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            moveAnimation.Draw(spritebatch);

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].X <= Camera.Instance.Position.X + ScreenManager.Instance.Dimensions.X  &&
                    bullets[i].X >= Camera.Instance.Position.X )
                {
                    spritebatch.Draw(bulletImage, bullets[i], Color.White);
                }
                else
                {
                    bullets.RemoveAt(i);
                    bulletDirections.RemoveAt(i);
                }
                
            }
        }


        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                health--;
                fileManager = new FileManager();
                fileManager.LoadContent("Load/Maps/map1.cme", "");

                for (int i = 0; i < fileManager.Attributes.Count; i++)
                {
                    for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                    { 
                        switch (fileManager.Attributes[i][j])
                        {
                            case "PlayerPosition":
                                string[] split = fileManager.Contents[i][j].Split(',');
                                position = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
                                break;
                        }
                    }
                }
            }
        }

        public void BulletCollision(Entity e)
        { 
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    FloatRect bulletRect = new FloatRect(bullets[i].X, bullets[i].Y, bulletImage.Width, bulletImage.Height);
                    if (bulletRect.Intersects(e.Rect))
                    {
                        Enemy theEnemy = (Enemy)e;
                        theEnemy.IsAlive = false;
                    }
                }
            }
        }
    }
}
