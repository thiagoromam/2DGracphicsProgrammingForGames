using System;
using Core;

namespace GeneralTest.Xna
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
#endif
}

