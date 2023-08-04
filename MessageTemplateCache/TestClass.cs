﻿using MessageTemplateCache.Attributes;

namespace MessageTemplateCache;


public static class TestClass
{

    [MessageTemplate("Hello World or, {0}!")]
    [MessageTemplate("Hello World or, {0}...", "Version2")]
    public static string HelloMethod(string name) => MessageTemplateCache.CreateRequest("D:\\VS 2022\\UtilityKit.MessageTemplateCache\\MessageTemplateCache\\TestClass.cs", "HelloMethod", 11).GetMessage(name);

    [MessageTemplate("See ya soon, {0}!")]
    [MessageTemplate("See ya soon, {0}...", "Version2")]
    public static string BeyMethod(string name) => MessageTemplateCache.CreateRequest("").WithIdentifier("").GetMessage(name);
}

