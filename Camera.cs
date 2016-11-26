using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace XNAPlatformer
{
    public class Camera
    {
        private static Camera instance;
        Vector2 position;
        Matrix viewMatrix;

        public static Camera Instance
        {
            get 
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public Vector2 Position
        {
            get { return position; }
        }


        public void SetFocalPoint(Vector2 focalPosition)
        {
            position = new Vector2(focalPosition.X - ScreenManager.Instance.Dimensions.X / 2, focalPosition.Y - ScreenManager.Instance.Dimensions.Y / 2);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X + ScreenManager.Instance.Dimensions.X >= Map.mapWidth)
                position.X = Map.mapWidth - ScreenManager.Instance.Dimensions.X;
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
