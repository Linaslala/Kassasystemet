namespace Kassasystemet_refac
{
    public static class NotificationService
    {
        public static void ShowError(
            string message)
        {
            Console.Clear();
            Console.ForegroundColor =
                ConsoleColor.Red;

            CenterConsoleOutput
                .CenterTextToWindow(message);

            Console.ResetColor();

            ValidatedConsoleInput
                .PauseCentered();
        }
    }
}
