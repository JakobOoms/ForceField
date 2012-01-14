// Type: System.Collections.ObjectModel.ReadOnlyCollection`1
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
  [ComVisible(false)]
  [DebuggerDisplay("Count = {Count}")]
  [DebuggerTypeProxy(typeof (Mscorlib_CollectionDebugView<>))]
  [Serializable]
  public class ReadOnlyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
  {
    private IList<T> list;
    [NonSerialized]
    private object _syncRoot;

    public int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        return this.list.Count;
      }
    }

    public T this[int index]
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        return this.list[index];
      }
    }

    protected IList<T> Items
    {
      get
      {
        return this.list;
      }
    }

    bool ICollection<T>.IsReadOnly
    {
      get
      {
        return true;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
        {
          ICollection collection = this.list as ICollection;
          if (collection != null)
            this._syncRoot = collection.SyncRoot;
          else
            Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        }
        return this._syncRoot;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        return true;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public ReadOnlyCollection(IList<T> list)
    {
      if (list == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
      this.list = list;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public bool Contains(T value)
    {
      return this.list.Contains(value);
    }

    public void CopyTo(T[] array, int index)
    {
      this.list.CopyTo(array, index);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public IEnumerator<T> GetEnumerator()
    {
      return this.list.GetEnumerator();
    }

    public int IndexOf(T value)
    {
      return this.list.IndexOf(value);
    }

    T IList<T>.get_Item(int index)
    {
      return this.list[index];
    }

    void IList<T>.set_Item(int index, T value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    void ICollection<T>.Add(T value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    void ICollection<T>.Clear()
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    void IList<T>.Insert(int index, T value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    bool ICollection<T>.Remove(T value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      return false;
    }

    void IList<T>.RemoveAt(int index)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.list.GetEnumerator();
    }

    void ICollection.CopyTo(Array array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      if (array.GetLowerBound(0) != 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      T[] array1 = array as T[];
      if (array1 != null)
      {
        this.list.CopyTo(array1, index);
      }
      else
      {
        Type elementType = array.GetType().GetElementType();
        Type c = typeof (T);
        if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        object[] objArray = array as object[];
        if (objArray == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        int count = this.list.Count;
        try
        {
          for (int index1 = 0; index1 < count; ++index1)
            objArray[index++] = (object) this.list[index1];
        }
        catch (ArrayTypeMismatchException ex)
        {
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
        }
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    object IList.get_Item(int index)
    {
      return (object) this.list[index];
    }

    void IList.set_Item(int index, object value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    int IList.Add(object value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
      return -1;
    }

    void IList.Clear()
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    private static bool IsCompatibleObject(object value)
    {
      if (value is T)
        return true;
      if (value == null)
        return (object) default (T) == null;
      else
        return false;
    }

    bool IList.Contains(object value)
    {
      if (ReadOnlyCollection<T>.IsCompatibleObject(value))
        return this.Contains((T) value);
      else
        return false;
    }

    int IList.IndexOf(object value)
    {
      if (ReadOnlyCollection<T>.IsCompatibleObject(value))
        return this.IndexOf((T) value);
      else
        return -1;
    }

    void IList.Insert(int index, object value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    void IList.Remove(object value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }

    void IList.RemoveAt(int index)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
    }
  }
}
