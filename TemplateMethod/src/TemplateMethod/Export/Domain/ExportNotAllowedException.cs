namespace dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>The user may not export reports. Nothing was made when this is thrown.</summary>
public sealed class ExportNotAllowedException(User user)
    : Exception(user.Name + " may not export reports");
