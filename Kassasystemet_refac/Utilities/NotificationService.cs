namespace Kassasystemet_refac
{
    public static class NotificationService
    {
        public static void ShowError(
            string message)
        {            
            Console.ForegroundColor =
                ConsoleColor.Red;

            CenterConsoleOutput
                .CenterTextToWindow(message);

            Console.ResetColor();
        }


        public static void ShowSuccessHeader(string header)
        {
            Console.Clear();
            Console.ForegroundColor =
                ConsoleColor.Green;

            CenterConsoleOutput.CenterTextToWindow(
                header);

            Console.ResetColor();

            Console.WriteLine();
        }
    }
}
