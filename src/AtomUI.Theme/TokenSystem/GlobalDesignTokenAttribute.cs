﻿namespace AtomUI.Theme.TokenSystem;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class GlobalDesignTokenAttribute : Attribute
{
   public const string DefaultResourceCatalog = $"{ResourceCatalogConstants.Root}.{ResourceCatalogConstants.Token}";
   public string ResourceCatalog { get; }
   
   public GlobalDesignTokenAttribute(string resourceCatalog = DefaultResourceCatalog)
   {
      ResourceCatalog = resourceCatalog;
   }
}