using Microsoft.CodeAnalysis;

namespace MessageTemplateCache.Analyzers;
public partial class CreateRequestAnalyzer
{

    private static class Descriptors
    {
        public static readonly DiagnosticDescriptor NoParameters = new(
            id: "MTC0001",
            title: "MessageTemplateCache.CreateRequest() should be called without explicit parameters",
            messageFormat: "MessageTemplateCache.CreateRequest() should be called without explicit parameters",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "MessageTemplateCache.CreateRequest() is generally called with metadata automatically set by `CallerMemberName`, `CallerFilePath`, and `CallerLineNumber`. Providing explicit arguments may lead to unexpected behavior or errors in the message template handling. To ensure the intended functionality and proper metadata handling, invoke the method without passing any arguments.",
            helpLinkUri: "https://github.com/ElectroAttacks/UtilityKit.MessageTemplateCache/blob/master/README.md");

        public static readonly DiagnosticDescriptor InvalidParameters = new(
            id: "MTC0002",
            title: "Invalid parameters in MessageTemplateCache.CreateRequest() method call",
            messageFormat: "Invalid parameters provided in the method call: {0}",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "The parameters provided in the MessageTemplateCache.CreateRequest() method call do not match the expected metadata values (`CallerMemberName`, `CallerFilePath`, and `CallerLineNumber`). This may lead to unexpected behavior or errors in the message template handling. Ensure that the provided parameters match the metadata automatically set by the compiler.",
            helpLinkUri: "https://github.com/ElectroAttacks/UtilityKit.MessageTemplateCache/blob/master/README.md");
    }

}
