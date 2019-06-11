/*
Animal Shelter: An animal shelter, which holds only dogs and cats, operates on a strictly "first in, first out" basis. People must adopt either the "oldest" (based on arrival time) of all animals at the shelter, or they can select whether they would prefer a dog or a cat (and will receive the oldest animal of that type). They cannot select which specific animal they would like. Create the data structures to maintain this system and implement operations such as enqueue, dequeueAny, dequeueDog, and dequeueCat. You may use the built-in LinkedList data structure.

Undefined requirements:
  What should happen if a dequeue is called when there are no animals?
    - I will return a null
    - Alternative is to throw an exception
*/

public abstract class Animal
{
  public string Species { get; set; }
}

public class Dog : Animal
{
  public Dog()
  {
    Species = "dog";
  }
}

public class Cat : Animal
{
  public Cat()
  {
    Species = "cat";
  }
}

public class AnimalShelter
{
  // insert at end, remove from beginning
  private LinkedList<Animal> animals = new LinkedList<Animal>();

  public void Enqueue(Animal a) => animals.AddLast(a);

  public Animal DequeueAny()
  {
    Animal output = animals.First?.Value;
    if (output != null)
    {
      animals.RemoveFirst();
    }
    return output;
  }

  public Dog DequeueDog() => (Dog)DequeueSpecies("dog");

  public Cat DequeueCat() => (Cat)DequeueSpecies("cat");

  private Animal DequeueSpecies(string species)
  {
    LinkedListNode<Animal> outputNode = animals.First;
    while (outputNode != null)
    {
      if (outputNode.Value.Species == species)
      {
        animals.Remove(outputNode);
        break;
      }
      outputNode = outputNode.Next;
    }
    return outputNode?.Value;
  }
}
