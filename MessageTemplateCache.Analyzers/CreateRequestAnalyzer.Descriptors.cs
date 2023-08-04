using Microsoft.CodeAnalysis;

namespace MessageTemplateCache.Analyzers;
public partial class CreateRequestAnalyzer
{

    private static class Descriptors
    {

        public static readonly DiagnosticDescriptor CreateRequestMustBeCalledWithoutParameters = new(
            id: "MTC0001",
            title: "CreateRequest must be called without parameters",
            messageFormat: "CreateRequest must be called without parameters",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "CreateRequest must be called without parameters",
            helpLinkUri: "");
    }

}
