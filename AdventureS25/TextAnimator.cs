using System;
using System.Threading;

namespace AdventureS25
{
    public static class TextAnimator
    {
        private static int delayBetweenChars = 30; // milliseconds
        private static int delayForPeriods = 500; // milliseconds
        private static int delayForCommas = 150; // milliseconds
        private static int delayForExclamation = 250; // milliseconds
        private static int delayForQuestion = 250; // milliseconds
        
        /// <summary>
        /// Writes text to the console letter by letter with a delay between each letter
        /// and a longer delay for periods
        /// </summary>
        /// <param name="text">The text to animate</param>
        public static void AnimateText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine();
                return;
            }
            
            for (int i = 0; i < text.Length; i++)
            {
                // Check if Enter was pressed to skip animation
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        // Write the remaining text instantly
                        Console.Write(text.Substring(i));
                        Console.WriteLine();
                        return;
                    }
                }
                
                char c = text[i];
                Console.Write(c);
                
                // Use a longer delay for periods
                if (c == '.')
                {
                    Thread.Sleep(delayForPeriods);
                }
                else if (c == ',')
                {
                    Thread.Sleep(delayForCommas);
                }
                else if (c == '!' || c == '?')
                {
                    Thread.Sleep(c == '!' ? delayForExclamation : delayForQuestion);
                }
                else
                {
                    Thread.Sleep(delayBetweenChars);
                }
            }
            
            // Add a newline at the end
            Console.WriteLine();
        }
        
        /// <summary>
        /// Initializes the TextAnimator with default settings
        /// </summary>
        public static void Initialize()
        {
            // Default settings are already set as static field initializers
            // This method exists to provide a clear initialization point
            // and could be expanded in the future
            Console.Clear();
        }
        
        /// <summary>
        /// Sets the delay between characters in milliseconds
        /// </summary>
        /// <param name="milliseconds">Delay in milliseconds</param>
        public static void SetCharDelay(int milliseconds)
        {
            delayBetweenChars = milliseconds;
        }
        
        /// <summary>
        /// Sets the delay for periods in milliseconds
        /// </summary>
        /// <param name="milliseconds">Delay in milliseconds</param>
        public static void SetPeriodDelay(int milliseconds)
        {
            delayForPeriods = milliseconds;
        }
        
        /// <summary>
        /// Sets the delay for commas in milliseconds
        /// </summary>
        /// <param name="milliseconds">Delay in milliseconds</param>
        public static void SetCommaDelay(int milliseconds)
        {
            delayForCommas = milliseconds;
        }
        
        /// <summary>
        /// Sets the delay for exclamation marks in milliseconds
        /// </summary>
        /// <param name="milliseconds">Delay in milliseconds</param>
        public static void SetExclamationDelay(int milliseconds)
        {
            delayForExclamation = milliseconds;
        }
        
        /// <summary>
        /// Sets the delay for question marks in milliseconds
        /// </summary>
        /// <param name="milliseconds">Delay in milliseconds</param>
        public static void SetQuestionDelay(int milliseconds)
        {
            delayForQuestion = milliseconds;
        }
    }
}
