namespace dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>Someone who asks for an export. Only some users may export reports.</summary>
public sealed record User(string Name, bool MayExport);
