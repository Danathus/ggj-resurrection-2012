using System;

namespace SecretGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SecretGame game = new SecretGame())
            {
                game.Run();
            }
        }
    }
#endif
}

