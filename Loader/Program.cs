
//using var game = new LabRat.LabRatGame();
//game.Run();

using System;

namespace LabRat
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new LabRatGame())
                game.Run();
        }
    }
}