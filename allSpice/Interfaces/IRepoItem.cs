using System;

namespace allSpice.Interfaces
{
  //We use 'T' to allow dynamic injection of data types.
  // In the class itself where we call the interface, we would say IRepoItem<int> or <string>
  public interface IRepoItem<T>
  {
    //in the interface, we don't define how a method works, only that it exists.
    //don't add props or methods on an interface that are not absolutely necessary to implement that interface.
    T Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }

  }
}