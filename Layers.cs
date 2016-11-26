using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAPlatformer
{
    public class Layers
    {
        List<List<List<Vector2>>> tileMap;
        List<List<Vector2>> layer;
        List<Vector2> tileList;

        ContentManager content;
        FileManager fileManager;

        Texture2D tileSet;
        Vector2 tileDimensions;

        int layerNumber;

        public int LayerNumber
        {
            set { layerNumber = value; }
        }

        public Vector2 TileDimensions
        {
            get { return tileDimensions; }
        }

        public void LoadContent(ContentManager content, string mapID)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            fileManager = new FileManager();

            tileList = new List<Vector2>();
            layer = new List<List<Vector2>>();
            tileMap = new List<List<List<Vector2>>>();

            string[] split;

            fileManager.LoadContent("Load/Maps/" + mapID + ".cme",  "Layers");

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "TileSet":
                            tileSet = this.content.Load<Texture2D>("TileSets/" + fileManager.Contents[i][j]);
                            break;
                        case "TileDimensions":
                            split = fileManager.Contents[i][j].Split(',');
                            tileDimensions = new Vector2(int.Parse(split[0]), int.Parse(split[1]));
                            break;
                        case "StartLayer":
                            for (int k = 0; k < fileManager.Contents[i].Count; k++)
                            {
                                split = fileManager.Contents[i][k].Split(',');
                                tileList.Add(new Vector2(int.Parse(split[0]), int.Parse(split[1])));
                            }
                            if (tileList.Count > 0)
                                layer.Add(tileList);

                            tileList = new List<Vector2>();
                            break;
                        case "EndLayer":
                            if (layer.Count > 0)
                            {
                                tileMap.Add(layer);
                                
                            }
                            layer = new List<List<Vector2>>();
                            break;
                    }
                }
               
            }
        }


        public void UnloadContent()
        {
            this.content.Unload();
            tileMap.Clear();
            layer.Clear();
            tileMap.Clear();
            fileManager = null;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < tileMap.Count; k++) //layer number
            {
                for (int i = 0; i < tileMap[k].Count; i++) //vertical position (column number)
                {
                    for (int j = 0; j < tileMap[k][i].Count; j++) //horizontal position (row number)
                    {
                        spriteBatch.Draw(tileSet, new Vector2(j * tileDimensions.X, i * tileDimensions.Y),
                            new Rectangle((int)tileMap[k][i][j].X * (int)tileDimensions.X, (int)tileMap[k][i][j].Y * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), Color.White);
                    }
                }
            }
        }
    }
}
