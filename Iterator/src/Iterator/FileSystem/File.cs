namespace dev.kaldiroglu.Iterator.FileSystem;

// Inside this namespace `File` means this class, not System.IO.File.
public class File(string name, IStorage parent) : StorageElement(name, parent);
