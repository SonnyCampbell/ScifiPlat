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
    public class Entity
    {
        protected int health, range, direction;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected float speed;
        protected ContentManager content;
        protected List<List<string>> attributes, contents;
        protected Texture2D image;
        protected Vector2 position;
        protected Vector2  velocity, prevPosition, destPosition, origPosition;
        protected float gravity;
        protected bool activateGravity;
        protected bool syncTilePosition;
        protected bool onTile;

        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (direction == 2)
                {
                    destPosition.X = origPosition.X - range;
                }
                else
                {
                    destPosition.X = origPosition.X + range;
                }
            }
        }

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }

        public bool OnTile
        {
            get { return onTile; }
            set { onTile = value; }
        }

        public virtual void LoadContent(ContentManager Content, List<string> attributes, List<string> contents, InputManager input)
        {
            
            this.content = new ContentManager(Content.ServiceProvider, "Content");
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            Vector2 tempFrames = Vector2.Zero;
            direction = 1;


            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i])
                {
                    case "Health":
                        health = int.Parse(contents[i]);
                        break;
                    case "Frames":
                        string[] frames = contents[i].Split(',');
                        tempFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                        break;
                    case "Image":
                        image = this.content.Load<Texture2D>(contents[i]);
                        break;
                    case "Position":
                        string[] pos = contents[i].Split(',');
                        position = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                        break;
                    case "MoveSpeed":
                        speed = float.Parse(contents[i]);
                        break;
                    case "Range":
                        range = int.Parse(contents[i]);
                         break;
                }
            }

            gravity = 100f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;
            if (tempFrames != Vector2.Zero)
            {
                moveAnimation.LoadContent(content, image, "", position, tempFrames);
            }
            else
            {
                moveAnimation.LoadContent(content, image, "", position);
            }
            
            
            
        }

        public virtual void UnloadContent()
        {
            
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input)
        { }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            syncTilePosition = false;
            prevPosition = position;
        }

        public virtual void Draw(SpriteBatch spritebatch)
        { }

        public virtual void OnCollision(Entity e)
        { }

    }
}
