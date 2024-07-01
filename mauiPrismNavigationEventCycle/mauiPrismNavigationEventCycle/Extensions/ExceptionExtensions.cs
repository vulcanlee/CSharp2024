using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiPrismNavigationEventCycle.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullExceptionMessage(this Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            var sb = new StringBuilder();
            GetExceptionDetails(sb, ex);
            return sb.ToString();
        }

        private static void LogExceptionDetails(StringBuilder sb, Exception ex,
            int level = 0, string leadSpace = "")
        {
            if (ex is AggregateException aggregateException)
            {
                sb.AppendLine($"{leadSpace}[Aggregation Exceptions]");
            }
            sb.AppendLine($"{leadSpace}Exception Level {level}:");
            sb.AppendLine($"{leadSpace}Message: {ex.Message}");
            sb.AppendLine($"{leadSpace}Type: {ex.GetType()}");
            sb.AppendLine($"{leadSpace}Stack Trace: {ex.StackTrace}");
        }
        private static void GetExceptionDetails(StringBuilder sb, Exception ex, int level = 0)
        {
            if (ex == null) return;

            var levelSpace = "".PadRight(level * 3);

            //sb.AppendLine($"{levelSpace}Exception Level {level}:");
            //sb.AppendLine($"{levelSpace}Message: {ex.Message}");
            //sb.AppendLine($"{levelSpace}Type: {ex.GetType()}");
            //sb.AppendLine($"{levelSpace}Stack Trace: {ex.StackTrace}");

            LogExceptionDetails(sb, ex, level, levelSpace);

            if (ex is AggregateException aggregateException)
            {
                sb.AppendLine($"{levelSpace}Inner Exceptions:");
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    sb.AppendLine();
                    GetExceptionDetails(sb, innerException, level + 1);
                }
            }
            else if (ex.InnerException != null)
            {
                sb.AppendLine($"{levelSpace}Inner Exception:");
                sb.AppendLine();
                GetExceptionDetails(sb, ex.InnerException, level + 1);
            }
        }
    }
}
