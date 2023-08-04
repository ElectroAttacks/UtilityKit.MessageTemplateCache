﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessageTemplateCache.Analyzers.Ressources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MessageTemplateCache.Analyzers.Ressources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fix incorrect value.
        /// </summary>
        internal static string InvalidParameters_CodeFix {
            get {
                return ResourceManager.GetString("InvalidParameters_CodeFix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameters provided in &apos;CreateRequest()&apos; do not match the expected metadata values (CallerMemberName, CallerFilePath, and CallerLineNumber). This warning is raised when the parameters are deliberately set to obtain the template of another method. However, it might lead to unexpected behavior in the message template handling. Ensure that the provided parameters match the metadata automatically set by the compiler when not intentionally retrieving another method&apos;s template..
        /// </summary>
        internal static string InvalidParameters_Description {
            get {
                return ResourceManager.GetString("InvalidParameters_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect parameters provided in &apos;CreateRequest()&apos;. Parameter &apos;{0}&apos; has a value of &apos;{1}&apos; but an expected value of &apos;{2}&apos;.
        /// </summary>
        internal static string InvalidParameters_MessageFormat {
            get {
                return ResourceManager.GetString("InvalidParameters_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect values might be passed to &apos;CreateRequest()&apos;.
        /// </summary>
        internal static string InvalidParameters_Title {
            get {
                return ResourceManager.GetString("InvalidParameters_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove explicit parameters.
        /// </summary>
        internal static string NoExplicitParameters_CodeFix {
            get {
                return ResourceManager.GetString("NoExplicitParameters_CodeFix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Passing explicit parameters might result in unexpected behavior. To ensure that the correct message template is returned, you should call the method with no arguments..
        /// </summary>
        internal static string NoExplicitParameters_Description {
            get {
                return ResourceManager.GetString("NoExplicitParameters_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;CreateRequest()&apos; should be called without explicit parameters.
        /// </summary>
        internal static string NoExplicitParameters_MessageFormat {
            get {
                return ResourceManager.GetString("NoExplicitParameters_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;CreateRequest()&apos; should be called without explicit parameters.
        /// </summary>
        internal static string NoExplicitParameters_Title {
            get {
                return ResourceManager.GetString("NoExplicitParameters_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add unique identifier.
        /// </summary>
        internal static string UniqueIdentifiers_CodeFix {
            get {
                return ResourceManager.GetString("UniqueIdentifiers_CodeFix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using multiple message templates in the same method requires unique identifiers. To ensure that the correct template is returned, you must use a unique identifier and also pass it in the request through the &apos;WithIdentifier()&apos; method..
        /// </summary>
        internal static string UniqueIdentifiers_Description {
            get {
                return ResourceManager.GetString("UniqueIdentifiers_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method &apos;{0}&apos; has multiple message templates without unique identifiers. Ensure that each template has a unique identifier to avoid ambiguous results..
        /// </summary>
        internal static string UniqueIdentifiers_MessageFormat {
            get {
                return ResourceManager.GetString("UniqueIdentifiers_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use unique identifiers when using multiple message templates.
        /// </summary>
        internal static string UniqueIdentifiers_Title {
            get {
                return ResourceManager.GetString("UniqueIdentifiers_Title", resourceCulture);
            }
        }
    }
}