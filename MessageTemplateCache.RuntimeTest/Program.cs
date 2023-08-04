using UtilityKit.MessageTemplateCache;
using UtilityKit.MessageTemplateCache.Attributes;

internal class Program
{
    [MessageTemplate("Test Template {0}")]
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine(TestClass.HelloMethod("John"));
        Console.WriteLine(TestClass.BeyMethod("John"));
        Console.WriteLine(MessageTemplateCache.CreateRequest().GetTemplate());
    }
}