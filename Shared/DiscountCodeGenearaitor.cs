using System.Text;

namespace DiscountService.Shared
{
    public class DiscountCodeGenerator
    {
        private static readonly Random random = new();

        public static string GenerateDiscountCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder discountCode = new();

            for (int i = 0; i < length; i++)
                discountCode.Append(chars[random.Next(chars.Length)]);

            return discountCode.ToString();
        }
    }
}