using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    class MainProgram
    {
        int screenWidth;
        int screenHeight;

        public void Start()
        {
            Raylib.InitWindow(800, 600, "Avaruuspeli");

            screenWidth = Raylib.GetScreenWidth();
            screenHeight = Raylib.GetScreenHeight();

            while (Raylib.WindowShouldClose() == false)
            {
                Update();
                Draw();
            }
            Raylib.CloseWindow();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.EndDrawing();
        }


        /// <summary>
        /// Makes the movement for player
        /// </summary>
        /// <param name="transform"></param>
        public void Movement(Transform transform)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                transform.position.X -= transform.speed * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                transform.position.X += transform.speed * Raylib.GetFrameTime();
            }
        }

        /// <summary>
        /// Keeps player in the game area
        /// </summary>
        /// <param name="transfrom"></param>
        /// <param name="collision"></param>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public void KeepInsideScreen(Transform transfrom, Collision collision, int width, int height)
        {
            float x = transfrom.position.X;
            transfrom.position.X = Math.Clamp(x, 0, width - collision.size / 2);
            /*
            float y = transfrom.position.Y;
            transfrom.position.Y = Math.Clamp(y, 0, height - collision.size / 2);
            */
        }
    }
}
