namespace dev.kaldiroglu.Composite.FileSystem.Iterator;

/// <summary>
/// An iterator over storage elements.
/// </summary>
/// <remarks>
/// The interface adds nothing to <see cref="IEnumerator{T}"/> and is here for the name: code
/// that takes an <c>IStorageIterator</c> says what it walks, and the file system can change
/// how it walks without touching that code.
/// </remarks>
public interface IStorageIterator : IEnumerator<IStorage>
{
}
