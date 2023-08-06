using Microsoft.CodeAnalysis;

namespace MessageTemplateCache.Analyzers.Resources;

internal static class DiagnosticRules
{

    public const string _helpLinkUri = "https://github.com/ElectroAttacks/UtilityKit.MessageTemplateCache/wiki/Help";


    public static readonly DiagnosticDescriptor InvalidArgumentRule = new(
        id: "MTC0001",
        title: GetLocalizedString(nameof(DiagnosticMessages.InvalidArgument_Title)),
        messageFormat: GetLocalizedString(nameof(DiagnosticMessages.InvalidArgument_MessageFormat)),
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: GetLocalizedString(nameof(DiagnosticMessages.InvalidArgument_Description)),
        helpLinkUri: _helpLinkUri
        );

    public static readonly DiagnosticDescriptor MissingIdentifierRule = new(
        id: "MTC0002",
        title: GetLocalizedString(nameof(DiagnosticMessages.MissingIdentifier_Title)),
        messageFormat: GetLocalizedString(nameof(DiagnosticMessages.MissingIdentifier_MessageFormat)),
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: GetLocalizedString(nameof(DiagnosticMessages.MissingIdentifier_Description)),
        helpLinkUri: _helpLinkUri
        );



    public static LocalizableString GetLocalizedString(string resourceKey)
       => new LocalizableResourceString(resourceKey, DiagnosticMessages.ResourceManager, typeof(DiagnosticMessages));

}
