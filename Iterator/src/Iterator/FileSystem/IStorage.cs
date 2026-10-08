namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>The element type: anything that can be kept in a <see cref="Directory"/>.</summary>
public interface IStorage
{
    void Rename(string newName);

    void Save();

    void Delete();

    IStorage Copy();

    void Move(Directory target);
}
