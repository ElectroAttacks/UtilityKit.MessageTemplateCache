using MessageTemplateCache.Attributes;

namespace MessageTemplateCache;


public static class TestClass
{

    [MessageTemplate("Hello World or")]
    [MessageTemplate("Hello World or", "0xDCAFFD72")]
    public static string HelloMethod(string name) => MessageTemplateCache.CreateRequest("D:\\VS 2022\\UtilityKit.MessageTemplateCache\\MessageTemplateCache\\TestClass.cs", "HelloMethod", 18).GetMessage(name);


    [MessageTemplate("Hello World or, {0}...")]
    [MessageTemplate("Hello World or, {0}...", "0x339C4714")]
    public static string BeyMethod(string name) => MessageTemplateCache.CreateRequest("D:\\VS 2022\\UtilityKit.MessageTemplateCache\\MessageTemplateCache\\TestClass.cs", "BeyMethod", 23).WithIdentifier("").GetMessage(name);


}

