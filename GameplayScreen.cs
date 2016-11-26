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
    public class GameplayScreen : GameScreen
    {

        EntityManager player, enemies;
        Map map;

        public override void LoadContent(ContentManager Content, InputManager input)
        {
            base.LoadContent(Content, input);
            player = new EntityManager();
            enemies = new EntityManager();
            map = new Map();

            map.LoadContent(content, map, "Map1");
            player.LoadContent("Player", content, "Load/Player.cme", "", input);
            enemies.LoadContent("Enemy", content, "Load/Enemies.cme", "Level1", input);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            enemies.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Update(gameTime, map);
            enemies.Update(gameTime, map);
            map.Update(gameTime);

            Entity e;
            for (int i = 0; i < player.Entities.Count; i++)
            {
                e = player.Entities[i];
                map.UpdateCollision(ref e);
                player.Entities[i] = e;
            }
            for (int i = 0; i < enemies.Entities.Count; i++)
            {
                e = enemies.Entities[i];
                map.UpdateCollision(ref e);
                enemies.Entities[i] = e;
            }

            player.EntityCollision(enemies);
            player.BulletCollision(enemies);

            
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            map.Draw(spritebatch);
            player.Draw(spritebatch);
            enemies.Draw(spritebatch);
        }
    }

    
}
