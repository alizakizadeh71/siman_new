﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Message {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Global {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Global() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Message.Global", typeof(Global).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to درخواست شما معتبر نمی‌باشد!.
        /// </summary>
        public static string BadRequest {
            get {
                return ResourceManager.GetString("BadRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to نمايش خطاها.
        /// </summary>
        public static string DisplayErrors {
            get {
                return ResourceManager.GetString("DisplayErrors", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to خطا.
        /// </summary>
        public static string Error {
            get {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to دسترسی به اين صفحه مقدور نمی‌باشد!.
        /// </summary>
        public static string Forbidden {
            get {
                return ResourceManager.GetString("Forbidden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} صحیح وارد نشده است.
        /// </summary>
        public static string MissDataFormat {
            get {
                return ResourceManager.GetString("MissDataFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to صفحه مورد نظر يافت نشد!.
        /// </summary>
        public static string NotFound {
            get {
                return ResourceManager.GetString("NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}مورد نظر يافت نشد!.
        /// </summary>
        public static string RecordNotFound {
            get {
                return ResourceManager.GetString("RecordNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to تکمیل فیلد {0} الزامی است.
        /// </summary>
        public static string Required {
            get {
                return ResourceManager.GetString("Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to پیام های سیستمی.
        /// </summary>
        public static string SystemMessage {
            get {
                return ResourceManager.GetString("SystemMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to دسترسی به اين صفحه تا اطلاع ثانوی مقدور نمی‌باشد!.
        /// </summary>
        public static string TemporaryRedirect {
            get {
                return ResourceManager.GetString("TemporaryRedirect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to برای دسترسی به اين صفحه، بايد ابتدا وارد پايگاه شويد!.
        /// </summary>
        public static string Unauthorized {
            get {
                return ResourceManager.GetString("Unauthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to در فرایند انجام درخواست شما خطایی رخ داده است. این خطا در سیستم ثبت و توسط تیم پشتیبانی پیگیری خواهد شد..
        /// </summary>
        public static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
    }
}