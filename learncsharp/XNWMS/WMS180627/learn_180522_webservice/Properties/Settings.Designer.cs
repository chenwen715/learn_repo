﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XNWMS.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172.16.5.28;Initial Catalog=XNWms;Persist Security Info=True;User ID=" +
            "sa;Password=abc123*")]
        public string sql {
            get {
                return ((string)(this["sql"]));
            }
            set {
                this["sql"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172.16.5.28;Initial Catalog=XinNingWms;Persist Security Info=True;Use" +
            "r ID=sa;Password=abc123*")]
        public string sql1 {
            get {
                return ((string)(this["sql1"]));
            }
            set {
                this["sql1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172.16.5.28;Initial Catalog=XinNingWcs;Persist Security Info=True;Use" +
            "r ID=sa;Password=abc123*")]
        public string sqlwcs {
            get {
                return ((string)(this["sqlwcs"]));
            }
            set {
                this["sqlwcs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.16.5.28:2000/api")]
        public string url {
            get {
                return ((string)(this["url"]));
            }
            set {
                this["url"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.16.5.28:9998/api")]
        public string url1 {
            get {
                return ((string)(this["url1"]));
            }
            set {
                this["url1"] = value;
            }
        }
    }
}
