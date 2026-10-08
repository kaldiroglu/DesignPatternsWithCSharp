namespace dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>The result of one export: a file name and its contents.</summary>
public sealed record Export(string FileName, string Content);
